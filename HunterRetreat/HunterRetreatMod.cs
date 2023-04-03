using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using MelonLoader.Preferences;
using MelonLoader.Utils;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace HunterRetreat
{
    public class HunterRetreatMod : MelonMod
    {
        private MelonPreferences_Category modCategory;
        private MelonPreferences_Entry<int> healthLimit;
        private float healthLimitPercent;

        /// <inheritdoc/>
        public override void OnInitializeMelon()
        {
            MethodInfo method = typeof(VillagerRetreatSearchEntry).GetMethod(nameof(VillagerRetreatSearchEntry.HunterVillagerRetreatCheck), new Type[] { typeof(GameObject).MakeByRefType(), typeof(VillagerState.State).MakeByRefType() });
            MethodInfo prefix = typeof(VillagerRetreatSearchEntryPatch).GetMethod(nameof(VillagerRetreatSearchEntryPatch.HunterVillagerRetreatCheck_Prefix), BindingFlags.Public | BindingFlags.Static);
            LoggerInstance.Msg($"Patching method {method} with {prefix}");
            var patched = HarmonyInstance.Patch(method, new HarmonyMethod(prefix));
            LoggerInstance.Msg($"Patch return {patched}");

            LoggerInstance.Msg("Loading configuration ...");
            modCategory = MelonPreferences.CreateCategory("HunterRetreat");
            modCategory.SetFilePath(Path.Combine(MelonEnvironment.UserDataDirectory, "HunterRetreat.cfg"));
            healthLimit = modCategory.CreateEntry("HealthRetreatLimit", 30, "Health Limit Percentage", "Percentage of health above which a hunter will not retreat.", validator: new ValueRange<int>(0, 100));
            healthLimit.OnEntryValueChanged.Subscribe((_,_) => UpdateCachedHealthLimit());
            UpdateCachedHealthLimit();
            LoggerInstance.Msg($"Health limit is {healthLimit.Value}%.");

            VillagerRetreatSearchEntryPatch.Handler += VillagerRetreatSearchEntryPatch_Handler;
        }

        private bool VillagerRetreatSearchEntryPatch_Handler(Villager villager)
        {
            return villager.villagerHealth.health < healthLimitPercent;
        }

        private void UpdateCachedHealthLimit()
        {
            healthLimitPercent = healthLimit.Value / 100.0f;
        }
    }

    public static class VillagerRetreatSearchEntryPatch
    {
        public static event Func<Villager, bool> Handler;

        public static bool HunterVillagerRetreatCheck_Prefix(VillagerRetreatSearchEntry __instance, ref bool __result)
        {
            bool? shouldRetreat = Handler?.Invoke(__instance.villager);
            if (shouldRetreat.HasValue)
            {
                // If we decided it is OK to retreat, let the base logic determine how/where
                if (shouldRetreat.Value)
                {
                    return true;
                }
                else
                {
                    // Otherwise, just return false and move on
                    __result = false;
                    return false;
                }
            }

            return true;
        }
    }
}
