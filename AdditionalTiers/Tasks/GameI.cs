namespace AdditionalTiers.Tasks {
    public class GameI {
        [HarmonyPatch(typeof(GameModelLoader), nameof(GameModelLoader.Load))]
        public static class Loaded {
            [HarmonyPostfix]
            public static void Postfix(ref GameModel __result) {
                AddCoroutine(new ACoroutine(Timer.Run(AdditionalTiers.towers.Select(a => a.gameLoad).ToArray(), __result), null));
                /*var ts = AdditionalTiers.towers.Select(a => a.getTower()).ToArray();
                for (int i = 0; i < ts.Length; i++)
                    __result.towers = __result.towers.Add(ts);*/
            }
        }
    }
}