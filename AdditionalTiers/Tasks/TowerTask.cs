using System;
using System.Collections;
using System.Collections.Generic;
using AdditionalTiers.Utils;
using AdditionalTiers.Utils.Assets;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Unity.Bridge;

namespace AdditionalTiers.Tasks {
    public class TowerTask {
        public Func<TowerToSimulation, bool> requirements;
        public Action<TowerToSimulation> onComplete;
        public Action<TowerToSimulation> recurring;
        public Action<GameModel> gameLoad;
        public Action onLeave;
        public TowerModel getTower;
        public string identifier;
        public List<AssetInfo> assetsToRead = new();
    }
}