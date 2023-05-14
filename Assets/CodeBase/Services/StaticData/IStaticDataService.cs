#region

using System.Collections.Generic;
using CodeBase.StaticData.Ads;
using CodeBase.StaticData.Audio;
using CodeBase.StaticData.Dialog;
using CodeBase.StaticData.GamePlayMessage;
using CodeBase.StaticData.Inventory;
using CodeBase.StaticData.Items;
using CodeBase.StaticData.Level;
using CodeBase.StaticData.Npc;
using CodeBase.StaticData.Player;
using CodeBase.StaticData.SaveLoad;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Window;
using System.Threading.Tasks;
using UnityEngine;

#endregion

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        WindowConfig ForWindow(WindowId id);

        AudioClip ForAudio(AudioId id);

        LevelStaticData ForLevel(string sceneKey);

        NpcConfig ForNpc(NpcId id);

        InventoryConfig InventoryConfig { get; }
        PlayerStaticData PlayerStaticData { get; }
        DialogStaticData DialogStaticData { get; }
        GameplayMessageStaticData GameplayMessageData { get; }
        UILoadSaveStaticData UILoadSaveStaticData { get; }
        AdsConfig AdsStaticData { get; }

        BaseItem ForItem(ItemId id);

        Task Load();
        List<DialogContext> ForDialogContext(DialogId id);
    }
}