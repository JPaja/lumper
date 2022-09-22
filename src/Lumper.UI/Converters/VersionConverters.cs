using System;
using Avalonia.Data.Converters;

namespace Lumper.UI.Converters;

public static class VersionConverters 
{
    /// <summary>
    /// A value converter that returns version string in format "Major".
    /// </summary>
    public static readonly IValueConverter ToStringMajor =
        new FuncValueConverter<Version?, string>(v => v is null ? string.Empty : v.ToString(1));

    /// <summary>
    /// A value converter that returns version string in format "Major.Minor".
    /// </summary>
    public static readonly IValueConverter ToStringMajorMinor =
        new FuncValueConverter<Version?, string>(v => v is null ? string.Empty : v.ToString(2));

    /// <summary>
    /// A value converter that returns version string in format "Major.Minor.Build".
    /// </summary>
    public static readonly IValueConverter ToStringMajorMinorBuild =
        new FuncValueConverter<Version?, string>(v => v is null ? string.Empty : v.ToString(3));

    /// <summary>
    /// A value converter that returns version string in format "Major.Minor.Build.Revision".
    /// </summary>
    public static readonly IValueConverter ToStringMajorMinorBuildRevision =
        new FuncValueConverter<Version?, string>(v => v is null ? string.Empty : v.ToString(4));
    
    /// <summary>
    /// A value converter that returns version.Major.
    /// </summary>
    public static readonly IValueConverter GetMajor =
        new FuncValueConverter<Version?, int?>(v => v?.Major);
    
    /// <summary>
    /// A value converter that returns version.Minor.
    /// </summary>
    public static readonly IValueConverter GetMinor =
        new FuncValueConverter<Version?, int?>(v => v?.Minor);
    
    /// <summary>
    /// A value converter that returns version.Build.
    /// </summary>
    public static readonly IValueConverter GetBuild =
        new FuncValueConverter<Version?, int?>(v => v?.Build);
    
    /// <summary>
    /// A value converter that returns version.Revision.
    /// </summary>
    public static readonly IValueConverter GetRevision =
        new FuncValueConverter<Version?, int?>(v => v?.Revision);

}