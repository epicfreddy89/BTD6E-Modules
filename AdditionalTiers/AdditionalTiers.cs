[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
[assembly: MelonInfo(typeof(AdditionalTiers.AdditionalTiers), "Additional Tier Addon", "1.6", "1330 Studios LLC")]

namespace AdditionalTiers {
    public sealed class AdditionalTiers : MelonMod {
        public static TowerTask[] Towers;
        public static string Version;

        public override void OnApplicationStart() {
            ErrorHandler.VALUE.Initialize();

            Version = Assembly.GetCustomAttribute<MelonInfoAttribute>().Version;

            List<TowerTask> towers = new();

            Assembly.GetTypes().AsParallel().ForAll(type => {
                if (typeof(TowerTask).IsAssignableFrom(type) && !typeof(TowerTask).FullName.Equals(type.FullName)) {
                    var tower = (TowerTask)Activator.CreateInstance(type);
                    if ((long)tower.tower != -1)
                        towers.Add(tower);
                }
            });

            Towers = towers.ToArray();

            if (!MelonPreferences.HasEntry("Additional Tier Addon Config", "Tier 6 required pop count multiplier")) {
                MelonPreferences.CreateCategory("Additional Tier Addon Config", "Additional Tier Addon Config");
                MelonPreferences.CreateEntry("Additional Tier Addon Config", "Tier 6 required pop count multiplier", (float)1);
                MelonPreferences.CreateEntry("Additional Tier Addon Config", "Display Format", ADisplay.style);
            }

            Globals.Load();

            HarmonyInstance.Patch(Method(typeof(Tower), nameof(Tower.Hilight)), postfix: new HarmonyMethod(Method(typeof(HighlightManager), nameof(HighlightManager.Highlight))));
            HarmonyInstance.Patch(Method(typeof(Tower), nameof(Tower.UnHighlight)), postfix: new HarmonyMethod(Method(typeof(HighlightManager), nameof(HighlightManager.UnHighlight))));

            LoggerInstance.Msg(ConsoleColor.Red, "Additional Tier Addon Loaded!");

            Logger13.Log("Success!");

            InternalVerification.Verify();

            CacheBuilder.Build();
            DisplayFactory.Build();
        }

        public override void OnApplicationQuit() {
            LoggerInstance.Msg($"Last Win32 Error - {Marshal.GetLastWin32Error()}");
            DisplayFactory.Flush();
            CacheBuilder.Flush();
        }

        public override void OnGUI() {
            var guiCol = GUI.color;
            GUI.color = new Color32(255, 255, 255, 50);
            var guiStyle = new GUIStyle
            {
                normal =
                {
                    textColor = Color.white
                }
            };
            GUI.Label(new Rect(0, Screen.height - 20, 100, 90), $"Additional Tiers v{Version}", guiStyle);
            GUI.color = guiCol;

            ErrorHandler.VALUE.OnGUI();
        }

        public override void OnUpdate() {
            if (!DisplayFactory.hasBeenBuilt)
                DisplayFactory.Build();

            if (InGame.instance == null || InGame.instance.bridge == null || InGame.instance.bridge.GetAllTowers() == null) return;

            var allTowers = InGame.instance.bridge.GetAllTowers();
            var allAdditionalTiers = Towers;
            for (var indexOfTowers = 0; indexOfTowers < allTowers.Count; indexOfTowers++) {
                var towerToSimulation = allTowers.ToArray()[indexOfTowers];
                if (towerToSimulation != null && !towerToSimulation.destroyed)
                    foreach (var addedTier in allAdditionalTiers) {
                        if (!addedTier.requirements(towerToSimulation)) continue;

                        var popsNeeded = (int) ((int) addedTier.tower * Globals.SixthTierPopCountMulti);

                        if (popsNeeded < towerToSimulation.damageDealt) {
                            if (!TransformationManager.VALUE.Contains(towerToSimulation.tower))
                                addedTier.onComplete(towerToSimulation);
                            else if (TransformationManager.VALUE.Contains(towerToSimulation.tower)) addedTier.recurring(towerToSimulation);
                        }
                        else if (!TransformationManager.VALUE.Contains(towerToSimulation.tower))
                            ADisplay.towerdata.Add((addedTier.identifier, towerToSimulation.damageDealt, popsNeeded));
                    }
            }
        }
    }
}