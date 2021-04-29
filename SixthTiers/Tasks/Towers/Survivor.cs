
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
using Assets.Scripts.Models.Towers.Weapons;
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
    public class Survivor : TowerTask {
        public static TowerModel underWorld;
        private static int time = -1;
        public Survivor() {
            identifier = "Survivor";
            requirements += tts => tts.tower.towerModel.baseId.Equals("SniperMonkey") && tts.tower.towerModel.tiers[2] == 5 && tts.damageDealt > 100000;
            onComplete += tts => {
                if (time < 50) {
                    time++;
                    return;
                }
                tts.tower.namedMonkeyName = identifier;
                tts.tower.worth = 0;
                tts.tower.UpdateRootModel(underWorld);
                tts.sim.simulation.CreateTextEffect(new(tts.position), "UpgradedText", 10, "Upgraded!", false);
                AbilityMenu.instance.TowerChanged(tts);
                AbilityMenu.instance.RebuildAbilities();
            };
            gameLoad += gm => {
                underWorld = gm.towers.First(a => a.name.Contains("SniperMonkey-205")).Clone()
                    .Cast<TowerModel>();

                underWorld.cost = 0;
                underWorld.name = "Survivor";
                underWorld.baseId = "Survivor";
                underWorld.display = "Survivor";
                underWorld.dontDisplayUpgrades = true;
                underWorld.portrait = new("SurvivorIcon");
                underWorld.behaviors.First(a => a.GetIl2CppType() == Il2CppType.Of<DisplayModel>()).Cast<DisplayModel>().display = "Survivor";

                var beh = underWorld.behaviors;

                for (var i = 0; i < beh.Length; i++)
                    if (beh[i].GetIl2CppType() == Il2CppType.Of<AttackModel>()) {
                        var am = beh[i].Cast<AttackModel>();

                        for (var j = 0; j < am.weapons.Length; j++) {
                            var we = am.weapons[j];
                            we.rate = 0;

                            am.weapons[j] = we;
                        }

                        beh[i] = am;
                    }

                underWorld.behaviors = beh.Add(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
            };
            recurring += tts => {};
            onLeave += () => { time = -1; };
            CacheBuilder.toBuild.PushAll("Survivor", "SurvivorIcon");
            CacheBuilder.Build();
        }
    }
}