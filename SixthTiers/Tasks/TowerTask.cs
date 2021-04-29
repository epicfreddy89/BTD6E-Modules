using System;
using System.Collections;
using Assets.Scripts.Models;
using Assets.Scripts.Unity.Bridge;

namespace SixthTiers.Tasks {
    public class TowerTask {
        public Func<TowerToSimulation, bool> requirements;
        public Action<TowerToSimulation> onComplete;
        public Action<TowerToSimulation> recurring;
        public Action<GameModel> gameLoad;
        public Action onLeave;
        public string identifier;
    }
}