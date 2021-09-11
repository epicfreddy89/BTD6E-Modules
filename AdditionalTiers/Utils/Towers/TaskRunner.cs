namespace AdditionalTiers.Utils.Towers {
    public class TaskRunner {

        [HarmonyPatch(typeof(InGame), nameof(InGame.Quit))]
        [HarmonyPatch(typeof(InGame), nameof(InGame.Restart))]
        private sealed class Reset {
            [HarmonyPostfix]
            private static void RunLeave() {
                var allAdditionalTiers = AdditionalTiers.towers;
                for (var towerIndex = allAdditionalTiers.Length - 1; towerIndex >= 0; towerIndex--)
                    allAdditionalTiers[towerIndex].onLeave();
            }
        }
    }
}