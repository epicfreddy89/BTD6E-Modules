global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;

global using Assets.Scripts.Models;
global using Assets.Scripts.Models.Bloons.Behaviors;
global using Assets.Scripts.Models.Effects;
global using Assets.Scripts.Models.GenericBehaviors;
global using Assets.Scripts.Models.Map;
global using Assets.Scripts.Models.Profile;
global using Assets.Scripts.Models.Towers;
global using Assets.Scripts.Models.Towers.Behaviors;
global using Assets.Scripts.Models.Towers.Behaviors.Abilities;
global using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
global using Assets.Scripts.Models.Towers.Behaviors.Attack;
global using Assets.Scripts.Models.Towers.Behaviors.Emissions;
global using Assets.Scripts.Models.Towers.Filters;
global using Assets.Scripts.Models.Towers.Mods;
global using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
global using Assets.Scripts.Models.Towers.Upgrades;
global using Assets.Scripts.Models.Towers.Weapons;
global using Assets.Scripts.Models.TowerSets;
global using Assets.Scripts.Simulation.Towers;
global using Assets.Scripts.Simulation.Towers.Weapons;
global using Assets.Scripts.Unity.Audio;
global using Assets.Scripts.Unity.Bridge;
global using Assets.Scripts.Unity.Display;
global using Assets.Scripts.Unity.Player;
global using Assets.Scripts.Unity.UI_New.InGame.StoreMenu;
global using Assets.Scripts.Unity.UI_New.InGame.TowerSelectionMenu;
global using Assets.Scripts.Unity.UI_New.Main.MapSelect;
global using Assets.Scripts.Unity.UI_New.Upgrade;
global using Assets.Scripts.Utils;

global using GodlyTowers.Models;
global using GodlyTowers.Towers;
global using GodlyTowers.Util;

global using GodTier.Towers;
global using GodTier.Utils;

global using HarmonyLib;

global using MelonLoader;

global using NinjaKiwi.Common;

global using UnhollowerRuntimeLib;

global using UnityEngine;

global using Color = UnityEngine.Color;
global using Object = UnityEngine.Object;
global using Image = UnityEngine.UI.Image;

using System.Threading;

using Assets.Scripts.Models.ServerEvents;
using Assets.Scripts.Simulation.Input;

[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
[assembly: MelonInfo(typeof(GodTier.GodTier), "God Tiers", "1.6", "1330 Studios LLC")]

namespace GodTier {
    public class GodTier : MelonMod {

        public override void OnApplicationStart() {
            Godzilla.Assets = AssetBundle.LoadFromMemory(Models.godzilla);
            Spider_Man.Assets = AssetBundle.LoadFromMemory(Models.spiderman);
            Carnage.Assets = AssetBundle.LoadFromMemory(Models.carnage);
            Venom.Assets = AssetBundle.LoadFromMemory(Models.venom);
            MiniPekka.Assets = AssetBundle.LoadFromMemory(Models.minipekka);
        }

        [HarmonyPatch(typeof(Btd6Player), "CheckForNewParagonPipEvent")]
        public class Btd6PlayerIsBad {
            [HarmonyPrefix]
            public static bool Prefix(string checkSpecificTowerId, string checkSpecificTowerSet, ref bool __result) => __result = false;
        }

        [HarmonyPatch(typeof(UpgradeScreen), "UpdateUi")]
        public class AddShopDetails {
            [HarmonyPrefix]
            public static bool Prefix(ref UpgradeScreen __instance, ref string towerId) {
                foreach (var tower in towers)
                    if (towerId.Contains(tower.Item1.baseId)) {
                        towerId = "DartMonkey";
                    }
                foreach (var paragon in paragons)
                    if (towerId.Contains(paragon.Item3)) {
                        towerId = paragon.Item3;
                        __instance.currTowerId = paragon.Item3;
                        __instance.hasTower = true;
                    }

                return true;
            }
        }

        [HarmonyPatch(typeof(MonkeyTeamsIcon), nameof(MonkeyTeamsIcon.Init))]
        public class MTIcon {
            [HarmonyPrefix]
            public static bool Prefix(ref MonkeyTeamsIcon __instance) {
                __instance.enabled = false;
                __instance.gameObject.SetActive(false);
                return false;
            }
        }

        [HarmonyPatch(typeof(StandardTowerPurchaseButton), nameof(StandardTowerPurchaseButton.UpdateTowerDisplay))]
        public class SetBG {
            [HarmonyPostfix]
            public static void Postfix(ref StandardTowerPurchaseButton __instance) {
                __instance.bg = __instance.gameObject.GetComponent<Image>();

                if (__instance.baseTowerModel.emoteSpriteLarge != null)
                    switch (__instance.baseTowerModel.emoteSpriteLarge.guidRef) {
                        case "Dark":
                            __instance.bg.overrideSprite = LoadSprite(LoadTextureFromBytes(GodlyTowers.Properties.Resources.TowerContainerDark));
                            break;
                        case "Movie":
                            __instance.bg.overrideSprite = LoadSprite(LoadTextureFromBytes(GodlyTowers.Properties.Resources.TowerContainerMovie));
                            break;
                        case "Paragon":
                            __instance.bg.overrideSprite = LoadSprite(LoadTextureFromBytes(GodlyTowers.Properties.Resources.TowerContainerParagonLarge));
                            break;
                        case "None":
                            __instance.bg.overrideSprite = LoadSprite(LoadTextureFromBytes(GodlyTowers.Properties.Resources.none));
                            break;
                    }

                return;
            }
        }

        public static Texture2D LoadTextureFromBytes(byte[] FileData) {
            Texture2D Tex2D = new(64, 64);
            if (ImageConversion.LoadImage(Tex2D, FileData)) return Tex2D;

            return null;
        }

        public static Sprite LoadSprite(Texture2D text) {
            return Sprite.Create(text, new(0, 0, text.width, text.height), new());
        }

        internal static List<(TowerModel, ShopTowerDetailsModel, string, TowerModel)> paragons = new();
        internal static List<(TowerModel, ShopTowerDetailsModel, TowerModel[], UpgradeModel[])> towers = new();
        internal static Dictionary<string, UpgradeBG> CustomUpgrades = new();

        public enum UpgradeBG {
            AntiVenom,
            MiniPekka
        }

        [HarmonyPatch(typeof(ProfileModel), "Validate")]
        public class ProfileModel_Patch {
            [HarmonyPostfix]
            public static void Postfix(ref ProfileModel __instance) {
                var unlockedTowers = __instance.unlockedTowers;
                var unlockedUpgrades = __instance.acquiredUpgrades;

                foreach (var paragon in paragons)
                    if (paragon.Item1 != null)
                        if (!unlockedTowers.Contains(paragon.Item1.baseId))
                            unlockedTowers.Add(paragon.Item1.baseId);

                foreach (var tower in towers) {
                    if (tower.Item1 != null)
                        if (!unlockedTowers.Contains(tower.Item1.baseId))
                            unlockedTowers.Add(tower.Item1.baseId);

                    foreach (var upgrade in tower.Item4)
                        if (upgrade != null)
                            if (!unlockedUpgrades.Contains(upgrade.name))
                                unlockedUpgrades.Add(upgrade.name);
                }

            }
        }

        [HarmonyPatch(typeof(GameModelLoader), nameof(GameModelLoader.Load))]
        public static class GameStart {
            [HarmonyPostfix]
            public static void Postfix(ref GameModel __result) {
                paragons.Add(Paragons.GetDartMonkey(__result));
                paragons.Add(Paragons.GetBoomerangMonkey(__result));
                paragons.Add(Paragons.GetNinjaMonkey(__result));
                paragons.Add(Paragons.GetMonkeyBuccaneer(__result));
                towers.Add(Godzilla.GetTower(__result));
                towers.Add(Spider_Man.GetTower(__result));
                towers.Add(Carnage.GetTower(__result));
                towers.Add(Venom.GetTower(__result));
                towers.Add(MiniPekka.GetTower(__result));
                towers.Add(Grim_Reaper.GetTower(__result));

                foreach (var paragon in paragons) {
                    __result.towers = __result.towers.Add(paragon.Item1);
                    __result.towerSet = __result.towerSet.Add(paragon.Item2);
                }

                foreach (var tower in towers) {
                    __result.towers = __result.towers.Add(tower.Item3);
                    __result.towerSet = __result.towerSet.Add(tower.Item2);
                    __result.upgrades = __result.upgrades.Add(tower.Item4);
                    foreach (var upgrade in tower.Item4) {
                        if (upgrade != null)
                            __result.upgradesByName.Add(upgrade.name, upgrade);
                    }
                }
            }

            [HarmonyPatch(typeof(UpgradeButton), nameof(UpgradeButton.SetUpgradeModel))]
            internal class TowerManager_UpgradeTower {
                [HarmonyPostfix]
                internal static void fix(ref UpgradeButton __instance) {
                    if (__instance == null) return;
                    if (__instance.upgradeStatus != UpgradeButton.UpgradeStatus.Purchasable) {
                        __instance.purchaseArrowGlow.active = false;
                        __instance.backgroundActive = new("Ui[GreenArrowBtn]");
                        __instance.background.overrideSprite = null;
                    }
                    if (__instance.upgrade == null) return;
                    if (__instance.upgrade.name == null) return;
                    __instance.purchaseArrowGlow.active = CustomUpgrades.ContainsKey(__instance.upgrade.name);
                    if (CustomUpgrades.ContainsKey(__instance.upgrade.name)) {
                        string resourceName = "";
                        Sprite resourceSprite = null;
                        switch (CustomUpgrades[__instance.upgrade.name]) {
                            case UpgradeBG.AntiVenom: {
                                    resourceName = "AntiVenomUBG";
                                    resourceSprite = LoadSprite(LoadTextureFromBytes(GodlyTowers.Properties.Resources.AVenomUBG));
                                    break;
                                }
                            case UpgradeBG.MiniPekka: {
                                    resourceName = "MiniPekkaUBG";
                                    resourceSprite = LoadSprite(LoadTextureFromBytes(GodlyTowers.Properties.Resources.MPUBG));
                                    break;
                                }
                        }
                        __instance.backgroundActive = new(resourceName);
                        __instance.background.overrideSprite = resourceSprite;
                    } else {
                        __instance.backgroundActive = new("Ui[GreenArrowBtn]");
                        __instance.background.overrideSprite = null;
                    }

                    return;
                }
            }

            // Cancer I know, but at least it's an idea

            [HarmonyPatch(typeof(Tower), nameof(Tower.Hilight))]
            public static class TH {
                [HarmonyPostfix]
                public static void Postfix(ref Tower __instance) {
                    if (__instance?.Node?.graphic?.genericRenderers == null)
                        return;

                    foreach (var graphicGenericRenderer in __instance.Node.graphic.genericRenderers)
                        foreach (var item in graphicGenericRenderer.materials)
                            item.SetColor("_OutlineColor", Color.white);
                }
            }

            [HarmonyPatch(typeof(Tower), nameof(Tower.UnHighlight))]
            public static class TU {
                [HarmonyPostfix]
                public static void Postfix(ref Tower __instance) {
                    if (__instance?.Node?.graphic?.genericRenderers == null)
                        return;

                    foreach (var graphicGenericRenderer in __instance.Node.graphic.genericRenderers)
                        foreach (var item in graphicGenericRenderer.materials)
                            item.SetColor("_OutlineColor", Color.black);
                }
            }
        }
    }
}