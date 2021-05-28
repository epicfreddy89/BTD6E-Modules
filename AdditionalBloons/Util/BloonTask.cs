using System;
using Assets.Scripts.Models.Bloons;
using Assets.Scripts.Simulation.Bloons;
using Assets.Scripts.Unity.Bridge;
using Newtonsoft.Json;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;

namespace AdditionalBloons.Utils {
    public class BloonTask {
        public BloonModel model;
        public int amount;
        public int updatesBetween;

        public BloonTask(BloonModel model, int amount, int updatesBetween) {
            this.model = model;
            this.amount = amount;
            this.updatesBetween = updatesBetween*10;
        }
    }
}