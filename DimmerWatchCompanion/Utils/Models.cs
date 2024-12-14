using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DimmerWatchCompanion.Utils;
public partial class SongModelView :ObservableObject
{

    [ObservableProperty]
    string? title;
    [ObservableProperty]
    string? filePath;
    [ObservableProperty]
    string? artistName;
    [ObservableProperty]
    string? albumName;
    [ObservableProperty]
    bool isCurrentPlayingHighlight;
    [ObservableProperty]
    string latestUpdateBy = DeviceInfo.Platform.ToString();
    [ObservableProperty]
    string? localDeviceId;

    // Override Equals to compare based on string
    public override bool Equals(object? obj)
    {
        if (obj is SongModelView other)
        {
            return this.LocalDeviceId == other.LocalDeviceId;
        }
        return false;
    }

    // Override GetHashCode to use string's hash code
    public override int GetHashCode()
    {
        return LocalDeviceId!.GetHashCode();
    }
}
