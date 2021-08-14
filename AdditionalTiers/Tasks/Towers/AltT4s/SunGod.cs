using AdditionalTiers.Utils;
using AdditionalTiers.Utils.Assets;
using AdditionalTiers.Utils.Towers;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using System.Linq;
using UnhollowerRuntimeLib;

namespace AdditionalTiers.Tasks.Towers.AltT4s {
    public class SunGod : TowerTask {
        public static TowerModel btd4SunGod;
        private static int time = -1;
        public SunGod() {
            identifier = "BTD4 Sun God";
            getTower = btd4SunGod;
            requirements += tts => tts.tower.towerModel.baseId.Equals("SuperMonkey") && tts.tower.towerModel.tiers[0] == 3 && tts.tower.towerModel.tiers[1] == 2 && tts.damageDealt > ((int)AddedTierEnum.BTD4SUNGOD) * Globals.SixthTierPopCountMulti;
            onComplete += tts => {
                if (time < 50) {
                    time++;
                    return;
                }
                tts.tower.namedMonkeyName = identifier;
                tts.tower.UpdateRootModel(btd4SunGod);
                tts.sim.simulation.CreateTextEffect(new(tts.position), "UpgradedText", 10, "???", false);
            };
            gameLoad += gm => {
                btd4SunGod = gm.towers.First(a => a.name.Contains(AddedTierName.BTD4SUNGOD)).Clone().Cast<TowerModel>();

                btd4SunGod.display = "BTD4SunGod";
                btd4SunGod.portrait = new("BTD4SunGodPor");
                btd4SunGod.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<DisplayModel>()).Cast<DisplayModel>().display = "BTD4SunGod";
                var beh = btd4SunGod.behaviors;
                for (var i = 0; i < beh.Length; i++) {
                    var behavior = beh[i];
                    if (behavior.GetIl2CppType() == Il2CppType.Of<AttackModel>()) {
                        var am = behavior.Cast<AttackModel>();

                        for (var j = 0; j < am.weapons.Length; j++) {
                            var weapon = am.weapons[j];

                            if (weapon.emission.Is<RandomArcEmissionModel>(out var raem)) {
                                raem.offset = 0;
                                raem.angle = 0;
                                raem.randomAngle = 5;
                                raem.startOffset = 0;
                                raem.offsetStart = 0;
                                raem.sliceSize = 0;

                                weapon.emission = raem;
                            }

                            am.weapons[j] = weapon;
                        }

                        beh[i] = am;
                    }

                    beh[i] = behavior;
                }

                btd4SunGod.behaviors = beh;
            };
            recurring += tts => { };
            onLeave += () => { time = -1; };
            assetsToRead.Add(new("BTD4SunGod", "e23d594d3bf5af44c8b1e2445fe10a9e", RendererType.SPRITERENDERER));
        }
    }
}
