namespace AdditionalTiers.Tasks {
    public class GameI {
        [HarmonyPatch(typeof(GameModelLoader), nameof(GameModelLoader.Load))]
        public static class Loaded {
            [HarmonyPostfix]
            public static void Postfix(ref GameModel __result) {
                for (var i = 0; i < AdditionalTiers.towers.Length; i++) AdditionalTiers.towers[i].gameLoad(__result);
                /*var ts = AdditionalTiers.towers.Select(a => a.getTower()).ToArray();
                for (int i = 0; i < ts.Length; i++)
                    __result.towers = __result.towers.Add(ts);*/
            }
        }
    }
}