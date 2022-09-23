using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using GlobExpressions;
using Lumper.UI.ViewModels.Bsp.Lump;
using ReactiveUI;

namespace Lumper.UI.ViewModels.Bsp;

public class BspBrowserViewModel : ViewModelBase
{
    private readonly ObservableCollection<BspNode> _foundItems = new();
    private readonly BspViewModel _bspImage;
    private BspNode? _selectedNode;
    private string _searchText = "";
    private object _lock = new object();
    private int _foundSelectedIndex = 0;

#if DEBUG
    public BspBrowserViewModel()
    {
        _bspImage = DesignerModels.BspImage;
    }
#endif
    
    public BspBrowserViewModel(BspViewModel bspImage)
    {
        _bspImage = bspImage;
    }

    public BspViewModel BspImage => _bspImage;

    public BspNode? SelectedNode
    {
        get => _selectedNode;
        set => this.RaiseAndSetIfChanged(ref _selectedNode, value);
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if(value != _searchText)
                return;
            var reset = !value.StartsWith(_searchText);
            this.RaiseAndSetIfChanged(ref _searchText, value);
            if(!string.IsNullOrWhiteSpace(value))
                Task.Run(()=>Search(value.Trim(), reset));
        }
    }

    // ReSharper disable once InconsistentlySynchronizedField
    public ObservableCollection<BspNode> FoundNodes => _foundItems;

    public int FoundSelectedIndex
    {
        get => _foundSelectedIndex;
        set
        {
            if(value < 0 ||  value >= _foundItems.Count)
                return;
            if (value != _foundSelectedIndex)
                SelectedNode = _foundItems[value];
            this.RaiseAndSetIfChanged(ref _foundSelectedIndex, value);
        }
    }

    public void IncreaseFoundIndex() => FoundSelectedIndex++;
    public void DecreaseFoundIndex() => FoundSelectedIndex--;
    public bool CanIncrease => _foundSelectedIndex < _foundItems.Count - 1;
    public bool CanDecrease => _foundSelectedIndex > 0;
    public int UISelectedIndex => _foundItems.Any() 
        ? _foundSelectedIndex + 1
        : 0;
    
    
    private async Task Search(string pattern, bool reset = true)
    {
        IEnumerable<BspNode> searchCollection = _bspImage.Lumps;
        if (!reset)
            searchCollection = _foundItems.ToArray();
        lock (_lock)
        {
            _foundItems.Clear();
            var matcher = new Glob(pattern);
            Parallel.ForEach(searchCollection, 
                node => Search(node, matcher));
            if(_foundItems.Any())
                Dispatcher.UIThread.Post(()=>
                {
                    FoundSelectedIndex = 0;
                });
        }
    }
    
    private void Search(BspNode node, Glob matcher)
    {
        if(matcher.IsMatch(node.NodeName))
            Dispatcher.UIThread.Post(()=>_foundItems.Add(node));
        
        if(node.Nodes.Count != 0)
            return;
        Parallel.ForEach(node.Nodes, 
            childNode => Search(childNode, matcher));
    }
}