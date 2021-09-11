namespace AdditionalTiers.Tasks {
    [StructLayout(LayoutKind.Sequential)]
    public unsafe class TowerTask {
        public Func<TowerToSimulation, bool> requirements;
        public Action<TowerToSimulation> onComplete;
        public Action<TowerToSimulation> recurring;
        public Action<GameModel> gameLoad;
        public Action onLeave;
        public Func<TowerModel> getTower;
        public string identifier;
        public AssetStack assetsToRead = new();
        public string baseTower;
        public AddedTierEnum tower;
    }
}