namespace Lumper.Lib.BSP.Lumps.GameLumps
{
    public enum GameLumpType
    {
        Unknown = 0,
        sprp = 0x73707270, //(1936749168) static prop
        dprp = 0x64707270, // detail prop
        dplt = 0x64706c74, //detail prop lighting LDR
        dplh = 0x64706c68, //detail prop lighting HDR
    }
}
