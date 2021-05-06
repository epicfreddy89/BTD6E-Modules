using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assets.Scripts.Models;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets.Scripts.Simulation.SMath;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.UI_New.InGame.AbilitiesMenu;
using Assets.Scripts.Utils;
using Harmony;
using Il2CppSystem;
using MelonLoader;
using AdditionalTiers.Utils;
using UnhollowerRuntimeLib;
using UnityEngine;
using v = Assets.Scripts.Simulation.SMath.Vector3;

namespace AdditionalTiers.Tasks.Towers.Tier6s {
    public class SuperFly : TowerTask {
        public static TowerModel superFly;
        private static int time = -1;
        public SuperFly() {
            identifier = "Super Fly";
            getTower = superFly;
            requirements += tts => tts.tower.towerModel.baseId.Equals("MonkeyVillage") && tts.tower.towerModel.tiers[0] == 5 && tts.damageDealt > ((int)AddedTierEnum.SUPERFLY) * Globals.SixthTierPopCountMulti;
            onComplete += tts => {
                if (time < 50) {
                    time++;
                    return;
                }
                tts.tower.namedMonkeyName = identifier;
                tts.tower.worth = 0;
                tts.tower.UpdateRootModel(superFly);
                tts.sim.simulation.CreateTextEffect(new(tts.position), "UpgradedText", 10, "Upgraded!", false);
                AbilityMenu.instance.TowerChanged(tts);
                AbilityMenu.instance.RebuildAbilities();
            };
            gameLoad += gm => {
                superFly = gm.towers.First(a => a.name.Contains("MonkeyVillage-520")).Clone()
                    .Cast<TowerModel>();

                superFly.range = 150;
                superFly.cost = 0;
                superFly.name = "Super Fly";
                superFly.baseId = "SuperFly";
                //superFly.display = "SuperFly";
                superFly.dontDisplayUpgrades = true;
                //superFly.portrait = new("superFlyIcon");
                //superFly.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<DisplayModel>()).Cast<DisplayModel>().display = "superFly";
                var beh = superFly.behaviors;
                ProjectileModel proj = null;
                for (var i = 0; i < beh.Length; i++) {
                    var behavior = beh[i];
                    if (behavior.GetIl2CppType() == Il2CppType.Of<AttackModel>()) {
                        var am = behavior.Cast<AttackModel>();

                        for (var j = 0; j < am.weapons.Length; j++) {
                            am.weapons[j].projectile.pierce *= 5;
                            am.weapons[j].projectile.scale *= 1.25f;
                            proj = am.weapons[j].projectile.Clone().Cast<ProjectileModel>();
                        }

                        beh[i] = am;
                    }

                    beh[i] = behavior;
                }

                superFly.behaviors = beh.Remove(a=>a.GetIl2CppType()==Il2CppType.Of<AttackModel>()).Add(new OrbitModel("OrbitModel_", proj));
            };
            recurring += tts => {};
            onLeave += () => { time = -1; };
            CacheBuilder.toBuild.PushAll();
        }
    }
}