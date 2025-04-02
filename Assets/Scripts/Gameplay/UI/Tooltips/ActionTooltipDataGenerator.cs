using Unity.BossRoom.Gameplay.Actions;

namespace Unity.BossRoom.Gameplay.UI
{
    /// <summary>
    /// Injects data from <see cref="ActionConfig">ActionConfig</see> using <see cref="ActionTooltipInjector">ActionTooltipInjector</see> and generates formatted text that can be used for tooltips.
    /// </summary>
    public static class ActionTooltipDataGenerator
    {
        public static TooltipData Generate(ActionConfig config)
        {
            var rootText = string.Format(TooltipSettings.Default.TooltipFormat, config.DisplayedName,
            string.Format(config.Description, config.Logic.ToString()));

            var rootTooltip = new TooltipData(rootText);
            var tooltipChain = CreateTooltipChain(config);

            rootTooltip.Attach(tooltipChain);

            return rootTooltip;
        }

        private static TooltipData CreateTooltipChain(ActionConfig config)
        {
            TooltipData rootTooltip = null;

            if (config.Projectiles.Length > 1)
            {
                var currentTooltip = rootTooltip;

                for (int i = 0; i < config.Projectiles.Length; i++)
                {
                    var nextTooltip = GetDataFromProjectile(config, i);
                    if (rootTooltip == null)
                    {
                        rootTooltip = nextTooltip;
                        currentTooltip = nextTooltip;
                    }
                    else
                    {
                        currentTooltip.Attach(nextTooltip);
                        currentTooltip = nextTooltip;
                    }
                }
            }
            else
            {
                string configTooltipText = InjectConfigIntoTemplate(config);
                if (configTooltipText == null)
                {
                    return null;
                }
                rootTooltip = new TooltipData(configTooltipText);
            }

            return rootTooltip;
        }

        private static string InjectConfigIntoTemplate(ActionConfig config, int projectileIndex = 0)
        {
            if (!TooltipSettings.Default.TooltipDatabase.TryGetTooltipLinkData(config.Logic.ToString(), out var template))
            {
                return null;
            }

            return ActionTooltipInjector
                .InjectIntoTemplate(template.Description, config, projectileIndex);
        }

        private static TooltipData GetDataFromProjectile(ActionConfig config, int projectileIndex)
        {
            string projectileTooltipText = InjectConfigIntoTemplate(config, projectileIndex);

            if (projectileIndex < config.Projectiles.Length - 1)
            {
                var nextProjectile = config.Projectiles[projectileIndex + 1];
                string linkText = GetNextProjectileLink(nextProjectile, config);
                projectileTooltipText = $"{projectileTooltipText} {linkText}";
            }

            return new TooltipData(projectileTooltipText);
        }

        private static string GetNextProjectileLink(ProjectileInfo nextProjectile, ActionConfig config)
        {
            return $"Turns into <link={config.Logic}><style=Clickable>{nextProjectile.ProjectilePrefab.name}</link></style> projectile upon {config.DurationSeconds / config.Projectiles.Length} seconds.";
        }
    }
}
