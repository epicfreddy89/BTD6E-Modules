using Assets.Scripts.Models.Bloons;

namespace AdditionalBloons.Utils {
    public class BloonTask {
        public BloonModel model;
        public int amount;
        public int updatesBetween;

        public BloonTask(BloonModel model, int amount, int updatesBetween) {
            this.model = model;
            this.amount = amount;
            this.updatesBetween = updatesBetween * 10;
        }
    }
}