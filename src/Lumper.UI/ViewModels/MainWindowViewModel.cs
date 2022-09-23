using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Lumper.UI.ViewModels.Bsp;
using ReactiveUI;

namespace Lumper.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
#if DEBUG
            BspImage = DesignerModels.BspImage;
#endif
        }
        private BspViewModel? _bspImage;
        private BspBrowserViewModel? _bspBrowser;

        public Version? Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;

        public BspViewModel? BspImage
        {
            get => _bspImage;
            set
            {
                if (value != _bspImage)
                    this.RaiseAndSetIfChanged(ref _bspBrowser, value is null 
                        ? null 
                        : new BspBrowserViewModel(value));
                this.RaiseAndSetIfChanged(ref _bspImage, value);
            }
        }

        public BspBrowserViewModel? BspBrowser => _bspBrowser;
    }
}