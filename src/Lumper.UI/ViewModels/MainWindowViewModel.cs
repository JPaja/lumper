using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Lumper.UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";

        public Version? Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;
    }
}