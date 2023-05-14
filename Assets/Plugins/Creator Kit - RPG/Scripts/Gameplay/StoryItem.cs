using Plugins.Creator_Kit___RPG.Scripts.Core;
using Plugins.Creator_Kit___RPG.Scripts.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Plugins.Creator_Kit___RPG.Scripts.Gameplay
{
    /// <summary>
    /// This class will trigger a text message when the player enters the trigger,
    /// and optionally start a cutscene.
    /// </summary>
    public class StoryItem : MonoBehaviour, ISerializationCallbackReceiver
    {
        public string ID;

        [Multiline]
        public string text = "There is no story to be found here.";

        public AudioClip audioClip;

        public bool disableWhenDiscovered = false;

        public HashSet<StoryItem> requiredStoryItems;
        public HashSet<InventoryItem> requiredInventoryItems;
        public Cutscene.Runtime.Cutscene cutscenePrefab;

        [System.NonSerialized] public HashSet<StoryItem> dependentStoryItems = new HashSet<StoryItem>();

        [SerializeField] private StoryItem[] _requiredStoryItems;
        [SerializeField] private InventoryItem[] _requiredInventoryItems;

        private GameModel model = Schedule.GetModel<GameModel>();

        private void OnEnable()
        {
            if (ID == string.Empty && text != null)
            {
                ID = $"SI:{text.GetHashCode()}";
            }
        }

        private void Awake()
        {
            ConnectRelations();
        }

        public void ConnectRelations()
        {
            foreach (var i in requiredStoryItems)
            {
                i.dependentStoryItems.Add(this);
            }
        }

        public void OnTriggerEnter2D(Collider2D collider)
        {
            if (!Application.isPlaying) return;

            foreach (var requiredInventoryItem in requiredInventoryItems)
                if (requiredInventoryItem != null)
                    if (!model.HasInventoryItem(requiredInventoryItem.name))
                        return;
            foreach (var requiredStoryItem in requiredStoryItems)
                if (requiredStoryItem != null)
                    if (!model.HasSeenStoryItem(requiredStoryItem.ID))
                        return;
            if (text != string.Empty)
                MessageBar.Show(text);
            if (ID != string.Empty)
                model.RegisterStoryItem(ID);
            if (audioClip == null)
                UserInterfaceAudio.OnStoryItem();
            else
                UserInterfaceAudio.PlayClip(audioClip);
            if (disableWhenDiscovered) gameObject.SetActive(false);
            if (cutscenePrefab != null)
            {
                var cs = Instantiate(cutscenePrefab);
                if (cs.audioClip != null)
                {
                    cs.OnFinish += (i) => model.musicController.CrossFade(model.musicController.audioClip);
                }
            }
        }

        public void OnBeforeSerialize()
        {
            if (requiredInventoryItems != null)
                _requiredInventoryItems = requiredInventoryItems.ToArray();

            if (requiredStoryItems != null)
                _requiredStoryItems = requiredStoryItems.ToArray();
        }

        public void OnAfterDeserialize()
        {
            requiredStoryItems = new HashSet<StoryItem>();
            if (_requiredStoryItems != null)
                foreach (var i in _requiredStoryItems) requiredStoryItems.Add(i);

            requiredInventoryItems = new HashSet<InventoryItem>();
            if (_requiredInventoryItems != null)
                foreach (var i in _requiredInventoryItems) requiredInventoryItems.Add(i);
        }
    }
}