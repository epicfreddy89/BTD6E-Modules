using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AdditionalBloons.Resources;
using AdditionalBloons.Utils;
using Assets.Scripts.Models;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Utils;
using Harmony;
using Il2CppSystem.IO;
using Il2CppSystem.Reflection;
using MelonLoader;
using TMPro;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using BindingFlags = System.Reflection.BindingFlags;
using Color = UnityEngine.Color;
using Convert = System.Convert;
using DateTime = System.DateTime;
using Image = UnityEngine.UI.Image;
using IntPtr = System.IntPtr;
using Marshal = Il2CppSystem.Runtime.InteropServices.Marshal;
using Object = UnityEngine.Object;
using Type = Il2CppSystem.Type;

namespace AdditionalBloons.Tasks {
    public class Assets {
        [HarmonyPatch(typeof(Factory), nameof(Factory.FindAndSetupPrototypeAsync))]
        public class DisplayFactory {
            public static List<AssetInfo> allAssetsKnown = new();

            [HarmonyPrefix]
            public static bool Prefix(Factory __instance, string objectId, Il2CppSystem.Action<UnityDisplayNode> onComplete) {
                using (var enumerator = allAssetsKnown.GetEnumerator()) {
                    while (enumerator.MoveNext()) {
                        AssetInfo curAsset = enumerator.Current;
                        if (objectId.Equals(curAsset.CustomAssetName)) {
                            GameObject obj = Object.Instantiate(new GameObject(objectId + "(Clone)"), __instance.PrototypeRoot);
                            var sr = obj.AddComponent<SpriteRenderer>();
                            sr.sprite = SpriteBuilder.createBloon(CacheBuilder.Get(objectId));
                            var udn = obj.AddComponent<UnityDisplayNode>();
                            udn.transform.position = new(-3000, 0);
                            onComplete.Invoke(udn);
                            return false;
                        }
                    }
                }
                return true;
            }

            public static void Build() {
                using var en = BloonCreator.assets.GetEnumerator();
                while (en.MoveNext()) {
                    allAssetsKnown.Add(en.Current);
                }
            }

            public static void Flush() => allAssetsKnown.Clear();
        }

        [HarmonyPatch(typeof(ResourceLoader), nameof(ResourceLoader.LoadSpriteFromSpriteReferenceAsync))]
        public class ResourceLoader_Patch {
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference, Image image)
            {
                if (reference != null)
                {
                    var bitmap = BloonSprites.ResourceManager.GetObject(reference.guidRef) as byte[];
                    if (bitmap != null)
                    {
                        var texture = new Texture2D(0, 0);
                        ImageConversion.LoadImage(texture, bitmap);
                        image.canvasRenderer.SetTexture(texture);
                        image.sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(), 10.2f);
                    }
                    else
                    {
                        var b = BloonSprites.ResourceManager.GetObject(reference.guidRef);
                        if (b != null)
                        {
                            var bm = (byte[])new ImageConverter().ConvertTo(b, typeof(byte[]));
                            var texture = new Texture2D(0, 0);
                            ImageConversion.LoadImage(texture, bm);
                            image.canvasRenderer.SetTexture(texture);
                            image.sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(), 10.2f);
                        }
                    }
                }
            }
        }
    }
}