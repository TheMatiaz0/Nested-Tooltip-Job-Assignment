using Unity.BossRoom.Gameplay.Configuration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Action = Unity.BossRoom.Gameplay.Actions.Action;
using Unity.BossRoom.Gameplay.Actions;
using System.Collections.Generic;

namespace Unity.BossRoom.Gameplay.UI
{
    /// <summary>
    /// Controls the "information box" on the character-select screen.
    /// </summary>
    /// <remarks>
    /// This box also includes the "READY" button. The Ready button's state (enabled/disabled) is controlled
    /// here, but note that the actual behavior (when clicked) is set in the editor: the button directly calls
    /// ClientCharSelectState.OnPlayerClickedReady().
    /// </remarks>
    public class UICharSelectClassInfoBox : MonoBehaviour
    {
        [SerializeField]
        private ActionTooltipDatabase m_TooltipDatabase;
        [SerializeField]
        private TextMeshProUGUI m_WelcomeBanner;
        [SerializeField]
        private TextMeshProUGUI m_ClassLabel;
        [SerializeField]
        private GameObject m_HideWhenNoClassSelected;
        [SerializeField]
        private Image m_ClassBanner;
        [SerializeField]
        private Image m_Skill1;
        [SerializeField]
        private Image m_Skill2;
        [SerializeField]
        private Image m_Skill3;
        [SerializeField]
        private Image m_ReadyButtonImage;
        [SerializeField]
        private GameObject m_Checkmark;
        [SerializeField]
        [Tooltip("Message shown in the char-select screen. {0} will be replaced with the player's seat number")]
        [Multiline]
        private string m_WelcomeMsg = "Welcome, P{0}!";
        [SerializeField]
        [Tooltip("Format of tooltips. {0} is skill name, {1} is skill description. Html-esque tags allowed!")]
        [Multiline]
        private string m_TooltipFormat = "<b>{0}</b>\n\n{1}";

        private bool m_IsLockedIn = false;

        public void OnSetPlayerNumber(int playerNumber)
        {
            m_WelcomeBanner.text = string.Format(m_WelcomeMsg, (playerNumber + 1));
        }

        public void ConfigureForNoSelection()
        {
            m_HideWhenNoClassSelected.SetActive(false);
            SetLockedIn(false);
        }

        public void SetLockedIn(bool lockedIn)
        {
            m_ReadyButtonImage.color = lockedIn ? Color.green : Color.white;
            m_IsLockedIn = lockedIn;
            m_Checkmark.SetActive(lockedIn);
        }

        public void ConfigureForClass(CharacterClass characterClass)
        {
            m_HideWhenNoClassSelected.SetActive(true);

            m_Checkmark.SetActive(m_IsLockedIn);

            m_ClassLabel.text = characterClass.DisplayedName;
            m_ClassBanner.sprite = characterClass.ClassBannerLit;

            ConfigureSkillIcon(m_Skill1, characterClass.Skill1);
            ConfigureSkillIcon(m_Skill2, characterClass.Skill2);
            ConfigureSkillIcon(m_Skill3, characterClass.Skill3);
        }

        private void ConfigureSkillIcon(Image iconSlot, Action action)
        {
            if (action == null)
            {
                iconSlot.gameObject.SetActive(false);
            }
            else
            {
                iconSlot.gameObject.SetActive(true);
                iconSlot.sprite = action.Config.Icon;
                SetupTooltip(iconSlot, action);
            }
        }

        private void SetupTooltip(Image iconSlot, Action action)
        {
            if (!iconSlot.TryGetComponent<TooltipTrigger>(out var tooltipTrigger))
            {
                return;
            }

            var config = action.Config;
            var rootText = string.Format(m_TooltipFormat, config.DisplayedName,
                        string.Format(config.Description, config.Logic.ToString()));

            var currentTooltipSequence = new List<TooltipData>
            {
                new(rootText)
            };

            CreateTooltipChain(config, ref currentTooltipSequence);

            // TODO: push TooltipData sequence into the TooltipTrigger
            tooltipTrigger.UpdateText(rootText);
        }

        private string FormatTooltip(ActionConfig config, int projectileIndex = 0)
        {
            if (!m_TooltipDatabase.TryGetTooltipLinkData(config.Logic.ToString(), out var template))
            {
                Debug.Log($"Tooltip not found for action: {config.Logic}");
                return config.DisplayedName;
            }

            return ActionTooltipInjector
                .InjectIntoTemplate(template.Description, config, projectileIndex);
        }

        private void CreateTooltipChain(ActionConfig config, ref List<TooltipData> tooltips)
        {
            var configTooltipText = FormatTooltip(config);
            tooltips.Add(new(configTooltipText));

            if (config.Projectiles.Length > 1)
            {
                for (int i = 0; i < config.Projectiles.Length; i++)
                {
                    var projectileTooltipText = FormatTooltip(config, i);
                    tooltips.Add(new(projectileTooltipText));
                }
            }
        }
    }
}
