#if DEBUG
using System;
using DynamicData;
using Lumper.UI.ViewModels.Bsp;
using Lumper.UI.ViewModels.Bsp.Lump.Entity;

namespace Lumper.UI.ViewModels;

public static class DesignerModels
{
    static DesignerModels()
    {
        var rand = new Random();
        var bsp = new BspViewModel();
        for (int i = 0; i < 64; i++)
            bsp.Lumps[i] = new LumpEntityViewModel();
        for (int i = 0; i < 4; i++)
        {
            var entity = new EntityViewModel();
            ((LumpEntityViewModel)bsp.Lumps[0]!).Entities.Add(entity);
            for (int j = 0; j < rand.Next(5,10); j++)
            {
                var property = new EntityPropertyViewModel();
                property.Name = rand.Next().ToString();
                property.Value = rand.Next().ToString();
                entity.Properties.Add(property);
            }
        }
        BspImage = bsp;

    }
    public static BspViewModel BspImage { get; }
}
#endif
