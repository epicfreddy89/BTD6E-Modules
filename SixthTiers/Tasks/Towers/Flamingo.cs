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
using SixthTiers.Utils;
using UnhollowerRuntimeLib;
using UnityEngine;
using v = Assets.Scripts.Simulation.SMath.Vector3;

namespace SixthTiers.Tasks.Towers {
    public class Flamingo : TowerTask{
        public static TowerModel yellowSubmarine;
        private static int time = -1;

        //TODO Finish code
        public Flamingo()
        {
            identifier = "Flamingo";
            requirements += tts => tts.tower.towerModel.baseId.Equals("MonkeySub") && tts.tower.towerModel.tiers[1] == 5 && tts.damageDealt > 100000;
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
            gameLoad += gm =>
            {
                yellowSubmarine = gm.towers.First(a => a.name.Contains("MonkeySub-052")).Clone()
                    .Cast<TowerModel>();

                yellowSubmarine.range = 150;
                yellowSubmarine.cost = 0;
                yellowSubmarine.name = "Flamingo";
                yellowSubmarine.baseId = "Flamingo";
                yellowSubmarine.display = "Flamingo";
                yellowSubmarine.dontDisplayUpgrades = true;
                yellowSubmarine.portrait = new("YellowSubmarineIcon");
                yellowSubmarine.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<DisplayModel>())
                    .Cast<DisplayModel>().display = "Flamingo";
                var beh = yellowSubmarine.behaviors;

                yellowSubmarine.behaviors = beh.Add(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            };
            recurring += tts => { };
            onLeave += () => { time = -1; };
            CacheBuilder.toBuild.PushAll("YellowSubmarine", "YellowSubmarineIcon");
            CacheBuilder.Build();
        }
    }
}