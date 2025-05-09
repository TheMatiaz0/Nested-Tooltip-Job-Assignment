using System.Collections.Generic;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.UI
{
    [CreateAssetMenu(fileName = "ActionTooltipDatabase", menuName = "Tooltips/Action Database")]
    public class ActionTooltipDatabase : ScriptableObject
    {
        [SerializeField]
        private List<ActionTooltipDefinition> m_ActionTooltips;

        [SerializeField]
        [TextArea]
        private string m_PositiveEffectDescription;

        public string PositiveEffectDescription => m_PositiveEffectDescription; 

        private Dictionary<string, ActionTooltipDefinition> m_TooltipDataDictionary;

        private void OnEnable()
        {
            SetupDictionary();
        }

        private void SetupDictionary()
        {
            m_TooltipDataDictionary = new();
            foreach (var data in m_ActionTooltips)
            {
                var actionKey = data.Id.ToString();
                m_TooltipDataDictionary[actionKey] = data;
            }
        }

        public bool TryGetTooltipLinkData(string linkId, out ActionTooltipDefinition data)
        {
            data = null;
            if (m_TooltipDataDictionary.TryGetValue(linkId, out var value))
            {
                data = value;
                return true;
            }
            return false;
        }
    }
}
