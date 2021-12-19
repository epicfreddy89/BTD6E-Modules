using MelonLoader;


[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
[assembly: MelonInfo(typeof(PetTowers.PetTowers), "Pet Towers", "1.6", "1330 Studios LLC")]
namespace PetTowers {
    public class PetTowers : MelonMod {

        public override void OnApplicationStart() {
            MelonLogger.Msg("Pet Towers loaded!");
        }
    }
}
