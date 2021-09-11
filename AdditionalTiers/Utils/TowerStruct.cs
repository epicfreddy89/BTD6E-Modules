namespace AdditionalTiers.Utils {
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public unsafe struct TowerStruct {
        public IntPtr next;
        public TowerTask tower;
    }
}
