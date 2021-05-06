using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assets.Scripts.Models;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Towers.Mods;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.Weapons;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets.Scripts.Simulation.Factory;
using Assets.Scripts.Simulation.Objects;
using Assets.Scripts.Simulation.SMath;
using Assets.Scripts.Simulation.Towers.Behaviors.Abilities;
using Assets.Scripts.Simulation.Towers.Filters;
using Assets.Scripts.Simulation.Towers.Projectiles;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Bridge;
using Assets.Scripts.Unity.Network;
using Assets.Scripts.Unity.Towers.Emissions;
using Assets.Scripts.Unity.UI_New.InGame;
using Assets.Scripts.Unity.UI_New.InGame.AbilitiesMenu;
using Assets.Scripts.Utils;
using Harmony;
using Il2CppSystem;
using MelonLoader;
using AdditionalTiers.Utils;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using v = Assets.Scripts.Simulation.SMath.Vector3;

namespace AdditionalTiers.Tasks.Towers.Tier6s {
    public class BigJuggus : TowerTask {
        public static TowerModel bigJuggus;
        private static int time = -1;
        public BigJuggus() {
            identifier = "Big Juggus";
            getTower = bigJuggus;
            requirements += tts => tts.tower.towerModel.baseId.Equals("DartMonkey") && tts.tower.towerModel.tiers[0] == 5 && tts.damageDealt > 100000;
            onComplete += tts => {
                if (time < 50) {
                    time++;
                    return;
                }
                tts.tower.namedMonkeyName = identifier;
                tts.tower.worth = 0;
                tts.tower.UpdateRootModel(bigJuggus);
                tts.tower.display.SetScaleOffset(new(1.25f, 1.25f, 1.25f));
                tts.sim.simulation.CreateTextEffect(new(tts.position), "UpgradedText", 10, "Upgraded!", false);
                AbilityMenu.instance.TowerChanged(tts);
                AbilityMenu.instance.RebuildAbilities();
            };
            gameLoad += gm => {
                bigJuggus = gm.towers.First(a => a.name.Contains("DartMonkey-520")).Clone().Cast<TowerModel>();

                bigJuggus.range = 150;
                bigJuggus.cost = 0;
                bigJuggus.name = "Big Juggus";
                bigJuggus.baseId = "DartMonkey";
                bigJuggus.display = "BigJuggus";
                bigJuggus.dontDisplayUpgrades = true;
                bigJuggus.portrait = new("BigJuggusIcon");
                bigJuggus.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<DisplayModel>())
                    .Cast<DisplayModel>().display = "BigJuggus";
                bigJuggus.mods = bigJuggus.mods.Add(new ApplyModModel("ApplyModModel_", "BigJuggus", ""));
                var beh = bigJuggus.behaviors;
                ProjectileModel proj = null;
                AttackModel attack = null;
                var speed = 0f;
                for (var i = 0; i < beh.Length; i++) {
                    var behavior = beh[i];
                    if (behavior.GetIl2CppType() == Il2CppType.Of<AttackModel>()) {
                        var am = behavior.Cast<AttackModel>();

                        for (var j = 0; j < am.weapons.Length; j++) {
                            proj = am.weapons[j].projectile.Clone().Cast<ProjectileModel>();
                            am.weapons[j].projectile.display = "BigJuggusProj";
                            am.weapons[j].projectile.pierce *= 5;
                            am.weapons[j].projectile.scale *= 1.125f;
                            am.weapons[j].emission = new ArcEmissionModel("ArcEmissionModel_", 3, 0, 150, null, false, false).Cast<EmissionModel>();
                            am.weapons[j].rate /= 2;
                            am.weapons[j].rateFrames /= 2;
                            speed = am.weapons[j].rate;
                            for (var k = 0; k < am.weapons[j].projectile.behaviors.Length; k++) {
                                var b = am.weapons[j].projectile.behaviors[k];
                                if (b.GetIl2CppType() == Il2CppType.Of<CreateProjectileOnExhaustFractionModel>()) {
                                    var p = b.Cast<CreateProjectileOnExhaustFractionModel>();
                                    var em = p.emission.Cast<ArcEmissionModel>();
                                    em.Count = 4;
                                    p.emission = em;
                                    p.projectile = proj;
                                    am.weapons[j].projectile.behaviors[k] = p;
                                }

                                if (b.GetIl2CppType() == Il2CppType.Of<DamageModel>()) {
                                    var p = b.Cast<DamageModel>();
                                    p.damage *= 3;;
                                    am.weapons[j].projectile.behaviors[k] = p;
                                }
                            }
                        }

                        am.range = 150;
                        beh[i] = am;
                        attack = am.Clone().Cast<AttackModel>();
                    }

                    beh[i] = behavior;
                }

                for (var j = 0; j < attack.weapons.Length; j++) {
                    attack.weapons[j].emission = new SingleEmissionModel("SingleEmissionModel_", null).Cast<EmissionModel>();
                    attack.weapons[j].projectile.scale *= 2;
                    attack.weapons[j].projectile.ignorePierceExhaustion = true;
                    for (var i = 0; i < attack.weapons[j].projectile.behaviors.Length; i++) {
                        var b = attack.weapons[j].projectile.behaviors[i];
                        if (b.GetIl2CppType() == Il2CppType.Of<DamageModel>()) {
                            var p = b.Cast<DamageModel>();
                            p.damage = 25000;
                            attack.weapons[j].projectile.behaviors[i] = p;
                        }

                        if (b.GetIl2CppType() == Il2CppType.Of<TravelStraitModel>()) {
                            var a = b.Cast<TravelStraitModel>();
                            a.lifespan *= 5;
                            a.speed /= 4;
                            a.speedFrames /= 4;
                            attack.weapons[j].projectile.behaviors[i] = a;
                        }
                    }
                }

                var ability = new AbilityModel("AbilityModel_", "Meteor Strike",
                    "Launches a meteor sized projectile to the bloons", 1, 0, new("BigJuggusIcon"), 80,
                    new (new []{new ActivateAttackModel("ActivateAttackModel_", 1, false, new(new []{attack}), true, false, false, false, true)}),
                    false, false, "BigJuggus", 0, 0, 2, false, false);
                var mfc = new MonkeyFanClubModel("MonkeyFanClubModel_", 15, 5, 50, 160, speed, BloonProperties.None,
                    "SmallerJuggusProj", "SmallerJuggus", "", "SmallerJuggus", "9fdef1ee822fdae49a3ed3ba322d17d9",
                    "ef97abc30fa87e54f9f6ab4813f0fec9", "d25a04e9666f523488a19690b2806f64",
                    new("EE_", "d25a04e9666f523488a19690b2806f64", 1, 0.8f, false, false, false, false, false,
                        false, false), 60, 6, 250, null, 0, 0, 0, "BigJuggus");
                var ability2 = new AbilityModel("AbilityModel_2", "Juggus Conversion",
                    "Turns nearby monkeys into Smaller Juggus", 1, 0, new("SmallerJuggusIcon"), 45,
                    new(new[] {mfc}), false, false, "BigJuggus", 0, 0, 5, false, false);

                bigJuggus.behaviors = beh.Add(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true).Cast<Model>(), ability.Cast<Model>(), ability2.Cast<Model>());
            };
            recurring += tts => { };
            onLeave += () => { time = -1; };
            CacheBuilder.toBuild.PushAll("BigJuggus", "BigJuggusProj", "BigJuggusIcon", "SmallerJuggus", "SmallerJuggusProj", "SmallerJuggusIcon");
            CacheBuilder.Build();
        }
    }
}