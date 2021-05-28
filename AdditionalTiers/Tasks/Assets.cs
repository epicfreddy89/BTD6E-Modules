using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Assets.Scripts.Models;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Utils;
using Harmony;
using Il2CppSystem.IO;
using Il2CppSystem.Reflection;
using MelonLoader;
using AdditionalTiers.Resources;
using AdditionalTiers.Utils;
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

namespace AdditionalTiers.Tasks {
    public class Assets {
        private static AssetBundle _shader = null;

        public static AssetBundle shaderBundle {
            get {
                if (_shader == null)
                    _shader = AssetBundle.LoadFromMemory(Images.Shader);
                return _shader;
            }
        }

        [HarmonyPatch(typeof(Factory), nameof(Factory.FindAndSetupPrototypeAsync))]
        public class DisplayFactory {
            public static List<AssetInfo> allAssetsKnown = new();

            [HarmonyPrefix]
            public static bool Prefix(Factory __instance, string objectId, Il2CppSystem.Action<UnityDisplayNode> onComplete) {
                var assets = shaderBundle.LoadAllAssets();

                using (var enumerator = allAssetsKnown.GetEnumerator()) {
                    while (enumerator.MoveNext()) {
                        AssetInfo curAsset = enumerator.Current;
                        if (objectId.Equals(curAsset.CustomAssetName)) {
                            UnityDisplayNode udn = null;
                            __instance.FindAndSetupPrototypeAsync(curAsset.BTDAssetName, new System.Action<UnityDisplayNode>(
                                btdUDN => {
                                    var instance = Object.Instantiate(btdUDN, __instance.PrototypeRoot);
                                    instance.name = objectId + "(Clone)";
                                    if (curAsset.RendererType == RendererType.SPRITERENDERER)
                                        instance.isSprite = true;
                                    instance.RecalculateGenericRenderers();
                                    
                                    Il2CppSystem.Type rendererType = null;
                                    switch (curAsset.RendererType) {
                                        case RendererType.MESHRENDERER:
                                            rendererType = Il2CppType.Of<MeshRenderer>();
                                            break;
                                        case RendererType.SPRITERENDERER:
                                            rendererType = Il2CppType.Of<SpriteRenderer>();
                                            break;
                                        case RendererType.SKINNEDMESHRENDERER:
                                            rendererType = Il2CppType.Of<SkinnedMeshRenderer>();
                                            break;
                                    }

                                    if (rendererType == null)
                                        throw new NullReferenceException("rendererType is still null, don't leave things unset.");
                                    
                                    for (int i = 0; i < instance.genericRenderers.Length; i++) {
                                        if (instance.genericRenderers[i].GetIl2CppType() == rendererType) {
                                            if (curAsset.RendererType != RendererType.SPRITERENDERER) {
                                                var renderer = instance.genericRenderers[i].Cast<Renderer>();
                                                renderer.material.shader = assets[0].Cast<Shader>();
                                                renderer.material.SetColor("_OutlineColor", Color.black);
                                                renderer.material.mainTexture = CacheBuilder.Get(objectId);
                                            } else
                                                instance.genericRenderers[i].Cast<SpriteRenderer>().sprite = SpriteBuilder.createProjectile(CacheBuilder.Get(objectId));
                                        }
                                    }

                                    udn = instance;
                                    onComplete.Invoke(udn);
                                }));
                            return false;
                        }
                    }
                }
                
                if (objectId.Equals("UpgradedText")) {
                    UnityDisplayNode udn = null;
                    __instance.FindAndSetupPrototypeAsync("3dcdbc19136c60846ab944ada06695c0",
                        new System.Action<UnityDisplayNode>(oudn => {
                            var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                            nudn.name = objectId + "(Clone)";
                            nudn.isSprite = true;
                            nudn.RecalculateGenericRenderers();
                            var nktmp = nudn.GetComponentInChildren<NK_TextMeshPro>();
                            nktmp.m_fontColorGradient = new(Color.red, Color.red, new(255,255,0), Color.white);
                            udn = nudn;
                            onComplete.Invoke(udn);
                        }));
                    return false;
                }
                return true;
            }

            public static void Build() {
                for (int i = 0; i < AdditionalTiers.towers.Count; i++) {
                    var assets = AdditionalTiers.towers[i].assetsToRead;
                    if (assets != null) 
                        using (var enumerator = assets.GetEnumerator())
                            while (enumerator.MoveNext())
                                allAssetsKnown.Add(enumerator.Current);
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
                    var bitmap = Images.ResourceManager.GetObject(reference.guidRef) as byte[];
                    if (bitmap != null)
                    {
                        var texture = new Texture2D(0, 0);
                        ImageConversion.LoadImage(texture, bitmap);
                        image.canvasRenderer.SetTexture(texture);
                        image.sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(), 10.2f);
                    }
                    else
                    {
                        var b = Images.ResourceManager.GetObject(reference.guidRef);
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