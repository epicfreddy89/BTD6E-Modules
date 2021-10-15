namespace AdditionalTiers.Tasks {
    public class GameI {
        [HarmonyPatch(typeof(GameModelLoader), nameof(GameModelLoader.Load))]
        public static class Loaded {
            [HarmonyPostfix]
            public static void Postfix(ref GameModel __result) {
                AddCoroutine(new ACoroutine(Timer.Run(AdditionalTiers.Towers.Select(a => a.gameLoad).ToArray(), __result), null));
            }
        }
    }
}