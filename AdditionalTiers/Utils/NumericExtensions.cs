using System;

namespace AdditionalTiers.Utils {
    public static class NumericExtensions
    {
        public static float ToRadians(this float val) => 0.0174532925199433F * val;
    }
}