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
using AdditionalTiers.Utils;
using UnhollowerRuntimeLib;
using UnityEngine;
using v = Assets.Scripts.Simulation.SMath.Vector3;

namespace AdditionalTiers.Tasks.Towers.Tier6s {
    public class Yellow_Submarine : TowerTask {
        public static TowerModel yellowSubmarine;
        private static int time = -1;
        public Yellow_Submarine() {
            identifier = "Yellow Submarine";
            getTower = yellowSubmarine;
            requirements += tts => tts.tower.towerModel.baseId.Equals("MonkeySub") && tts.tower.towerModel.tiers[1] == 5 && tts.damageDealt > ((int)AddedTierEnum.YELLOWSUBMARINE) * Globals.SixthTierPopCountMulti;
            onComplete += tts => {
                if (time < 50)
                {
                    time++;
                    return;
                }
                tts.tower.namedMonkeyName = identifier;
                tts.tower.worth = 0;
                tts.tower.UpdateRootModel(yellowSubmarine);
                tts.sim.simulation.CreateTextEffect(new(tts.position), "UpgradedText", 10, "Upgraded!", false);
                AbilityMenu.instance.TowerChanged(tts);
                AbilityMenu.instance.RebuildAbilities();
            };
            gameLoad += gm => {
                yellowSubmarine = gm.towers.First(a => a.name.Contains("MonkeySub-052")).Clone()
                    .Cast<TowerModel>();

                yellowSubmarine.range = 150;
                yellowSubmarine.cost = 0;
                yellowSubmarine.name = "Yellow Submarine";
                yellowSubmarine.baseId = "YellowSubmarine";
                yellowSubmarine.display = "YellowSubmarine";
                yellowSubmarine.dontDisplayUpgrades = true;
                yellowSubmarine.portrait = new("YellowSubmarineIcon");
                yellowSubmarine.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<DisplayModel>())
                    .Cast<DisplayModel>().display = "YellowSubmarine";
                var beh = yellowSubmarine.behaviors;
                ProjectileModel projectile = null;
                for (int i = 0; i < beh.Length; i++) {
                    if (beh[i].GetIl2CppType() == Il2CppType.Of<PreEmptiveStrikeLauncherModel>()) {
                        var am = beh[i].Cast<PreEmptiveStrikeLauncherModel>();
                        am.emissionModel = new ArcEmissionModel("ArcEmissionModel_", 3, 0, 72.5f, null, false, false).Cast<EmissionModel>();
                        am.projectileModel.behaviors = am.projectileModel.behaviors.Add(
                            new CreateProjectileOnContactModel("CreateProjectileOnContactModel_",
                                am.projectileModel.Clone().Cast<ProjectileModel>(),
                                am.emissionModel.Clone().Cast<EmissionModel>(), true, false, true));
                        projectile = am.projectileModel.Clone().Cast<ProjectileModel>();
                        beh[i] = am;
                    }

                    if (beh[i].GetIl2CppType() == Il2CppType.Of<AttackModel>()) {
                        var am = beh[i].Cast<AttackModel>();

                        am.range = 2000;
                        for (int j = 0; j < am.weapons.Length; j++) {
                            am.weapons[j].rate /=5;
                            am.weapons[j].rateFrames /=5;

                            am.weapons[j].projectile.behaviors = am.weapons[j].projectile.behaviors.Add(
                                new CreateProjectileOnContactModel("CreateProjectileOnContactModel_",
                                    am.weapons[j].projectile.Clone().Cast<ProjectileModel>(),
                                    am.weapons[j].emission.Clone().Cast<EmissionModel>(), false, false, false));

                            am.weapons[j].projectile.behaviors = am.weapons[j].projectile.behaviors.Add(
                                new CreateProjectileOnContactModel("CreateProjectileOnContactModel_", projectile,
                                    am.weapons[j].emission.Clone().Cast<EmissionModel>(), false, false, false));

                            for (int k = 0; k < am.weapons[j].projectile.behaviors.Length; k++) {
                                var bh = am.weapons[j].projectile.behaviors[k];

                                if (bh.GetIl2CppType() == Il2CppType.Of<DamageModel>()) {
                                    var dm = bh.Cast<DamageModel>();

                                    dm.damage *= 3 * Globals.SixthTierDamageMulti;

                                    bh = dm;
                                }

                                am.weapons[j].projectile.behaviors[k] = bh;
                            }
                        }

                        beh[i] = am;
                    }

                    if (beh[i].GetIl2CppType() == Il2CppType.Of<AbilityModel>()) {
                        var am = beh[i].Cast<AbilityModel>();
                        am.icon = new("YellowSubmarineIcon");
                        var ab = am.behaviors;
                        for (int j = 0; j < ab.Length; j++) {
                            if (ab[j].GetIl2CppType() == Il2CppType.Of<ActivateAttackModel>()) {
                                var aam = ab[j].Cast<ActivateAttackModel>();

                                for (int k = 0; k < aam.attacks.Length; k++)
                                for (int l = 0; l < aam.attacks[k].weapons.Length; l++) {
                                    aam.attacks[k].weapons[l].projectile.pierce *= 2000;
                                    aam.attacks[k].weapons[l].projectile.radius = 2000;
                                }

                                ab[j] = aam;
                            }
                        }
                        beh[i] = am;
                    }
                }

                yellowSubmarine.behaviors = beh.Add(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            };
            recurring += tts => { };
            onLeave += () => { time = -1; };
            assetsToRead.Add(new ("YellowSubmarine", "62ff4c3f34f9c3c4c9fce1ac3d122ee0", RendererType.SKINNEDMESHRENDERER));
        }
    }
}