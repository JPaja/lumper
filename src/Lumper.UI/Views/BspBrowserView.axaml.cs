using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Lumper.UI.Views;

public partial class BmpBrowserView : UserControl
{
    public BmpBrowserView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}