using AdditionalTiers.Utils;
using AdditionalTiers.Utils.Assets;
using AdditionalTiers.Utils.Towers;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Unity.UI_New.InGame.AbilitiesMenu;
using System.Linq;
using UnhollowerRuntimeLib;

namespace AdditionalTiers.Tasks.Towers.Tier6s {
    public class Tusk : TowerTask {
        public static TowerModel gds;
        private static int time = -1;
        public Tusk() {
            identifier = "Tusk";
            getTower = gds;
            requirements += tts => ((tts.tower.towerModel.baseId.Equals("BoomerangMonkey") && tts.tower.towerModel.isParagon) || tts.tower.towerModel.baseId.Equals("ParagonBoomerangMonkey")) && tts.damageDealt > ((int)AddedTierEnum.TUSK) * Globals.SixthTierPopCountMulti;
            onComplete += tts => {
                if (time < 50) {
                    time++;
                    return;
                }
                tts.tower.namedMonkeyName = identifier;
                tts.tower.worth = 0;
                tts.tower.UpdateRootModel(gds);
                tts.sim.simulation.CreateTextEffect(new(tts.position), "UpgradedText", 10, "Upgraded!", false);
                AbilityMenu.instance.TowerChanged(tts);
                AbilityMenu.instance.RebuildAbilities();
            };
            gameLoad += gm => {
                gds = gm.towers.First(a => a.name.Contains(AddedTierName.TUSK)).CloneCast();

                gds.range += 50;
                gds.cost = 0;
                gds.dontDisplayUpgrades = true;
                gds.portrait = new("GlaiveDominusSilverIcon");
                gds.name = identifier;
                gds.display = "GlaiveDominusSilver";
                var beh = gds.behaviors;

                var mortarMonkeyAtt = gm.towers.First(a => a.name.Contains("MortarMonkey-002")).behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<AttackModel>()).CloneCast<AttackModel>();
                var mortarMonkeyCrt = mortarMonkeyAtt.weapons[0].projectile.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<CreateProjectileOnExhaustFractionModel>()).Cast<CreateProjectileOnExhaustFractionModel>();
                OrbitModel cOM = null;
                OrbitModel cCOM = null;
                for (int i = 0; i < beh.Length; i++) {
                    if (beh[i].Is<OrbitModel>(out var orbitModel)) {
                        orbitModel.count = 9;
                        orbitModel.range += 5;
                        orbitModel.projectile.display = "GlaiveDominusSilverOrbit3";
                        orbitModel.projectile.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<DisplayModel>()).Cast<DisplayModel>().display = "GlaiveDominusSilverOrbit3";
                        var dm = new DamageModel("dm", 100, 100, true, true, true, BloonProperties.None);
                        orbitModel.projectile.behaviors = orbitModel.projectile.behaviors.Add(dm);
                        cOM = orbitModel.CloneCast();
                        cOM.count = 5;
                        cOM.range /= 2;
                        cOM.projectile.display = "GlaiveDominusSilverOrbit2";
                        cOM.projectile.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<DisplayModel>()).Cast<DisplayModel>().display = "GlaiveDominusSilverOrbit2";
                        var cdm = new DamageModel("cdm", 250, 250, true, true, true, BloonProperties.None);
                        cOM.projectile.behaviors = cOM.projectile.behaviors.Add(cdm);
                        cCOM = orbitModel.CloneCast();
                        cCOM.count = 15;
                        cCOM.range = orbitModel.range * 1.5f;
                        cCOM.projectile.display = "GlaiveDominusSilverOrbit";
                        cCOM.projectile.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<DisplayModel>()).Cast<DisplayModel>().display = "GlaiveDominusSilverOrbit";
                        cCOM.projectile.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<DamageModel>()).Cast<DamageModel>().damage *= 20;
                        var cCdm = new DamageModel("cCdm", 25, 25, true, true, true, BloonProperties.None);
                        cCOM.projectile.behaviors = cCOM.projectile.behaviors.Add(cCdm);
                    }
                    if (beh[i].Is<AttackModel>(out var attackModel)) {
                        attackModel.weapons[0].projectile.display = "358f76f3f21b8f9439f3cccd23c9b5ff";
                        attackModel.weapons[0].emission = new ArcEmissionModel("arcEmissionModel_", 3, 0, 30, null, false, false);
                        attackModel.weapons[0].projectile.behaviors = attackModel.weapons[0].projectile.behaviors.Add(mortarMonkeyCrt);
                        attackModel.weapons[0].projectile.ignorePierceExhaustion = true;
                        attackModel.range += 50;

                        attackModel.weapons[0].rate = 0;
                        attackModel.weapons[0].rateFrames = 0;

                        var projBeh = attackModel.weapons[0].projectile.behaviors;
                        for (int j = 0; j < projBeh.Length; j++) {
                            if (projBeh[j].Is<DamageModel>(out var damageModel)) {

                                projBeh[j] = damageModel;
                            }
                            if (projBeh[j].Is<RetargetOnContactModel>(out var retargetOnContactModel)) {
                                retargetOnContactModel.maxBounces *= 5;

                                projBeh[j] = retargetOnContactModel;
                            }
                        }
                    }
                }

                AbilityModel abm = gm.towers.FirstOrDefault(pat => pat.name.Contains("Pat") && pat.tier == 20).behaviors.FirstOrDefault(ab => ab.name.Contains("Rally")).Clone().Cast<AbilityModel>();
                abm.icon = new("GlaiveDominusSilverOrbit3");
                abm.name = "GlaiveDominusSilverBuff";
                abm.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<CreateEffectOnAbilityModel>()).Cast<CreateEffectOnAbilityModel>().effectModel =
                new("EM_", "GlaiveDominusSilverAbility", 1.1f, 9999999.0f, false, false, false, false, false, false, false);
                ActivateTowerDamageSupportZoneModel atdszm = abm.behaviors.FirstOrDefault(AA => AA.name.Contains("ActivateTowerDamageSupportZoneModel")).Cast<ActivateTowerDamageSupportZoneModel>();
                atdszm.damageIncrease = 50;
                atdszm.canEffectThisTower = true;
                atdszm.buffIconName = "GlaiveDominusSilverBuff";
                atdszm.buffLocsName = "GlaiveDominusSilverOrbit3";
                atdszm.showBuffIcon = true;
                atdszm.isGlobal = true;

                gm.buffIndicatorModels = gm.buffIndicatorModels.Add(new BuffIndicatorModel("bim_", "GlaiveDominusSilverBuff", "GlaiveDominusSilverOrbit3"));

                gds.behaviors = beh.Remove(a => a.Is<ParagonTowerModel>(out _)).Add(abm).Add(cOM).Add(cCOM).Add(new DisplayModel("dm", "Assets/Monkeys/Adora/Graphics/Effects/AdoraSunBeamPlacement.prefab", 0, new(), 1, true, 0));
            };
            recurring += tts => { };
            onLeave += () => { time = -1; };
            assetsToRead.Add(new("GlaiveDominusSilver", "2ef4918ce220ca04d87a9afc8290a0c4", RendererType.SKINNEDMESHRENDERER));
            assetsToRead.Add(new("GlaiveDominusSilverOrbit", "0a990350818c21e4bb5039a00c8a4977", RendererType.SPRITERENDERER));
            assetsToRead.Add(new("GlaiveDominusSilverOrbit2", "0a990350818c21e4bb5039a00c8a4977", RendererType.SPRITERENDERER));
            assetsToRead.Add(new("GlaiveDominusSilverOrbit3", "0a990350818c21e4bb5039a00c8a4977", RendererType.SPRITERENDERER));
            assetsToRead.Add(new("GlaiveDominusSilverAbility", "a73a10bf47571424080d9e2b33bb2045", RendererType.PARTICLESYSTEMRENDERER));
        }
    }
}