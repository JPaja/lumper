using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace Lumper.UI.ViewModels.Bsp;

public abstract class BspNode : ViewModelBase
{
    private ReadOnlyObservableCollection<BspNode>? _nodes = null;
    public abstract string NodeName { get;}
    public abstract string NodeIcon { get;}
    public ReadOnlyObservableCollection<BspNode>? Nodes => _nodes;

    internal void InitializeNodes<T>(ISourceList<T> list) where T : BspNode
    {
        if (_nodes is not null)
            throw new Exception("Nodes cannot be bound to multiple lists");
        list.Connect()
            .Transform(t => (BspNode) t)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _nodes)
            .DisposeMany()
            .Subscribe(_ => { }, RxApp.DefaultExceptionHandler.OnNext);
    }
}