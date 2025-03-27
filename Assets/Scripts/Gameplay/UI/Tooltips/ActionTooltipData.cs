using System;
using Unity.BossRoom.Gameplay.Actions;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.UI
{
    [Serializable]
    public class ActionTooltipData
    {
        [field: SerializeField]
        public ActionLogic Id { get; private set; }
        [field: SerializeField]
        [field: TextArea]
        public string Description { get; private set; }
    }
}
