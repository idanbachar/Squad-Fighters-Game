using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadFighters
{
    public enum ServerMethod
    {
        None,
        StartDownloadMapData,
        MapDataDownloadCompleted,
        PlayerConnected,
        RemoveItem,
        UpdateItemCapacity,
        ShootData,
        Revive,
        PlayerData,
        DownloadingItem,
        JoinedMatch,
        PlayerKilled,
        PlayerDisconnected,
        PlayerDrown,
        TeamsCounts,
        ClientCreateItem
    }
}
