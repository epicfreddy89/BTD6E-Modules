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
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using v = Assets.Scripts.Simulation.SMath.Vector3;

namespace AdditionalTiers.Tasks.Towers.Tier6s {
    public class NinjaSexParty : TowerTask {
        public static TowerModel NSP;
        private static int time = -1;

        public NinjaSexParty()
        {
            identifier = "NSP";
            getTower = NSP;
            requirements += tts => tts.tower.towerModel.baseId.Equals("NinjaMonkey") && tts.tower.towerModel.tiers[0] == 5 && tts.damageDealt > ((int)AddedTierEnum.NINJASEXPARTY) * Globals.SixthTierPopCountMulti;
            onComplete += tts => {
                if (time < 50)
                {
                    time++;
                    return;
                }
                tts.tower.namedMonkeyName = identifier;
                tts.tower.worth = 0;
                tts.tower.UpdateRootModel(NSP);
                tts.sim.simulation.CreateTextEffect(new(tts.position), "UpgradedText", 10, "Upgraded!", false);
                /*AbilityMenu.instance.TowerChanged(tts);
                AbilityMenu.instance.RebuildAbilities();*/
            };
            gameLoad += gm =>
            {
                NSP = gm.towers.First(a => a.name.Contains("NinjaMonkey-520")).Clone().Cast<TowerModel>();

                NSP.range = 150;
                NSP.cost = 0;
                NSP.name = "NSP";
                NSP.display = "NinjaSexParty";
                NSP.dontDisplayUpgrades = true;
                NSP.portrait = new("NinjaSexPartyIcon");
                NSP.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<DisplayModel>())
                    .Cast<DisplayModel>().display = "NinjaSexParty";
                var beh = NSP.behaviors;

                for (var i = 0; i < beh.Length; i++) {
                    var behavior = beh[i];
                    if (behavior.GetIl2CppType() == Il2CppType.Of<AttackModel>()) {
                        var am = behavior.Cast<AttackModel>();
                        am.range = 150;
                        am.weapons[0].projectile.display = "NinjaSexPartyProj";
                        am.weapons[0].rate *= 4;
                        am.weapons[0].rateFrames *= 4;
                        am.weapons[0].rate /= 3;
                        am.weapons[0].rateFrames /= 3;

                        for (var k = 0; k < am.weapons[0].projectile.behaviors.Length; k++) {
                            if (am.weapons[0].projectile.behaviors[k].GetIl2CppType() == Il2CppType.Of<TravelStraitModel>()) {
                                var p = am.weapons[0].projectile.behaviors[k].Cast<TravelStraitModel>();
                                p.lifespan *= 100;
                                p.Lifespan *= 100;
                                p.lifespanFrames *= 100;
                                p.speed /= 2;
                                p.Speed /= 2;
                                p.speedFrames /= 2;
                                am.weapons[0].projectile.behaviors[k] = p;
                            }
                            if (am.weapons[0].projectile.behaviors[k].GetIl2CppType() == Il2CppType.Of<DamageModel>()) {
                                var p = am.weapons[0].projectile.behaviors[k].Cast<DamageModel>();
                                p.immuneBloonProperties = BloonProperties.Lead;
                                p.damage *= 5 * Globals.SixthTierDamageMulti;
                                am.weapons[0].projectile.behaviors[k] = p;
                            }
                        }

                        var projectile = gm.towers.FirstOrDefault(a => a.name.Equals("BombShooter-300")).behaviors.First((Model a) => a.GetIl2CppType() == Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>().weapons[0].projectile;
                        var cpocm = projectile.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<CreateProjectileOnContactModel>());
                        var csopcm = projectile.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<CreateSoundOnProjectileCollisionModel>());
                        var ceocm = projectile.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<CreateEffectOnContactModel>());
                        am.weapons[0].projectile.behaviors = am.weapons[0].projectile.behaviors.Add(cpocm, csopcm, ceocm);
                    }
                }

                NSP.behaviors = beh.Add(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            };
            recurring += tts => { };
            onLeave += () => { time = -1; };
            CacheBuilder.toBuild.PushAll("NinjaSexParty", "NinjaSexPartyIcon", "NinjaSexPartyProj");
            assetsToRead.Add(new ("NinjaSexParty", "9cd388b906451874abb35c8608c1d6ed", RendererType.SKINNEDMESHRENDERER));
            assetsToRead.Add(new ("NinjaSexPartyProj", "ae8cebf807b15984daf0219b66f42897", RendererType.SPRITERENDERER));
        }
    }
}