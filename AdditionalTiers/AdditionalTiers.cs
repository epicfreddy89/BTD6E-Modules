[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
[assembly: MelonInfo(typeof(AdditionalTiers.AdditionalTiers), "Additional Tier Addon", "1.5", "1330 Studios LLC")]

namespace AdditionalTiers {
    public sealed class AdditionalTiers : MelonMod {
        public static TowerTask[] Towers;

        public override void OnApplicationStart() {
            var asmTypes = Assembly.GetTypes();
            var ttypes = new Stack<SType>();
            for (int i = 0; i < asmTypes.Length; i++)
                if (typeof(TowerTask).IsAssignableFrom(asmTypes[i]) && !typeof(TowerTask).FullName.Equals(asmTypes[i].FullName))
                    ttypes.Push(asmTypes[i]);
            List<TowerTask> tts = new();
            foreach (var type in ttypes) {
                var tt = (TowerTask)Activator.CreateInstance(type);
                if (((long)tt.tower) != -1)
                    tts.Add(tt);
            }
            Towers = tts.ToArray();

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
            DisplayFactory.Build();
        }

        public override void OnApplicationQuit() {
            MelonLogger.Msg($"Last Win32 Error - {Marshal.GetLastWin32Error()}");
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
            GUI.Label(new Rect(0, Screen.height - 20, 100, 90), "Additional Tiers v1.5", guiStyle);
            GUI.color = guiCol;
        }

        public override void OnUpdate() {
            UpdateCoroutines();

            if (InGame.instance == null || InGame.instance.bridge == null || InGame.instance.bridge.GetAllTowers() == null) return;

            var allTowers = InGame.instance.bridge.GetAllTowers();
            var allAdditionalTiers = Towers;
            for (var indexOfTowers = 0; indexOfTowers < allTowers.Count; indexOfTowers++) {
                var towerToSimulation = allTowers.ToArray()[indexOfTowers];
                if (towerToSimulation != null && !towerToSimulation.destroyed)
                    for (var indexOfAdditionalTiers = 0; indexOfAdditionalTiers < allAdditionalTiers.Length; indexOfAdditionalTiers++) {
                        if (!allAdditionalTiers[indexOfAdditionalTiers].requirements(towerToSimulation)) continue;

                        var popsNeeded = (int) ((int) allAdditionalTiers[indexOfAdditionalTiers].tower * Globals.SixthTierPopCountMulti);

                        if (popsNeeded < towerToSimulation.damageDealt) {
                            if (!TransformationManager.VALUE.Contains(towerToSimulation.tower))
                                allAdditionalTiers[indexOfAdditionalTiers].onComplete(towerToSimulation);
                            else if (TransformationManager.VALUE.Contains(towerToSimulation.tower)) allAdditionalTiers[indexOfAdditionalTiers].recurring(towerToSimulation);
                        }
                        else if (!TransformationManager.VALUE.Contains(towerToSimulation.tower))
                            ADisplay.towerdata.Add((allAdditionalTiers[indexOfAdditionalTiers].identifier, towerToSimulation.damageDealt, (int) allAdditionalTiers[indexOfAdditionalTiers].tower));
                    }
            }
        }
    }
}