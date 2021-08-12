using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Data;
using Assets.Scripts.Data.MapSets;
using Assets.Scripts.Models.Profile;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Menu;
using Assets.Scripts.Unity.Player;
using Assets.Scripts.Unity.UI_New;
using Assets.Scripts.Unity.UI_New.InGame;
using Assets.Scripts.Unity.UI_New.Main.MapSelect;
using Assets.Scripts.Utils;
using BTD6_Expansion.Utilities;
using HarmonyLib;
using Maps.Maps;
using Maps.Util;
using MelonLoader;
using NinjaKiwi.Common;
using SixthTiers.Utils;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using Task = Il2CppSystem.Threading.Tasks.Task;

[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
[assembly: MelonInfo(typeof(Maps.Entry), "Maps", "1.4", "1330 Studios LLC")]
namespace Maps {
    class Entry : MelonMod {
        public static AssetBundle SceneBundle = null;
        public override void OnApplicationStart() {
            SceneBundle = AssetBundle.LoadFromMemory(Properties.Resources.map);
            MelonLogger.Msg(ConsoleColor.Red, "Maps Loaded!");
        }

        [HarmonyPatch(typeof(InGame), "Update")]
        public class Update_Patch {
            private static float lastX = 0, lastY = 0;
            private static bool run = false;

            [HarmonyPostfix]
            public static void Postfix() {
                var cursorPosition = InGame.instance.inputManager.cursorPositionWorld;
                if (Input.GetKeyDown(KeyCode.LeftBracket)) {
                    run = !run;
                    if (lastX != cursorPosition.x || lastY != cursorPosition.y) {
                        Console.WriteLine("initArea.Add(new(" + Math.Round(cursorPosition.x) + ", " + Math.Round(cursorPosition.y) + "));");
                        lastX = cursorPosition.x;
                        lastY = cursorPosition.y;
                    }
                }
                if (Input.GetKeyDown(KeyCode.RightBracket)) {
                    run = !run;
                    if (lastX != cursorPosition.x || lastY != cursorPosition.y) {
                        Console.WriteLine("initPath.Add(new(){ point = new(" + Math.Round(cursorPosition.x) + ", " + Math.Round(cursorPosition.y) + "), bloonScale = 1, moabScale = 1 });");
                        lastX = cursorPosition.x;
                        lastY = cursorPosition.y;
                    }
                }

                if (Input.GetKeyDown(KeyCode.KeypadDivide)) {
                    run = !run;
                    if (lastX != cursorPosition.x || lastY != cursorPosition.y) Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                }
            }
        }

        [HarmonyPatch(typeof(Game), nameof(Game.Awake))]
        public class GameAwake {
            [HarmonyPostfix]
            public static void Postfix(ref Game __instance) {
                var ms = ScriptableObjectSingleton<GameData>._instance.mapSet.Maps.items;

                for (var i = 0; i < ms.Length; i++) {
                    ms[i].isDebug = ms[i].isBrowserOnly = false;
                }

                var impls = new MapImpl[] { BigFoot.VALUE, Daffodils.VALUE, Shipwreck.VALUE };

                ScriptableObjectSingleton<GameData>._instance.mapSet.Maps.items = ms.AddAll(impls.Select(impl => impl.GetCreated()).ToArray());
            }
        }

        [HarmonyPatch(typeof(ResourceLoader), "LoadSpriteFromSpriteReferenceAsync")]
        public record ResourceLoader_Patch {
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference, Image image) {
                if (reference != null)
                {
                    var bitmap = Properties.Resources.ResourceManager.GetObject(reference.guidRef) as byte[];
                    if (bitmap != null) {
                        var texture = new Texture2D(0, 0);
                        ImageConversion.LoadImage(texture, bitmap);
                        image.canvasRenderer.SetTexture(texture);
                        image.sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height),
                            new(), 10.2f);
                    }else {
                        var b = Properties.Resources.ResourceManager.GetObject(reference.guidRef);
                        if (b != null)
                        {
                            var bm = (byte[])new ImageConverter().ConvertTo(b, typeof(byte[]));
                            var texture = new Texture2D(0, 0);
                            ImageConversion.LoadImage(texture, bm);
                            image.canvasRenderer.SetTexture(texture);
                            image.sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height),
                                new(), 10.2f);
                        }
                    }
                }
            }
        }
    }
}
