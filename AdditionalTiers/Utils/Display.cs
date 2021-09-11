namespace AdditionalTiers.Utils {
    [HarmonyPatch(typeof(RoundDisplay), nameof(RoundDisplay.OnUpdate))]
    public sealed class Display {
        public static List<(string, long, int)> towerdata = new(); // Additional Tier name, cur pops, needed pops
        public static string style = "{0:n0}: {3:P2}";

        [HarmonyPostfix]
        public static void Fix(ref RoundDisplay __instance) {
            __instance.text.text = $"{__instance.cachedRoundDisp}\n";
            towerdata.Sort((s1, s2) => s1.Item1.CompareTo(s2.Item1));
            for (int i = 0; i < towerdata.Count; i++) {
                var optionalPercentage = (float)towerdata[i].Item2 / (float)towerdata[i].Item3;
                if (i < 5)
                    __instance.text.text += string.Format(style, towerdata[i].Item1, towerdata[i].Item2, towerdata[i].Item3, optionalPercentage) + "\n";
                else if (!Input.GetKey(KeyCode.F1)) { // slower than math so it gets its own exception outside of the main part of if
                    __instance.text.text += "Press and hold F1 for more...";
                    break;
                } else
                    __instance.text.text += string.Format(style, towerdata[i].Item1, towerdata[i].Item2, towerdata[i].Item3, optionalPercentage) + "\n";

            }
            towerdata.Clear();
        }
    }
}
