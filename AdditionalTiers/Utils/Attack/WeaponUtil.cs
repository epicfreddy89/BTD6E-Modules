namespace AdditionalTiers.Utils.Attack {
    internal static class WeaponPatch {
        internal static bool Prefix_Emit(AVector3 ejectPoint, int elapsed, Tower owner, SizedList<Projectile> created, bool doubleShot) {
            if (ejectPoint == null)
                return false;
            if (owner == null)
                return false;
            if (created == null)
                return false;

            return true;
        }
    }
}
