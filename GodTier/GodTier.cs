using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Bloons.Behaviors;
using Assets.Scripts.Models.Effects;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Map;
using Assets.Scripts.Models.Profile;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.Upgrades;
using Assets.Scripts.Models.Towers.Weapons;
using Assets.Scripts.Models.Towers.Weapons.Behaviors;
using Assets.Scripts.Models.TowerSets;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Simulation.Towers.Behaviors.Attack;
using Assets.Scripts.Simulation.Towers.Projectiles;
using Assets.Scripts.Simulation.Towers.Weapons;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Bridge;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Unity.Localization;
using Assets.Scripts.Unity.UI_New.InGame;
using Assets.Scripts.Unity.UI_New.InGame.StoreMenu;
using Assets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu;
using Assets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu.TowerSelectionMenuThemes;
using Assets.Scripts.Unity.UI_New.Upgrade;
using Assets.Scripts.Utils;
using GodTier.Towers;
using GodTier.Utils;
using Harmony;
using MelonLoader;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;
using Object = UnityEngine.Object;

namespace GodTier {
    public class GodTier : MelonMod {
        private static AssetBundle __asset;
        private static List<string> assetNames = new();

        public static AssetBundle assets {
            get => __asset;
            set => __asset = value;
        }

        public override void OnApplicationStart() => assets = AssetBundle.LoadFromMemory(Models.Models.model);

        public static UnityDisplayNode GetGodzilla(Transform transform) {
            var udn = Object.Instantiate(assets.LoadAsset("Godzilla").Cast<GameObject>(), transform)
                .AddComponent<UnityDisplayNode>();
            udn.Active = false;
            udn.transform.position = new(-3000, 0);
            return udn;
        }

        [HarmonyPatch(typeof(UpgradeScreen), "UpdateUi")]
        public class AddShopDetails {
            [HarmonyPrefix]
            public static bool Prefix(ref UpgradeScreen __instance, ref string towerId, ref string upgradeID) {
                if (towerId.Contains(Godzilla.name)) {
                    towerId = "DartMonkey";
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(ProfileModel), "Validate")]
        public class ProfileModel_Patch {
            [HarmonyPostfix]
            public static void Postfix(ref ProfileModel __instance) {
                var unlockedTowers = __instance.unlockedTowers;
                var unlockedUpgrades = __instance.acquiredUpgrades;

                if (!unlockedTowers.Contains(Godzilla.name))
                    unlockedTowers.Add(Godzilla.name);
                if (!unlockedUpgrades.Contains("Atomic Blasts"))
                    unlockedUpgrades.Add("Atomic Blasts");
                if (!unlockedUpgrades.Contains("Thermonuclear Instability"))
                    unlockedUpgrades.Add("Thermonuclear Instability");
            }
        }

        [HarmonyPatch(typeof(StandardTowerPurchaseButton), "SetTower")]
        public static class SetTower {
            public static StandardTowerPurchaseButton stpb = null;
            [HarmonyPostfix]
            internal static void Fix(ref StandardTowerPurchaseButton __instance, ref TowerModel towerModel,
                ref bool showTowerCount, ref bool hero, ref int buttonIndex) {
                stpb = __instance;
                return;
            }
        }

        [HarmonyPatch(typeof(Hotkeys), nameof(Hotkeys.Setup))]
        public static class setupHotkeys {
            [HarmonyPostfix]
            public static void fix(ref Hotkeys __instance) {
                __instance.towerHotkeys.Add(new() {keyCode1 = KeyCode.I, keyCode2 = KeyCode.None, towerBaseId = Godzilla.name, towerPurchaseButton = SetTower.stpb});
            }
        }

        [HarmonyPatch(typeof(GameModelLoader), nameof(GameModelLoader.Load))]
        public static class GameStart {
            [HarmonyPostfix]
            public static void postfix(ref GameModel __result) {
                var godzillaDetails = __result.towerSet[0].Clone().Cast<TowerDetailsModel>();
                godzillaDetails.towerId = Godzilla.name;
                godzillaDetails.towerIndex = 32;

                __result.towers = __result.towers.Add(gett0(__result), gett1(__result), gett2(__result));
                __result.towerSet = __result.towerSet.Add(godzillaDetails);
                __result.upgrades = __result.upgrades.Add(getupgrades());

                LocalizationManager.instance.textTable.Add("Atomic Blasts Description", "The radiation within Godzilla flares greatly, only to be ejected as a breath-like attack.");
                LocalizationManager.instance.textTable.Add("Thermonuclear Instability Description", "With the power of Mothra's sacrifice, Godzilla has gained enough radiation to release Thermonuclear blasts, destroying anything in his way, and empowering all attacks.");
            }

            public static UpgradeModel[] getupgrades() {
                return new UpgradeModel[]
                {
                    new("Atomic Blasts", 7840, 0, new("GodzillaPortrait"), 0, 1, 0, "", "Atomic Blasts"),
                    new("Thermonuclear Instability", 19850, 0, new("GodzillaPortrait2"), 0, 2, 0, "", "Thermonuclear Instability")
                };
            }

            public static TowerModel gett0(GameModel __result) {
                var godzilla = __result.towers[0].Clone().Cast<TowerModel>();

                godzilla.name = Godzilla.name;
                godzilla.baseId = Godzilla.name;
                godzilla.display = "Godzilla0";
                godzilla.portrait = new("GodzillaPortrait");
                godzilla.icon = new("GodzillaPortrait");
                godzilla.towerSet = "Magic";
                godzilla.radius = 15;
                godzilla.cost = 1750;
                godzilla.range = 50;
                godzilla.towerSize = TowerModel.TowerSize.XL;
                godzilla.footprint.ignoresPlacementCheck = true;
                godzilla.cachedThrowMarkerHeight = 10;
                godzilla.areaTypes = new(4);
                godzilla.areaTypes[0] = AreaType.ice;
                godzilla.areaTypes[1] = AreaType.land;
                godzilla.areaTypes[2] = AreaType.track;
                godzilla.areaTypes[3] = AreaType.water;
                godzilla.upgrades = new UpgradePathModel[] {new("Atomic Blasts", Godzilla.name + "-100", 0, 1)};
                for (var i = 0; i < godzilla.behaviors.Count; i++) {
                    var b = godzilla.behaviors[i];
                    if (b.GetIl2CppType() == Il2CppType.Of<AttackModel>()) {
                        var att = __result.towers.First(a => a.name.Contains("Sauda 20"))
                            .behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<AttackModel>()).Clone()
                            .Cast<AttackModel>();
                        att.weapons[0].name = "Swipe";
                        att.weapons[0].rate = 0.95f * 4;
                        att.weapons[0].rateFrames = 56 * 4;
                        att.range = 50;
                        att.weapons[0].projectile.behaviors = att.weapons[0].projectile.behaviors
                            .Add(new KnockbackModel("KnockbackModel_", 0.2f, 0.15f, 0.3f, 29, "Knockback",
                                new("KnockbackKnockback", 0.3f, 0.2f, 0.15f)).Cast<Model>());
                        for (var j = 0; j < att.weapons[0].projectile.behaviors.Length; j++) {
                            var pb = att.weapons[0].projectile.behaviors[j];
                            if (pb.GetIl2CppType() == Il2CppType.Of<DamageModel>()) {
                                var d = pb.Cast<DamageModel>();

                                d.damage = 25;
                                d.maxDamage = 50;

                                pb = d;
                            }
                        }
                        godzilla.behaviors[i] = att;
                    }

                    if (b.GetIl2CppType() == Il2CppType.Of<DisplayModel>()) {
                        var display = b.Cast<DisplayModel>();
                        display.display = "Godzilla0";
                        b = display;
                    }
                }

                var link = __result.towers.First(a => a.name.Contains("Sauda 20"))
                    .behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<LinkProjectileRadiusToTowerRangeModel>())
                    .Clone().Cast<LinkProjectileRadiusToTowerRangeModel>();
                link.projectileModel.behaviors =
                    link.projectileModel.behaviors.Remove(a =>
                        a.name.Equals("AddBehaviorToBloonModel_MoabDeathSound"));
                link.baseTowerRange = 50;
                godzilla.behaviors = godzilla.behaviors.Add(link);
                return godzilla;
            }

            public static TowerModel gett1(GameModel __result) {
                var godzilla = __result.towers[0].Clone().Cast<TowerModel>();

                godzilla.name = Godzilla.name + "-100";
                godzilla.baseId = Godzilla.name;
                godzilla.tier = 1;
                godzilla.tiers = new int[] {1, 0, 0};
                godzilla.display = "Godzilla1";
                godzilla.portrait = new("GodzillaPortrait");
                godzilla.icon = new("GodzillaPortrait");
                godzilla.towerSet = "Magic";
                godzilla.radius = 15;
                godzilla.range = 127;
                godzilla.towerSize = TowerModel.TowerSize.XL;
                godzilla.footprint.ignoresPlacementCheck = true;
                godzilla.cachedThrowMarkerHeight = 10;
                godzilla.areaTypes = new(4);
                godzilla.areaTypes[0] = AreaType.ice;
                godzilla.areaTypes[1] = AreaType.land;
                godzilla.areaTypes[2] = AreaType.track;
                godzilla.areaTypes[3] = AreaType.water;
                godzilla.appliedUpgrades = new(new[] {"Atomic Blasts"});
                godzilla.upgrades = new[] {new UpgradePathModel("Thermonuclear Instability", Godzilla.name + "-200", 1, 2)};
                for (var i = 0; i < godzilla.behaviors.Count; i++) {
                    var b = godzilla.behaviors[i];
                    if (b.GetIl2CppType() == Il2CppType.Of<AttackModel>()) {
                        var att = __result.towers.First(a => a.name.Contains("SuperMonkey-200"))
                            .behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<AttackModel>()).Clone()
                            .Cast<AttackModel>();
                        att.weapons[0].rate = 0.95f * 5;
                        att.weapons[0].rateFrames = 56 * 5;
                        att.range = 127;
                        att.weapons[0].projectile.display = "GodzillaBreath";
                        att.weapons[0].projectile.behaviors = att.weapons[0].projectile.behaviors
                            .Add(new KnockbackModel("KnockbackModel_", 0.7f, 0.85f, 0.5f, 29, "Knockback",
                                new("KnockbackKnockback", 0.5f, 0.7f, 0.85f)).Cast<Model>());
                        for (var j = 0; j < att.weapons[0].projectile.behaviors.Length; j++) {
                            var pb = att.weapons[0].projectile.behaviors[j];
                            if (pb.GetIl2CppType() == Il2CppType.Of<DamageModel>()) {
                                var d = pb.Cast<DamageModel>();

                                d.damage = 30;
                                d.maxDamage = 50;

                                pb = d;
                            }
                        }
                        att.weapons[0].name = "Breath";
                        var projectile = __result.towers.FirstOrDefault(a => a.name.Equals("BombShooter-300")).behaviors.First((Model a) => a.GetIl2CppType() == Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>().weapons[0].projectile;
                        var cpocm = projectile.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<CreateProjectileOnContactModel>()).Clone();
                        var csopcm = projectile.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<CreateSoundOnProjectileCollisionModel>()).Clone();
                        var ceocm = projectile.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<CreateEffectOnContactModel>()).Clone();
                        att.weapons[0].projectile.behaviors = att.weapons[0].projectile.behaviors.Add(cpocm, csopcm, ceocm);
                        godzilla.behaviors[i] = att;
                    }

                    if (b.GetIl2CppType() == Il2CppType.Of<DisplayModel>()) {
                        var display = b.Cast<DisplayModel>();
                        display.display = "Godzilla1";
                        b = display;
                    }
                }
                var aa = gett0(__result).behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>();
                var link = __result.towers.First(a => a.name.Contains("Sauda 20"))
                    .behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<LinkProjectileRadiusToTowerRangeModel>())
                    .Clone().Cast<LinkProjectileRadiusToTowerRangeModel>();
                link.projectileModel.radius = 50;
                link.projectileModel.pierce = 150;
                link.projectileModel.maxPierce = 1500;
                link.projectileModel.behaviors =
                    link.projectileModel.behaviors.Remove(a =>
                        a.name.Equals("AddBehaviorToBloonModel_MoabDeathSound"));
                for (var i = 0; i < link.projectileModel.behaviors.Length; i++)
                    if (link.projectileModel.behaviors[i].GetIl2CppType() == Il2CppType.Of<DamageModel>()) {
                        var dm = link.projectileModel.behaviors[i].Cast<DamageModel>();
                        dm.damage = 150;
                        dm.maxDamage = 200;
                    }

                aa.Cast<AttackModel>().weapons[0].projectile = link.projectileModel;
                godzilla.behaviors = godzilla.behaviors.Add(aa, new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));

                return godzilla;
            }

            public static TowerModel gett2(GameModel __result) {
                var godzilla = __result.towers[0].Clone().Cast<TowerModel>();
                var aa = gett0(__result).behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>();

                godzilla.name = Godzilla.name + "-200";
                godzilla.baseId = Godzilla.name;
                godzilla.tier = 2;
                godzilla.tiers = new int[] {2, 0, 0};
                godzilla.display = "Godzilla2";
                godzilla.portrait = new("GodzillaPortrait2");
                godzilla.icon = new("GodzillaPortrait2");
                godzilla.towerSet = "Magic";
                godzilla.radius = 15;
                godzilla.range = 150;
                godzilla.towerSize = TowerModel.TowerSize.XL;
                godzilla.footprint.ignoresPlacementCheck = true;
                godzilla.cachedThrowMarkerHeight = 10;
                godzilla.areaTypes = new(4);
                godzilla.areaTypes[0] = AreaType.ice;
                godzilla.areaTypes[1] = AreaType.land;
                godzilla.areaTypes[2] = AreaType.track;
                godzilla.areaTypes[3] = AreaType.water;
                godzilla.appliedUpgrades = new(new[] {"Atomic Blasts", "Thermonuclear Instability"});
                godzilla.upgrades = new(0);
                for (var i = 0; i < godzilla.behaviors.Count; i++) {
                    var b = godzilla.behaviors[i];
                    if (b.GetIl2CppType() == Il2CppType.Of<AttackModel>()) {
                        var att = __result.towers.First(a => a.name.Contains("SuperMonkey-200"))
                            .behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<AttackModel>()).Clone()
                            .Cast<AttackModel>();
                        att.weapons[0].rate = 0.95f * 3;
                        att.weapons[0].rateFrames = 56 * 3;
                        att.range = 150;
                        att.weapons[0].projectile.display = "GodzillaBreath2";
                        for (var j = 0; j < att.weapons[0].projectile.behaviors.Length; j++) {
                            var pb = att.weapons[0].projectile.behaviors[j];
                            if (pb.GetIl2CppType() == Il2CppType.Of<DamageModel>()) {
                                var d = pb.Cast<DamageModel>();
                                d.damage = 350;
                                d.maxDamage = 500;
                                pb = d;
                            }
                        }
                        att.weapons[0].name = "Breath";
                        var projectile = __result.towers.FirstOrDefault(a => a.name.Equals("BombShooter-300")).behaviors.First((Model a) => a.GetIl2CppType() == Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>().weapons[0].projectile;
                        var cpocm = projectile.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<CreateProjectileOnContactModel>()).Clone().Cast<CreateProjectileOnContactModel>();

                        cpocm.projectile.pierce = 50;

                        for (var j = 0; j < cpocm.projectile.behaviors.Length; j++)
                            if (cpocm.projectile.behaviors[j].GetIl2CppType() == Il2CppType.Of<DamageModel>()) {
                                var dm = cpocm.projectile.behaviors[j].Cast<DamageModel>();

                                dm.damage = 375;
                                dm.maxDamage = 500;

                                cpocm.projectile.behaviors[j] = dm;
                            }

                        var csopcm = projectile.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<CreateSoundOnProjectileCollisionModel>()).Clone();
                        var ceocm = projectile.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<CreateEffectOnContactModel>()).Clone();
                        var abtbm = __result.towers.First(a => a.name.Equals("MortarMonkey-002")).behaviors
                            .First(a => a.GetIl2CppType() == Il2CppType.Of<AttackModel>()).Clone().Cast<AttackModel>()
                            .weapons[0].projectile.behaviors.First(a =>
                                a.GetIl2CppType() == Il2CppType.Of<CreateProjectileOnExhaustFractionModel>()).
                            Cast<CreateProjectileOnExhaustFractionModel>().projectile.behaviors.
                            First(a=>a.GetIl2CppType()==Il2CppType.Of<AddBehaviorToBloonModel>()).Cast<AddBehaviorToBloonModel>();
                        var dotm = abtbm.behaviors[0].Clone().Cast<DamageOverTimeModel>();
                        dotm.damage = 10;
                        abtbm.behaviors[0] = dotm;
                        cpocm.projectile.behaviors = cpocm.projectile.behaviors.Add(abtbm);
                        att.weapons[0].projectile.behaviors = att.weapons[0].projectile.behaviors.Add(cpocm, csopcm, ceocm,
                            new KnockbackModel("KnockbackModel_", 0.7f, 0.85f, 0.5f, 29, "Knockback",
                            new("KnockbackKnockback", 0.5f, 0.7f, 0.85f)).Cast<Model>(),
                        new TrackTargetWithinTimeModel("TrackTargetWithinTimeModel_",
                            9999999, true, false, 365, false, 9999999, false, 3.48f, true).Cast<Model>());
                        godzilla.behaviors[i] = att;
                    }

                    if (b.GetIl2CppType() == Il2CppType.Of<DisplayModel>()) {
                        var display = b.Cast<DisplayModel>();
                        display.display = "Godzilla2";
                        b = display;
                    }
                }
                var link = __result.towers.First(a => a.name.Contains("Sauda 20"))
                    .behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<LinkProjectileRadiusToTowerRangeModel>())
                    .Clone().Cast<LinkProjectileRadiusToTowerRangeModel>();
                link.projectileModel.radius = 50;
                link.projectileModel.pierce = 150;
                link.projectileModel.maxPierce = 1500;
                link.projectileModel.behaviors =
                    link.projectileModel.behaviors.Remove(a =>
                        a.name.Equals("AddBehaviorToBloonModel_MoabDeathSound"));
                for (var i = 0; i < link.projectileModel.behaviors.Length; i++)
                    if (link.projectileModel.behaviors[i].GetIl2CppType() == Il2CppType.Of<DamageModel>()) {
                        var dm = link.projectileModel.behaviors[i].Cast<DamageModel>();
                        dm.damage = 150;
                        dm.maxDamage = 200;
                    }

                aa.Cast<AttackModel>().weapons[0].projectile = link.projectileModel;
                var ab = __result.towers.First(a => a.name.Equals("SuperMonkey-250")).behaviors
                    .First(a => a.GetIl2CppType() == Il2CppType.Of<AbilityModel>()).Clone().Cast<AbilityModel>();
                ab.name = "Thermonuclear Exhaust";
                ab.displayName = "Thermonuclear Exhaust";
                ab.icon = new ("ThermoNuclearCharge");
                var aam = ab.behaviors[0].Cast<ActivateAttackModel>();
                var dam = aam.attacks[0].weapons[0].projectile.behaviors.First(a=>a.GetIl2CppType()==Il2CppType.Of<DamageModel>()).Cast<DamageModel>();
                dam.damage *= 50;
                godzilla.behaviors = godzilla.behaviors.Add(aa, new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true), ab);

                return godzilla;
            }
        }

        [HarmonyPatch(typeof(Factory), nameof(Factory.ProtoFlush))]
        public class PrototypeFlushUDN_Patch {
            [HarmonyPostfix]
            public static void Postfix() {
                foreach (var proto in PrototypeUDN_Patch.protos.Values)
                    Object.Destroy(proto.gameObject);
                PrototypeUDN_Patch.protos.Clear();
            }
        }

        [HarmonyPatch(typeof(Tower), nameof(Tower.Hilight))]
        public static class TH {
            [HarmonyPostfix]
            public static void postfix(ref Tower __instance) {
                if (__instance?.Node?.graphic?.genericRenderers == null)
                    return;

                foreach (var graphicGenericRenderer in __instance.Node.graphic.genericRenderers)
                    graphicGenericRenderer.material.SetColor("_OutlineColor", Color.white);
            }
        }

        [HarmonyPatch(typeof(Tower), nameof(Tower.UnHighlight))]
        public static class TU {
            [HarmonyPostfix]
            public static void postfix(ref Tower __instance) {
                if (__instance?.Node?.graphic?.genericRenderers == null)
                    return;

                foreach (var graphicGenericRenderer in __instance.Node.graphic.genericRenderers)
                    graphicGenericRenderer.material.SetColor("_OutlineColor", Color.black);
            }
        }

        [HarmonyPatch(typeof(Weapon), nameof(Weapon.SpawnDart))]
        public static class WI {
#pragma warning disable CS4014
            [HarmonyPostfix]
            public static void postfix(ref Weapon __instance) => run(__instance);

            private static async Task run(Weapon __instance) {
                if (__instance.weaponModel.name.EndsWith("Swipe")) {
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().StopPlayback();
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>()
                        .Play("metarig|metarigSwipe"); //metarig|metarigFireball metarig|metarigSwipe
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().SetBool("Attack", true);
                    var wait = 2300f;
                    await Task.Run(() => {
                        while (wait > 0) {
                            wait -= TimeManager.timeScaleWithoutNetwork + 1;
                            Task.Delay(1);
                        }

                        return;
                    });
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().SetBool("Attack", false);
                }
                if (__instance.weaponModel.name.EndsWith("Breath")) {
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().StopPlayback();
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>()
                        .Play("metarig|metarigFireball"); // metarig|metarigSwipe
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().SetBool("Attack", true);
                    var wait = 2300f;
                    await Task.Run(() => {
                        while (wait > 0) {
                            wait -= TimeManager.timeScaleWithoutNetwork + 1;
                            Task.Delay(1);
                        }

                        return;
                    });
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().SetBool("Attack", false);
                }
            }
        }

        [HarmonyPatch(typeof(Factory), nameof(Factory.FindAndSetupPrototypeAsync))]
        public class PrototypeUDN_Patch {
            public static Dictionary<string, UnityDisplayNode> protos = new();
            public static Shader nkOutline = null;

            [HarmonyPrefix]
            public static bool Prefix(Factory __instance, string objectId,
                Il2CppSystem.Action<UnityDisplayNode> onComplete) {
                if (!protos.ContainsKey(objectId) && objectId.Equals("Godzilla0")) {
                    var udn = GetGodzilla(__instance.PrototypeRoot);
                    udn.name = "Godzilla";
                    var a = assets.LoadAsset("GodzillaMaterial");
                    udn.genericRenderers[0].material = a.Cast<Material>();
                    udn.RecalculateGenericRenderers();
                    udn.isSprite = false;
                    onComplete.Invoke(udn);
                    protos.Add(objectId, udn);
                    return false;
                }

                if (!protos.ContainsKey(objectId) && objectId.Equals("Godzilla1")) {
                    var udn = GetGodzilla(__instance.PrototypeRoot);
                    udn.name = "Godzilla1";
                    udn.RecalculateGenericRenderers();
                    udn.isSprite = false;
                    var a = assets.LoadAsset("GodzillaMaterialBlu");
                    udn.genericRenderers[0].material = a.Cast<Material>();
                    onComplete.Invoke(udn);
                    protos.Add(objectId, udn);
                    return false;
                }

                if (!protos.ContainsKey(objectId) && objectId.Equals("Godzilla2")) {
                    var udn = GetGodzilla(__instance.PrototypeRoot);
                    udn.name = "Godzilla2";
                    udn.RecalculateGenericRenderers();
                    udn.isSprite = false;
                    var a = assets.LoadAsset("GodzillaMaterialRed");
                    udn.genericRenderers[0].material = a.Cast<Material>();
                    onComplete.Invoke(udn);
                    protos.Add(objectId, udn);
                    return false;
                }

                if (objectId.Equals("GodzillaBreath")) {
                    UnityDisplayNode udn = null;
                    __instance.FindAndSetupPrototypeAsync("bdbeaa256e6c63b45829535831843376",
                        new System.Action<UnityDisplayNode>(oudn => {
                            var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                            nudn.name = objectId + "(Clone)";
                            nudn.isSprite = true;
                            nudn.RecalculateGenericRenderers();
                            for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                if (nudn.genericRenderers[i].GetIl2CppType() == Il2CppType.Of<SpriteRenderer>()) {
                                    var smr = nudn.genericRenderers[i].Cast<SpriteRenderer>();
                                    var text = assets.LoadAsset("BluePlasma").Cast<Texture2D>();
                                    smr.sprite = Sprite.Create(text, new(0,0,text.width,text.height), new(0.5f,0.5f), 5.4f);
                                    nudn.genericRenderers[i] = smr;
                                }
                            }

                            udn = nudn;
                            onComplete.Invoke(udn);
                        }));
                    return false;
                }

                if (objectId.Equals("GodzillaBreath2")) {
                    UnityDisplayNode udn = null;
                    __instance.FindAndSetupPrototypeAsync("bdbeaa256e6c63b45829535831843376",
                        new System.Action<UnityDisplayNode>(oudn => {
                            var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                            nudn.name = objectId + "(Clone)";
                            nudn.isSprite = true;
                            nudn.RecalculateGenericRenderers();
                            for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                if (nudn.genericRenderers[i].GetIl2CppType() == Il2CppType.Of<SpriteRenderer>()) {
                                    var smr = nudn.genericRenderers[i].Cast<SpriteRenderer>();
                                    var text = assets.LoadAsset("RedPlasma").Cast<Texture2D>();
                                    smr.sprite = Sprite.Create(text, new(0,0,text.width,text.height), new(0.5f,0.5f), 2.7f);
                                    nudn.genericRenderers[i] = smr;
                                }
                            }

                            udn = nudn;
                            onComplete.Invoke(udn);
                        }));
                    return false;
                }

                if (protos.ContainsKey(objectId)) {
                    onComplete.Invoke(protos[objectId]);
                    return false;
                }

                return true;
            }
        }

#pragma warning disable CS0168
        [HarmonyPatch(typeof(ResourceLoader), nameof(ResourceLoader.LoadSpriteFromSpriteReferenceAsync))]
        public record ResourceLoader_Patch {
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference, Image image) {
                if (reference != null && reference.guidRef.Equals("GodzillaPortrait"))
                    try {
                        var b = assets.LoadAsset("GodzillaPortrait");
                        if (b != null) {
                            var text = b.Cast<Texture2D>();
                            image.canvasRenderer.SetTexture(text);
                            image.sprite = Sprite.Create(text, new(0, 0, text.width, text.height), new());
                        }
                    } catch (Exception e) {
                        //ignore e
                    }
                if (reference != null && reference.guidRef.Equals("GodzillaPortrait2"))
                    try {
                        var b = assets.LoadAsset("GodzillaPortrait2");
                        if (b != null) {
                            var text = b.Cast<Texture2D>();
                            image.canvasRenderer.SetTexture(text);
                            image.sprite = Sprite.Create(text, new(0, 0, text.width, text.height), new());
                        }
                    } catch (Exception e) {
                        //ignore e
                    }
                if (reference != null && reference.guidRef.Equals("ThermoNuclearCharge"))
                    try {
                        var b = assets.LoadAsset("ThermoNuclearCharge");
                        if (b != null) {
                            var text = b.Cast<Texture2D>();
                            image.canvasRenderer.SetTexture(text);
                            image.sprite = Sprite.Create(text, new(0, 0, text.width, text.height), new());
                        }
                    } catch (Exception e) {
                        //ignore e
                    }
            }
        }
    }
}