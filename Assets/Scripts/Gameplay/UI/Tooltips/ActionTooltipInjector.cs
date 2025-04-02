using System;
using System.Collections.Generic;
using Unity.BossRoom.Gameplay.Actions;

namespace Unity.BossRoom.Gameplay.UI
{
    public static class ActionTooltipInjector
    {
        private static readonly Dictionary<string, Func<ActionConfig, int, string>> k_TemplateMapping = new()
        {
            { "{Damage}", (config, index) => index < config.Projectiles.Length
                ? FormatDamageToFriendly(config.Projectiles[index].Damage, config)
                : FormatDamageToFriendly(config.Amount, config) },
            { "{Speed}", (config, index) => index < config.Projectiles.Length ? config.Projectiles[index].Speed_m_s.ToString() : "N/A" },
            { "{Pierce}", (config, index) => index < config.Projectiles.Length ? config.Projectiles[index].MaxVictims.ToString() : "N/A" },
            { "{StartTime}", (config, index) => config.ExecTimeSeconds.ToString() },
            { "{Radius}", (config, index) => config.Radius.ToString() },
            { "{Duration}", (config, index) => config.DurationSeconds.ToString() },
            { "{Reuse}", (config, index) => config.ReuseTimeSeconds.ToString() }
        };

        private static string FormatDamageToFriendly(int damage, ActionConfig config)
        {
            return config.IsFriendly ? (-damage).ToString() : damage.ToString();
        }

        public static string InjectIntoTemplate(string template, ActionConfig config, int projectileIndex = 0)
        {
            foreach (var data in k_TemplateMapping)
            {
                template = template.Replace(data.Key, data.Value(config, projectileIndex));
            }

            return template;
        }
    }
}
