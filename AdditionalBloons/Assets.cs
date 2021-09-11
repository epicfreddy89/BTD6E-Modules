using System.Collections.Generic;
using System.Drawing;

using AdditionalBloons.Resources;
using AdditionalBloons.Utils;

using Assets.Scripts.Unity.Display;
using Assets.Scripts.Utils;

using HarmonyLib;

using UnityEngine;

using Image = UnityEngine.UI.Image;
using Object = UnityEngine.Object;

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
            public static void Postfix(SpriteReference reference, Image image) {
                if (reference != null) {
                    var bitmap = BloonSprites.ResourceManager.GetObject(reference.guidRef) as byte[];
                    if (bitmap != null) {
                        var texture = new Texture2D(0, 0);
                        ImageConversion.LoadImage(texture, bitmap);
                        image.canvasRenderer.SetTexture(texture);
                        image.sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(), 10.2f);
                    } else {
                        var b = BloonSprites.ResourceManager.GetObject(reference.guidRef);
                        if (b != null) {
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