using Assets.Scripts.Models;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.TowerSets;
using GodTier.Towers;
using NinjaKiwi.Common;
using System.Linq;

namespace GodlyTowers.Towers {
    public class Paragons {
        private static int index = 32;

        public static (TowerModel, TowerDetailsModel, string, TowerModel) GetDartMonkey(GameModel model) {
            var origParagon = model.towers.First(t => t.name.Equals("DartMonkey-Paragon"));
            var paragon = origParagon.Clone().Cast<TowerModel>();
            paragon.baseId = "ParagonDartMonkey";
            paragon.name = "ParagonDartMonkey";
            paragon.towerSet = "Primary";
            paragon.emoteSpriteLarge = new("Paragon");
            paragon.tier = 0;
            paragon.tiers = new int[] { 0, 0, 0 };
            paragon.cost = 400000;
            paragon.canAlwaysBeSold = true;
            paragon.isParagon = false;
            paragon.dontDisplayUpgrades = true;
            paragon.icon = paragon.portrait;

            if (!LocalizationManager.Instance.textTable.ContainsKey("ParagonDartMonkey"))
                LocalizationManager.Instance.textTable.Add("ParagonDartMonkey", "Apex Plasma Master");

            var paragonDetails = model.towerSet[0].Clone().Cast<TowerDetailsModel>();
            paragonDetails.towerId = "ParagonDartMonkey";
            paragonDetails.towerIndex = ++index;

            return (paragon, paragonDetails, "DartMonkey", origParagon);
        }

        public static (TowerModel, TowerDetailsModel, string, TowerModel) GetBoomerangMonkey(GameModel model) {
            var origParagon = model.towers.First(t => t.name.Equals("BoomerangMonkey-Paragon"));
            var paragon = origParagon.Clone().Cast<TowerModel>();
            paragon.baseId = "ParagonBoomerangMonkey";
            paragon.name = "ParagonBoomerangMonkey";
            paragon.towerSet = "Primary";
            paragon.emoteSpriteLarge = new("Paragon");
            paragon.tier = 0;
            paragon.tiers = new int[] { 0, 0, 0 };
            paragon.cost = 450000;
            paragon.canAlwaysBeSold = true;
            paragon.isParagon = false;
            paragon.dontDisplayUpgrades = true;
            paragon.icon = paragon.portrait;

            if (!LocalizationManager.Instance.textTable.ContainsKey("ParagonBoomerangMonkey"))
                LocalizationManager.Instance.textTable.Add("ParagonBoomerangMonkey", "Glaive Dominus");

            var paragonDetails = model.towerSet[0].Clone().Cast<TowerDetailsModel>();
            paragonDetails.towerId = "ParagonBoomerangMonkey";
            paragonDetails.towerIndex = ++index;

            return (paragon, paragonDetails, "BoomerangMonkey", origParagon);
        }
    }
}
