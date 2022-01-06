using System;
using System.Collections.Generic;

using AdditionalBloons.Utils;

using Assets.Scripts.Models.Bloons;
using Assets.Scripts.Simulation.Bloons;
using Assets.Scripts.Simulation.Track;
using Assets.Scripts.Unity.UI_New.InGame;

namespace AdditionalBloons.Tasks {
    internal sealed class BloonTaskRunner {
        public static BloonQueue bloonQueue = new();

        // Key of bloontask which stores all needed info, Value of KeyValuePair with key of amount sent out of total and value of time spent
        private static Dictionary<BloonTask, KeyValuePair<int, int>> bloonTasks = new();
        private static Random rand = new(System.DateTime.Now.Millisecond);

        internal static void Run() {
            //Add to dictionary from queue
            while (bloonQueue.Count > 0) bloonTasks.Add(bloonQueue.Dequeue(), new KeyValuePair<int, int>(0, 0));

            if (InGame.instance?.bridge != null) {
                #region Bloon Spawner

                if (bloonTasks.Count > 0) {
                    List<BloonTask> remove = new();
                    Dictionary<BloonTask, KeyValuePair<int, int>> update = new();

                    foreach (var entry in bloonTasks) {
                        //exception for it updatesBetween is 0 so they get sent at the same time
                        bool shouldContinue = true;
                        if (entry.Key.updatesBetween is 0)
                            for (int i = 0; i < entry.Key.amount; i++) {
                                InGame.instance.bridge.simulation.map.spawner.Emit(entry.Key.model,
                                    InGame.instance.bridge.simulation.GetCurrentRound(), 0);
                                remove.Add(entry.Key);
                                shouldContinue = false;
                            }

                        if (!shouldContinue) break;

                        //timing for if should be sent
                        if (entry.Value.Key < entry.Key.amount && (entry.Value.Value % entry.Key.updatesBetween == 0))
                            InGame.instance.bridge.simulation.map.spawner.Emit(entry.Key.model,
                                InGame.instance.bridge.simulation.GetCurrentRound(), 0);

                        //if expired, remove
                        if (entry.Value.Value > entry.Key.amount * entry.Key.updatesBetween) remove.Add(entry.Key);
                        else {
                            //if it was sent add to the key
                            var keyNewVal = entry.Value.Key;
                            if (entry.Value.Value % entry.Key.updatesBetween == 0)
                                keyNewVal++;
                            //update it with new timing
                            var kvp = new KeyValuePair<int, int>(keyNewVal, entry.Value.Value + 1);
                            update.Add(entry.Key, kvp);
                        }
                    }

                    //remove entries in the list from the dictionary
                    foreach (var toRemove in remove) bloonTasks.Remove(toRemove);
                    bloonTasks = update;
                }

                #endregion
            }
        }

        internal static bool Damage(ref Bloon __instance, ref float totalAmount) {
            var model = __instance.bloonModel;

            if (model.icon.guidRef.Equals("CoconutIcon")) {
                var healthPercent = model.maxHealth / __instance.health / 5f;

                if (__instance.spawnRound > 60)
                    healthPercent /= 2f;
                if (__instance.spawnRound > 100)
                    healthPercent /= 5f;

                __instance.Move(healthPercent);
                totalAmount = 1;
            }

            return true;
        }

        internal static bool Emit(ref Spawner __instance) {
            if (!__instance.isSandbox) {
                var i = 0;
                if (__instance.CurrentRound < 50)
                    i = rand.Next(100);
                else if (__instance.CurrentRound < 100)
                    i = rand.Next(50);
                else if (__instance.CurrentRound < 200)
                    i = rand.Next(20);
                else
                    i = rand.Next(10);


                if (i == 4) {
                    var coconut = BloonCreator.bloons.Find(a => a.icon.guidRef.Equals("CoconutIcon")).Clone().Cast<BloonModel>();
                    if (__instance.currentRound.ValueInt > 20) coconut.maxHealth = Math.Min(__instance.currentRound.ValueInt, 50);
                    __instance.Emit(coconut, __instance.currentRound.ValueInt, 0);
                }
            }

            return true;
        }

        internal static void Quit() {
            bloonQueue.Clear();
            bloonTasks.Clear();
            rand = new(DateTime.Now.Millisecond);
        }
    }
}