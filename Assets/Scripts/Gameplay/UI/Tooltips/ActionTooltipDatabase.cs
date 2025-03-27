using System.Collections.Generic;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.UI
{
    [CreateAssetMenu(fileName = "ActionTooltipDatabase", menuName = "Tooltips/Action Database")]
    public class ActionTooltipDatabase : ScriptableObject
    {
        [SerializeField]
        private List<ActionTooltipData> m_ActionTooltips;

        private Dictionary<string, ActionTooltipData> m_TooltipDataDictionary;

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

        public ActionTooltipData GetTooltipLinkData(string linkId)
        {
            m_TooltipDataDictionary.TryGetValue(linkId, out var data);
            return data;
        }
    }
}
