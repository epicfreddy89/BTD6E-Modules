namespace AdditionalTiers.Tasks {
    public sealed class GameI {
        [HarmonyPatch(typeof(GameModelLoader), nameof(GameModelLoader.Load))]
        public static class Loaded {
            public static GameModel Model { get; set; }

            [HarmonyPostfix]
            public static void Postfix(ref GameModel __result) {
                Model = __result;

                AddCoroutine(new ACoroutine(Timer.Run(AdditionalTiers.Towers.Select(a => a.gameLoad).ToArray(), __result), null));
            }
        }
    }
}