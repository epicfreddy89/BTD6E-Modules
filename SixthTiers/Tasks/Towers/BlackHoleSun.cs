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
using SixthTiers.Utils;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using v = Assets.Scripts.Simulation.SMath.Vector3;

namespace SixthTiers.Tasks.Towers {
    public class BlackHoleSun : TowerTask{
        public static TowerModel BHS;
        private static int time = -1;

        public BlackHoleSun()
        {
            identifier = "Black Hole Sun";
            getTower = BHS;
            requirements += tts => tts.tower.towerModel.baseId.Equals("DartMonkey") && tts.tower.towerModel.tiers[1] == 5 && tts.damageDealt > 100000;
            onComplete += tts => {
                if (time < 50)
                {
                    time++;
                    return;
                }
                tts.tower.namedMonkeyName = identifier;
                tts.tower.worth = 0;
                tts.tower.UpdateRootModel(BHS);
                tts.tower.display.SetScaleOffset(new(1.25f, 1.25f, 1.25f));
                tts.sim.simulation.CreateTextEffect(new(tts.position), "UpgradedText", 10, "Upgraded!", false);
                AbilityMenu.instance.TowerChanged(tts);
                AbilityMenu.instance.RebuildAbilities();
            };
            gameLoad += gm =>
            {
                BHS = gm.towers.First(a => a.name.Contains("DartMonkey-250")).Clone()
                    .Cast<TowerModel>();

                BHS.range = 150;
                BHS.cost = 0;
                BHS.name = "Black Hole Sun";
                BHS.display = "BlackHoleSun";
                BHS.dontDisplayUpgrades = true;
                BHS.portrait = new("BlackHoleSunIcon");
                BHS.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<DisplayModel>())
                    .Cast<DisplayModel>().display = "BlackHoleSun";
                var beh = BHS.behaviors;

                for (var i = 0; i < beh.Length; i++) {
                    var behavior = beh[i];
                    if (behavior.GetIl2CppType() == Il2CppType.Of<AttackModel>()) {
                        var am = behavior.Cast<AttackModel>();
                        am.range = 150;

                        for (var j = 0; j < am.weapons.Length; j++) {
                            am.weapons[j].emission = new AdoraEmissionModel("Adora_", 1, 0, null).Cast<EmissionModel>();

                            am.weapons[j].projectile.display = "BlackHoleSunProjectile";
                            am.weapons[j].projectile.pierce *= 500;
                            am.weapons[j].projectile.scale *= 1.125f;
                            am.weapons[j].rate /= 15;
                            am.weapons[j].rateFrames /= 15;

                            for (var k = 0; k < am.weapons[j].projectile.behaviors.Length; k++) {
                                if (am.weapons[j].projectile.behaviors[k].GetIl2CppType() == Il2CppType.Of<TravelStraitModel>()) {
                                    var p = am.weapons[j].projectile.behaviors[k].Cast<TravelStraitModel>();
                                    p.lifespan *= 100;
                                    p.Lifespan *= 100;
                                    p.lifespanFrames *= 100;
                                    p.speed *= 2;
                                    p.Speed *= 2;
                                    p.speedFrames *= 2;
                                    am.weapons[j].projectile.behaviors[k] = p;
                                }
                                if (am.weapons[j].projectile.behaviors[k].GetIl2CppType() == Il2CppType.Of<DamageModel>()) {
                                    var p = am.weapons[j].projectile.behaviors[k].Cast<DamageModel>();
                                    p.immuneBloonProperties = BloonProperties.Lead;
                                    p.damage *= 2;
                                    am.weapons[j].projectile.behaviors[k] = p;
                                }
                            }

                            am.weapons[j].projectile.behaviors = am.weapons[j].projectile.behaviors.Add(
                                new RotateModel("RotateModel_", 720).Cast<Model>(),
                                new TrackTargetWithinTimeModel("TrackTargetWithinTimeModel_",
                                    9999999, true, false, 365, false, 9999999, false, 3.48f, true).Cast<Model>());
                        }
                    }

                    if (behavior.GetIl2CppType() == Il2CppType.Of<AbilityModel>()) {
                        var a = behavior.Cast<AbilityModel>();

                        a.icon = new("BlackHoleSunIcon");
                        for (var j = 0; j < a.behaviors.Length; j++) {
                            var b = a.behaviors[j];

                            if (b.GetIl2CppType() == Il2CppType.Of<MonkeyFanClubModel>()) {
                                var mfcm = b.Cast<MonkeyFanClubModel>();

                                mfcm.display = "BlackHoleSunProjectile";
                                mfcm.towerOriginDisplay = "BlackHoleSun";
                                mfcm.bonusDamage += 1;
                                mfcm.bonusPierce += 1;
                                mfcm.maxTier = 5;
                                mfcm.range = 200;

                                b = mfcm;
                            }

                            a.behaviors[j] = b;
                        }

                        beh[i] = a;
                    }
                }

                BHS.behaviors = beh.Add(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            };
            recurring += tts => { };
            onLeave += () => { time = -1; };
            CacheBuilder.toBuild.PushAll("BlackHoleSun", "BlackHoleSunIcon", "BlackHoleSunProjectile");
            CacheBuilder.Build();
        }
    }
}