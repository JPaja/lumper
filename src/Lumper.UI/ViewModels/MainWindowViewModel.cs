using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Lumper.Lib.BSP;
using Lumper.UI.ViewModels.Bsp;
using ReactiveUI;

namespace Lumper.UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private BspViewModel? _bspModel;
    private BspFile? _file;
    private BspNodeBase? _selectedNode;

    public MainWindowViewModel()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime
            desktop)
            throw new InvalidCastException(nameof(Application.Current.ApplicationLifetime));

        Desktop = desktop;

        SearchInit();
        TabsInit();
        IOInit();
    }

    public IClassicDesktopStyleApplicationLifetime Desktop { get; }

    public BspViewModel? BspModel
    {
        get => _bspModel;
        set => this.RaiseAndSetIfChanged(ref _bspModel, value);
    }

    public BspNodeBase? SelectedNode
    {
        get => _selectedNode;
        set => this.RaiseAndSetIfChanged(ref _selectedNode, value);
    }
}