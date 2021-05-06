
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Audio;
using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
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
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using v = Assets.Scripts.Simulation.SMath.Vector3;

namespace AdditionalTiers.Tasks.Towers.Tier6s {
    public class SkyHigh : TowerTask {
        public static TowerModel skyHigh;
        private static int time = -1;
        public SkyHigh() {
            identifier = "Sky High";
            getTower = skyHigh;
            requirements += tts => tts.tower.towerModel.baseId.Equals("DartMonkey") && tts.tower.towerModel.tiers[2] == 5 && tts.damageDealt > ((int)AddedTierEnum.SKYHIGH) * Globals.SixthTierPopCountMulti;
            onComplete += tts => {
                if (time < 50) {
                    time++;
                    return;
                }
                tts.tower.namedMonkeyName = identifier;
                tts.tower.worth = 0;
                tts.tower.UpdateRootModel(skyHigh);
                tts.sim.simulation.CreateTextEffect(new(tts.position), "UpgradedText", 10, "Upgraded!", false);
                AbilityMenu.instance.TowerChanged(tts);
                AbilityMenu.instance.RebuildAbilities();
            };
            gameLoad += gm => {
                skyHigh = gm.towers.First(a => a.name.Contains("DartMonkey-025")).Clone()
                    .Cast<TowerModel>();

                skyHigh.cost = 0;
                skyHigh.name = "Sky High";
                skyHigh.baseId = "SkyHigh";
                skyHigh.display = "SkyHigh";
                skyHigh.dontDisplayUpgrades = true;
                skyHigh.portrait = new("SkyHighIcon");
                skyHigh.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<DisplayModel>()).Cast<DisplayModel>().display = "SkyHigh";

                var beh = skyHigh.behaviors;

                for (var i = 0; i < beh.Length; i++)
                    if (beh[i].GetIl2CppType() == Il2CppType.Of<AttackModel>()) {
                        var am = beh[i].Cast<AttackModel>();

                        for (var j = 0; j < am.weapons.Length; j++) {
                            var we = am.weapons[j];
                            CritMultiplierModel cmm = null;

                            for (var k = 0; k < we.behaviors.Length; k++)
                                if (we.behaviors[k].GetIl2CppType() == Il2CppType.Of<CritMultiplierModel>()) {
                                    var web = we.behaviors[k].Cast<CritMultiplierModel>();

                                    web.display = "";
                                    web.damage = 100;
                                    web.lower = 10;
                                    web.upper = 10;
                                    cmm = web.Clone().Cast<CritMultiplierModel>();

                                    we.behaviors[k] = web;
                                }

                            cmm.name = "CritMultiplierModel__";
                            we.rate = 0.02f;
                            we.emission = new ArcEmissionModel("ArcEmissionModel_", 5, 0, 75, null, false, false).Cast<EmissionModel>();
                            we.behaviors = we.behaviors.Add(cmm);

                            we.projectile.pierce = 500000;
                            we.projectile.display = "SkyHighProj";

                            for (var k = 0; k < we.projectile.behaviors.Length; k++) {
                                if (we.projectile.behaviors[k].GetIl2CppType() == Il2CppType.Of<DamageModel>()) {
                                    var dm = we.projectile.behaviors[k].Cast<DamageModel>();

                                    dm.damage *= 2 * Globals.SixthTierDamageMulti;

                                    we.projectile.behaviors[k] = dm;
                                }
                                if (we.projectile.behaviors[k].GetIl2CppType() == Il2CppType.Of<DisplayModel>()) {
                                    var dm = we.projectile.behaviors[k].Cast<DisplayModel>();

                                    dm.display = "SkyHighProj";

                                    we.projectile.behaviors[k] = dm;
                                }
                                if (we.projectile.behaviors[k].GetIl2CppType() == Il2CppType.Of<TravelStraitModel>()) {
                                    var tsm = we.projectile.behaviors[k].Cast<TravelStraitModel>();

                                    tsm.lifespan *= 2;

                                    we.projectile.behaviors[k] = tsm;
                                }
                            }

                            am.weapons[j] = we;
                        }

                        beh[i] = am;
                    }

                skyHigh.behaviors = beh;
            };
            recurring += tts => {};
            onLeave += () => { time = -1; };
            CacheBuilder.toBuild.PushAll("SkyHigh", "SkyHighProj", "SkyHighIcon");
        }
    }
}