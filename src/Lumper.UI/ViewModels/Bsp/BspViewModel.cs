using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using DynamicData;
using Lumper.Lib.BSP;
using Lumper.Lib.BSP.Lumps;
using Lumper.Lib.BSP.Lumps.BspLumps;
using Lumper.UI.Models;
using Lumper.UI.ViewModels.Bsp.Lumps;
using Lumper.UI.ViewModels.Bsp.Lumps.Entity;
using ReactiveUI;

namespace Lumper.UI.ViewModels.Bsp;

/// <summary>
///     View model for <see cref="Lumper.Lib.BSP.BspFile" />
/// </summary>
public class BspViewModel : BspNodeBase, IDisposable
{
    private readonly SourceList<LumpBase> _lumps = new();
    private IStorageFile? _file;

    public BspViewModel(MainWindowViewModel main, BspFile bspFile)
        : base(main)
    {
        BspFile = bspFile;
        NodeName = Path.GetFileName(bspFile.FilePath);
        foreach (var (key, value) in bspFile.Lumps)
            ParseLump(key, value);
        InitializeNodeChildrenObserver(_lumps);
    }

    public BspFile BspFile
    {
        get;
    }

    public override string NodeName
    {
        get;
    }

    public IStorageFile? File
    {
        get => _file;
        set
        {
            //Remove old file lock
            _file?.Dispose();
            this.RaiseAndSetIfChanged(ref _file, value);
            this.RaisePropertyChanged(nameof(FilePath));
        }
    }

    [NotNullIfNotNull(nameof(File))]
    public string? FilePath => _file?.Name;

    public void Dispose()
    {
        _lumps.Dispose();
        _file?.Dispose();
    }

    private void ParseLump(BspLumpType type, Lump<BspLumpType> lump)
    {
        LumpBase lumpModel = lump switch
        {
            EntityLump el => new EntityLumpViewModel(this, el),
            _ => new UnmanagedLumpViewModel(this, type)
        };
        _lumps.Add(lumpModel);
    }

    protected override ValueTask<bool> Match(Matcher matcher,
        CancellationToken? cancellationToken)
    {
        return ValueTask.FromResult(true);
    }
}
