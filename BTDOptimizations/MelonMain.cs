#define AGGRESSIVE_TACTICS

using HarmonyLib;
using MelonLoader;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Assets.Scripts.Utils;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Scripting;

[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
[assembly: MelonInfo(typeof(BTDOptimizer.MelonMain), "BTD6 Optimizer", "1.0", "1330 Studios LLC")]

namespace BTDOptimizer {
    public class MelonMain : MelonMod {
        
        internal delegate IntPtr intIntPtrDelegate(int val);

        public override void OnSceneWasLoaded(int buildIndex, string sceneName) => Set();
        public override void OnApplicationLateStart() => Set(true);

#if AGGRESSIVE_TACTICS
        private IEnumerator GC;
        public override void OnApplicationQuit() => MelonCoroutines.Stop(GC==null?null:GC);
#endif

        internal void Set(bool val = false) {
#if AGGRESSIVE_TACTICS
            //IL2CPP.ResolveICall<intIntPtrDelegate>("UnityEngine.QualitySettings::set_vSyncCount")(0);
            var mods = AppDomain.CurrentDomain.GetAssemblies();
            GC = GCTime();
            MelonCoroutines.Start(GC);
#else
            var mods = MelonHandler.Mods.Select(a=>a.Assembly).ToArray();
#endif

            if (!val) return;

            var sw = new Stopwatch();
            sw.Start();
            var methodCount = 0;
            
            #region RuntimeHelpers
            
            for (var i = 0; i < mods.Count(); i++) {
                var asm = mods[i];
                var types = asm.GetTypes();
                for (var j = 0; j < types.Length; j++)
                    try {
                        var type = types[j];
                        var methods = AccessTools.GetDeclaredMethods(type);
                        for (var k = 0; k < methods.Count; k++) {
                            RuntimeHelpers.PrepareMethod(methods[k].MethodHandle);
                            methodCount += 1;
                        }
                    } catch {}
            }

            #endregion
            
            sw.Stop();
            MelonLogger.Msg($"Optimized {methodCount:N0} methods in {sw.Elapsed.Milliseconds:N0} milliseconds!");
        }

        long lastFrameMemory = 0;
        internal IEnumerator GCTime() {
            RuntimeHelpers.PrepareConstrainedRegionsNoOP();
            while (true) {
                long mem = Profiler.GetMonoUsedSizeLong();
                yield return new WaitForSeconds(15f);
                System.GC.Collect();
                GarbageCollector.CollectIncremental(0);
                long dif = mem - Profiler.GetMonoUsedSizeLong();
                MelonLogger.Msg($"{Math.Abs(dif)}MB collected");
                lastFrameMemory = mem;
            }
        }
    }
}
