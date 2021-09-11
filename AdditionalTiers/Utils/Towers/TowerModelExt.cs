namespace AdditionalTiers.Utils.Towers {
    public static class TowerModelExt {
        public static void RebuildBehaviors(this TowerModel tower, params Model[] behaviors) => tower.RebuildBehaviorsA(a => { return false; }, behaviors);
        public static void RebuildBehaviorsA(this TowerModel tower, Func<Model, bool> removalAction, params Model[] behaviors) => tower.behaviors = tower.behaviors.Remove(removalAction).Add(behaviors);
        public static bool HasBehavior<T>(this TowerModel tower) => tower.behaviors.Any(m => m.GetIl2CppType().Equals(Il2CppType.Of<T>()));
        public static void SetDisplay(this TowerModel tower, string display) {
            tower.display = display;
            if (tower.HasBehavior<DisplayModel>())
                tower.behaviors.First(m => m.GetIl2CppType().Equals(Il2CppType.Of<DisplayModel>())).Cast<DisplayModel>().display = display;
        }
    }
}
