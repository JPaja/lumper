using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using Lumper.Lib.BSP;
using Lumper.Lib.BSP.Lumps;
using Lumper.Lib.BSP.Lumps.BspLumps;
using Lumper.UI.Models;
using Lumper.UI.ViewModels.Bsp.Lumps;
using Lumper.UI.ViewModels.Bsp.Lumps.Entity;
using ReactiveUI;

namespace Lumper.UI.ViewModels.Bsp;

public class BspViewModel : BspNodeBase
{
    private readonly SourceList<LumpBase> _lumps = new();
    private string? _filePath;

    public BspViewModel(MainWindowViewModel main, BspFile bspFile) : base(main)
    {
        BspFile = bspFile;
        NodeName = Path.GetFileName(bspFile.FilePath);
        foreach (var (key, value) in bspFile.Lumps)
            ParseLump(key, value);
        InitializeNodeChildrenObserver(_lumps);
    }

    public BspFile BspFile { get; }

    public override string NodeName { get; }

    public string? FilePath
    {
        get => _filePath;
        set => this.RaiseAndSetIfChanged(ref _filePath, value);
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

    protected override async ValueTask<bool> Match(Matcher matcher, CancellationToken? cancellationToken)
    {
        return true;
    }
}