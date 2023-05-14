using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData.Dialog
{
    [Serializable]
    public class DialogConfig
    {
        [SerializeField] private string _inspectorName;
        [field: SerializeField] public DialogId DialogId { get; private set; }
        [field: SerializeField] public List<DialogContext> Contexts { get; private set; }

        public void OnValidate()
        {
            _inspectorName = DialogId.ToString();
        }
    }
}