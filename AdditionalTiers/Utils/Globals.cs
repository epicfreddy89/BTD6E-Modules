namespace AdditionalTiers.Utils {
    internal static class Globals {
        public static float SixthTierPopCountMulti = 1;
        public static float SixthTierDamageMulti = 1;

        internal static void Load() {
            SixthTierPopCountMulti = MelonPreferences.GetEntryValue<float>("Additional Tier Addon Config", "Tier 6 required pop count multiplier");
            Display.style = MelonPreferences.GetEntryValue<string>("Additional Tier Addon Config", "Display Format");
        }
    }
}