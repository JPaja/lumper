using Lumper.Core.BSP;
using System.IO;
using Xunit;

namespace Lumper.Core.Tests;

public class BspFileTests
{
    [Fact]
    public void Parse_TriggerTests()
    {
        var data = Properties.Resources.triggertests;
        var bsp = BspImage.FromStream(new MemoryStream(data));

        Assert.NotNull(bsp);
    }
}
