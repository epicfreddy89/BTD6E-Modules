using System;
using MelonLoader;

namespace AdditionalTiers.Utils {
    internal static class Globals {
        public static Single SixthTierPopCountMulti = 1;
        public static Single SixthTierDamageMulti = 1;

        internal static void Load() {
            SixthTierPopCountMulti = MelonPreferences.GetEntryValue<float>("Additional Tier Addon Config", "Tier 6 required pop count multiplier");
            SixthTierDamageMulti = MelonPreferences.GetEntryValue<float>("Additional Tier Addon Config", "Tier 6 damage multiplier");
        }
    }
}