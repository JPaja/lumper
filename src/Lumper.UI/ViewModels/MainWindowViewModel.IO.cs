using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Lumper.Lib.BSP;
using Lumper.UI.ViewModels.Bsp;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using ReactiveUI;

namespace Lumper.UI.ViewModels;

public partial class MainWindowViewModel
{
    private void IOInit()
    {
        this.WhenAnyValue(x => x.BspModel)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Where(m => m is not null)
            .Subscribe(x => BspModel!.RaisePropertyChanged(nameof(BspModel.FilePath)));

        RxApp.MainThreadScheduler.Schedule(OnLoad);
    }

    private static OpenFileDialog ConstructOpenBspDialog()
    {
        var dialog = new OpenFileDialog();
        var bspFilter = new FileDialogFilter { Name = "Bsp file" };
        bspFilter.Extensions.Add("bsp");
        var anyFilter = new FileDialogFilter { Name = "All files" };
        anyFilter.Extensions.Add("*");
        dialog.Filters!.Add(bspFilter);
        dialog.Filters!.Add(anyFilter);
        dialog.AllowMultiple = false;
        dialog.Title = "Pick bsp file";
        return dialog;
    }

    private static SaveFileDialog ConstructSaveBspDialog()
    {
        var dialog = new SaveFileDialog();
        var bspFilter = new FileDialogFilter { Name = "Bsp file" };
        bspFilter.Extensions.Add("bsp");
        dialog.Filters!.Add(bspFilter);
        dialog.Title = "Pick bsp file";
        return dialog;
    }

    public async ValueTask OpenCommand()
    {
        if (Desktop.MainWindow is null)
            return;
        var dialog = ConstructOpenBspDialog();
        var result = await dialog.ShowAsync(Desktop.MainWindow);
        if (result is not { Length: 1 })
            return;
        var path = result.First();
        await LoadBsp(path);
    }

    public async ValueTask SaveCommand()
    {
        if (_bspModel is null)
            return;

        if (_bspModel.FilePath is null)
        {
            if (Desktop.MainWindow is null)
                return;
            var dialog = ConstructSaveBspDialog();
            var result = await dialog.ShowAsync(Desktop.MainWindow);
            if (result is null)
                return;
            Save(result);
        }
        else
        {
            Save(_bspModel.FilePath);
        }
    }

    public async ValueTask SaveAsCommand()
    {
        if (Desktop.MainWindow is null || _bspModel is null)
            return;
        var dialog = ConstructSaveBspDialog();
        var result = await dialog.ShowAsync(Desktop.MainWindow);
        if (result is null)
            return;
        Save(result);
    }

    private async void Save(string path)
    {
        if (_bspModel is null)
            return;

        try
        {
            //TODO: Copy bsp model tree for fallback if error occurs
            _bspModel.Update();
            _bspModel.BspFile.Save(path);
        }
        catch (Exception e)
        {
            MessageBoxManager.GetMessageBoxStandardWindow("Error",
                $"Error while saving file \n{e.Message}");
            return;
        }

        _bspModel.FilePath = path;
    }

    private async ValueTask LoadBsp(string path)
    {
        if (!File.Exists(path))
            return;
        var file = new BspFile();
        file.Load(path);
        _file = file;
        BspModel = new BspViewModel(this, file);
    }


    public async Task CloseCommand()
    {
        if (_bspModel is null || !_bspModel.IsModifiedRecursive)
            return;

        var messageBox = MessageBoxManager
            .GetMessageBoxStandardWindow("You have unsaved changes",
                "Do you want to discard changes?", ButtonEnum.OkCancel);
        var result = await messageBox.ShowDialog(Desktop.MainWindow);
        if (result != ButtonResult.Ok)
            return;
        BspModel = null;
    }

    public async Task ExitCommand()
    {
        Desktop.MainWindow?.Close();
    }

    public async Task OnClose(CancelEventArgs e)
    {
        e.Cancel = true;
        if (_bspModel is not null && _bspModel.IsModifiedRecursive)
        {
            var messageBox = MessageBoxManager
                .GetMessageBoxStandardWindow("You have unsaved changes",
                    "Do you want to close application without saving?", ButtonEnum.OkCancel);
            var result = await messageBox.ShowDialog(Desktop.MainWindow);
            if (result != ButtonResult.Ok)
                return;
        }

        //Since we have to cancel closing event on start due to not being able to await on Event
        //and message box cannot work in synchronous mode due to main window thread being frozen,
        //we have to manually close process. (Window.Close() would recursively call OnClose function)
        Environment.Exit(1);
    }
}