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
    public class WhiteAlbum : TowerTask {
        public static TowerModel whiteWedding;
        private static int time = -1;
        public WhiteAlbum() {
            identifier = "White Wedding";
            getTower = whiteWedding;
            requirements += tts => tts.tower.towerModel.baseId.Equals("SuperMonkey") && tts.tower.towerModel.tiers[2] == 5 && tts.damageDealt > ((int)AddedTierEnum.WHITEWEDDING) * Globals.SixthTierPopCountMulti;
            onComplete += tts => {
                if (time < 50) {
                    time++;
                    return;
                }
                tts.tower.namedMonkeyName = identifier;
                tts.tower.worth = 0;
                tts.tower.UpdateRootModel(whiteWedding);
                tts.sim.simulation.CreateTextEffect(new(tts.position), "UpgradedText", 10, "Upgraded!", false);
                AbilityMenu.instance.TowerChanged(tts);
                AbilityMenu.instance.RebuildAbilities();
            };
            gameLoad += gm => {
                whiteWedding = gm.towers.First(a => a.name.Contains("SuperMonkey-205")).Clone()
                    .Cast<TowerModel>();

                whiteWedding.range = 150;
                whiteWedding.cost = 0;
                whiteWedding.name = "White Wedding";
                whiteWedding.baseId = "WhiteWedding";
                whiteWedding.display = "WhiteWedding";
                whiteWedding.dontDisplayUpgrades = true;
                whiteWedding.portrait = new("WhiteWeddingIcon");
                whiteWedding.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<DisplayModel>())
                    .Cast<DisplayModel>().display = "WhiteWedding";
                var beh = whiteWedding.behaviors;
                ProjectileModel proj = null;
                for (var i = 0; i < beh.Length; i++) {
                    var behavior = beh[i];
                    if (behavior.GetIl2CppType() == Il2CppType.Of<AttackModel>()) {
                        var am = behavior.Cast<AttackModel>();

                        am.behaviors = am.behaviors.Remove(a => a.GetIl2CppType() == Il2CppType.Of<DisplayModel>());

                        for (var j = 0; j < am.weapons.Length; j++) {
                            am.weapons[j].projectile.display = "WhiteWeddingProjectile";
                            am.weapons[j].projectile.pierce *= 5;
                            am.weapons[j].projectile.scale *= 1.25f;
                            am.weapons[j].rate = 0;
                            am.weapons[j].rateFrames = 0;
                            proj = am.weapons[j].projectile.Clone().Cast<ProjectileModel>();
                            proj.display = "WhiteWeddingOrbitProjectile";
                            proj.pierce *= 10;
                            proj.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<DamageModel>())
                                .Cast<DamageModel>().damage *= 5;
                            proj.behaviors = proj.behaviors.Remove(a =>
                                a.GetIl2CppType() == Il2CppType.Of<TravelStraitModel>());
                            am.weapons[j].behaviors = am.weapons[j].behaviors
                                .Remove(a => a.GetIl2CppType() == Il2CppType.Of<EjectEffectModel>());
                        }

                        am.range = 150;
                        beh[i] = am;
                    }

                    beh[i] = behavior;
                }

                whiteWedding.behaviors = beh.Add(new OrbitModel("OrbitModel_", proj));
            };
            recurring += tts => {};
            onLeave += () => { time = -1; };
            CacheBuilder.toBuild.PushAll("WhiteWedding", "WhiteWeddingProjectile", "WhiteWeddingOrbitProjectile", "WhiteWeddingIcon");
        }
    }
}