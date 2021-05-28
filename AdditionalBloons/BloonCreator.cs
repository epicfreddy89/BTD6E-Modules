using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdditionalBloons.Tasks;
using AdditionalBloons.Utils;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Bloons;
using Assets.Scripts.Models.Bloons.Behaviors;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Simulation.Bloons;
using Assets.Scripts.Simulation.SMath;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.UI_New.InGame;
using Assets.Scripts.Unity.UI_New.InGame.BloonMenu;
using Harmony;
using MelonLoader;
using Newtonsoft.Json;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using Bloon = Assets.Scripts.Unity.Bloons.Bloon;

namespace AdditionalBloons {
    public class BloonCreator {
        public static List<BloonModel> bloons = new();
        public static List<AssetInfo> assets = new();
        internal static BloonModel bloonBase;

        internal static void GameLoad(ref GameModel __result) {
            bloonBase = __result.bloons[0].Clone().Cast<BloonModel>();

            #region Bloon Defs

            #region Base Game

            ModelStorage.whiteBloon = __result.bloons.First(a => a.baseId.Equals("White")).Clone().Cast<BloonModel>();

            #endregion

            #region Golden

            bloons.Add(__result.bloons.First(a => a.baseId.ToLower().Equals("golden")));

            #endregion

            #region Coconut

            for (int i = 1; i < 6; i++) assets.Add(new($"Coconut{i}", bloonBase.display, RendererType.SPRITERENDERER));

            var coconutBloon = __result.bloons.First(a => a.baseId.Equals("Ceramic")).Clone().Cast<BloonModel>();
            coconutBloon.display = "Coconut1";
            coconutBloon.icon = new("CoconutIcon");
            coconutBloon.radius = 8;
            coconutBloon.danger = 10;
            coconutBloon.distributeDamageToChildren = false;
            coconutBloon.speedFrames = 1.04166675f;
            coconutBloon.Speed = 62.5f;
            coconutBloon.leakDamage = coconutBloon.maxHealth = 20;
            coconutBloon.tags = new Il2CppStringArray(new string[] {"Coconut", "NA"});
            coconutBloon.damageDisplayStates = new(new DamageStateModel[] {
                new DamageStateModel("DamageStateModel_4", "Coconut5", 0.2f),
                new DamageStateModel("DamageStateModel_3", "Coconut4", 0.4f),
                new DamageStateModel("DamageStateModel_2", "Coconut3", 0.6f),
                new DamageStateModel("DamageStateModel_1", "Coconut2", 0.8f)
            });
            var coconutBloonChildren = new global::Il2CppSystem.Collections.Generic.List<BloonModel>();
            for (int i = 0; i < 5; i++) coconutBloonChildren.Add(ModelStorage.whiteBloon);
            coconutBloon.childBloonModels = coconutBloonChildren;
            coconutBloon.UpdateChildBloonModels();
            for (int i = 0; i < coconutBloon.behaviors.Length; i++) {
                var behavior = coconutBloon.behaviors[i];

                if (behavior.GetIl2CppType() == Il2CppType.Of<SpawnChildrenModel>())
                    behavior.Cast<SpawnChildrenModel>().children =
                        new string[] {"White", "White", "White", "White", "White"};
                if (behavior.GetIl2CppType() == Il2CppType.Of<DamageStateModel>()) {
                    switch (behavior.Cast<DamageStateModel>().healthPercent) {
                        case 0.2f:
                            behavior.Cast<DamageStateModel>().displayPath = "Coconut5";
                            break;
                        case 0.4f:
                            behavior.Cast<DamageStateModel>().displayPath = "Coconut4";
                            break;
                        case 0.6f:
                            behavior.Cast<DamageStateModel>().displayPath = "Coconut3";
                            break;
                        case 0.8f:
                            behavior.Cast<DamageStateModel>().displayPath = "Coconut2";
                            break;
                    }
                }

                coconutBloon.behaviors[i] = behavior;
            }

            bloons.Add(coconutBloon);

            #endregion

            #endregion

            Tasks.Assets.DisplayFactory.Build();
        }

        internal static bool BloonMenuCreate(ref Il2CppSystem.Collections.Generic.List<BloonModel> sortedBloons) {
            for (int i = 0; i < bloons.Count; i++) sortedBloons.Add(bloons[i]);
            return true;
        }

        internal static bool SpawnBloon(ref SpawnBloonButton __instance) {
            for (int i = 0; i < bloons.Count; i++)
                if (__instance.model.icon.guidRef.Equals(bloons[i].icon.guidRef)) {
                    int amount = int.Parse(__instance.count.text);
                    int delay = int.Parse(__instance.rate.text);

                    BloonTaskRunner.bloonQueue.Enqueue(new(bloons[i], amount, delay));

                    return false;
                }

            return true;
        }
    }
}