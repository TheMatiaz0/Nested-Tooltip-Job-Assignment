using Unity.BossRoom.Gameplay.Configuration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Action = Unity.BossRoom.Gameplay.Actions.Action;
using Unity.BossRoom.Gameplay.Actions;

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
            if (!iconSlot.TryGetComponent<BaseTooltipTrigger>(out var tooltipTrigger))
            {
                return;
            }

            var config = action.Config;
            var rootText = string.Format(TooltipSettings.Default.TooltipFormat, config.DisplayedName,
                        string.Format(config.Description, config.Logic.ToString()));

            var rootTooltip = new TooltipData(rootText);
            var tooltipChain = CreateTooltipChain(config);

            rootTooltip.Attach(tooltipChain);
            tooltipTrigger.UpdateData(rootTooltip);
        }

        private string FormatTooltip(ActionConfig config, int projectileIndex = 0)
        {
            if (!m_TooltipDatabase.TryGetTooltipLinkData(config.Logic.ToString(), out var template))
            {
                return config.DisplayedName;
            }

            return ActionTooltipInjector
                .InjectIntoTemplate(template.Description, config, projectileIndex);
        }

        private TooltipData CreateTooltipChain(ActionConfig config)
        {
            var configTooltipText = FormatTooltip(config);
            var configTooltip = new TooltipData(configTooltipText);

            if (config.Projectiles.Length > 1)
            {
                var root = configTooltip;
                for (int i = 0; i < config.Projectiles.Length; i++)
                {
                    root = RefreshProjectile(root, config, i);
                }
            }

            return configTooltip;
        }

        private TooltipData RefreshProjectile(TooltipData root, ActionConfig config, int i)
        {
            var projectileTooltipText = FormatTooltip(config, i);

            if (i < config.Projectiles.Length - 1)
            {
                var nextProjectile = config.Projectiles[i + 1];
                var linkText = GetNextProjectileLink(nextProjectile, config);
                projectileTooltipText += " " + linkText;
            }

            var childTooltip = new TooltipData(projectileTooltipText);
            root.Attach(childTooltip);

            return root;
        }

        private string GetNextProjectileLink(ProjectileInfo nextProjectile, ActionConfig config)
        {
            return $"Turns into <link={config.Logic}><style=Clickable>{nextProjectile.ProjectilePrefab.name}</link></style> projectile upon {config.DurationSeconds / config.Projectiles.Length} seconds.";
        }
    }
}
