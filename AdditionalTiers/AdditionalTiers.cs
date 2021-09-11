[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
[assembly: MelonInfo(typeof(AdditionalTiers.AdditionalTiers), "Additional Tier Addon", "1.4", "1330 Studios LLC")]

namespace AdditionalTiers {
    public sealed class AdditionalTiers : MelonMod {

        public static TowerTask[] towers;

        public override void OnApplicationStart() {
            var ttypes = Assembly.GetTypes().Where(a => typeof(TowerTask).IsAssignableFrom(a) && !typeof(TowerTask).FullName.Equals(a.FullName));
            List<TowerTask> tts = new();
            foreach (var type in ttypes) tts.Add((TowerTask)Activator.CreateInstance(type));
            towers = tts.ToArray();

            if (!MelonPreferences.HasEntry("Additional Tier Addon Config", "Tier 6 required pop count multiplier")) {
                MelonPreferences.CreateCategory("Additional Tier Addon Config", "Additional Tier Addon Config");
                MelonPreferences.CreateEntry("Additional Tier Addon Config", "Tier 6 required pop count multiplier", (float)1);
                MelonPreferences.CreateEntry("Additional Tier Addon Config", "Display Format", ADisplay.style);
            }

            Globals.Load();

            HarmonyInstance.Patch(Method(typeof(Tower), nameof(Tower.Hilight)), postfix: new HarmonyMethod(Method(typeof(HighlightManager), nameof(HighlightManager.Highlight))));
            HarmonyInstance.Patch(Method(typeof(Tower), nameof(Tower.UnHighlight)), postfix: new HarmonyMethod(Method(typeof(HighlightManager), nameof(HighlightManager.UnHighlight))));

            MelonLogger.Msg(ConsoleColor.Red, "Additional Tier Addon Loaded!");
            CacheBuilder.Build();
            Tasks.Assets.DisplayFactory.Build();
        }

        public override void OnApplicationQuit() {
            MelonLogger.Msg($"Last Win32 Error - {Marshal.GetLastWin32Error()}");
            Tasks.Assets.DisplayFactory.Flush();
            CacheBuilder.Flush();
        }


        public override void OnUpdate() {
            if (InGame.instance == null || InGame.instance.bridge == null || InGame.instance.bridge.GetAllTowers() == null) return;

            var allTowers = InGame.instance.bridge.GetAllTowers();
            var allAdditionalTiers = towers;
            for (var indexOfTowers = 0; indexOfTowers < allTowers.Count; indexOfTowers++) {
                var towerToSimulation = allTowers.ToArray()[indexOfTowers];
                if (towerToSimulation != null && !towerToSimulation.destroyed) {
                    for (var indexOfAdditionalTiers = 0; indexOfAdditionalTiers < allAdditionalTiers.Length; indexOfAdditionalTiers++) {
                        if (!allAdditionalTiers[indexOfAdditionalTiers].requirements(towerToSimulation)) continue;

                        var popsNeeded = (int)((int)allAdditionalTiers[indexOfAdditionalTiers].tower * Globals.SixthTierPopCountMulti);

                        if (popsNeeded < towerToSimulation.damageDealt) {
                            if (towerToSimulation.tower.namedMonkeyName != allAdditionalTiers[indexOfAdditionalTiers].identifier) {
                                allAdditionalTiers[indexOfAdditionalTiers].onComplete(towerToSimulation);
                            } else if (towerToSimulation.tower.namedMonkeyName == allAdditionalTiers[indexOfAdditionalTiers].identifier) {
                                allAdditionalTiers[indexOfAdditionalTiers].recurring(towerToSimulation);
                            }
                        } else {
                            if (towerToSimulation.tower.namedMonkeyName != allAdditionalTiers[indexOfAdditionalTiers].identifier)
                                ADisplay.towerdata.Add((allAdditionalTiers[indexOfAdditionalTiers].identifier, towerToSimulation.damageDealt, (int)allAdditionalTiers[indexOfAdditionalTiers].tower));
                        }
                    }
                }
            }
        }
    }
}