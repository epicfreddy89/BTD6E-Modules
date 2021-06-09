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
using AdditionalTiers.Utils.Assets;
using AdditionalTiers.Utils.Components;
using TMPro;
using UnhollowerBaseLib;
using UnhollowerBaseLib.Attributes;
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
        [HideFromIl2Cpp] public static Dictionary<string, Type> Types { get; set; } = new() {
            { "WhitesnakeProj", Il2CppType.Of<AnimatedEnergyTexture>() },
            { "WhitesnakePheonixProj", Il2CppType.Of<AnimatedFlameTexture>() },
            { "WhitesnakeDarkPheonixProj", Il2CppType.Of<AnimatedDarkFlameTexture>() }
        };
        [HideFromIl2Cpp] public static Dictionary<string, Action<UnityDisplayNode>> Actions { get; set; } = new() {};

        private static AssetBundle _shader = null;

        public static AssetBundle ShaderBundle {
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
                var assets = ShaderBundle.LoadAllAssets();

                using (var enumerator = allAssetsKnown.GetEnumerator()) {
                    while (enumerator.MoveNext()) {
                        var curAsset = enumerator.Current;
                        if (objectId.Equals(curAsset.CustomAssetName)) {
                            UnityDisplayNode udn = null;
                            __instance.FindAndSetupPrototypeAsync(curAsset.BTDAssetName,
                                new System.Action<UnityDisplayNode>(
                                    btdUdn => {
                                        var instance = Object.Instantiate(btdUdn, __instance.PrototypeRoot);
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

                                        for (var i = 0; i < instance.genericRenderers.Length; i++) {
                                            if (instance.genericRenderers[i].GetIl2CppType() == rendererType) {
                                                if (curAsset.RendererType != RendererType.SPRITERENDERER) {
                                                    var renderer = instance.genericRenderers[i].Cast<Renderer>();
                                                    renderer.material.shader = assets[0].Cast<Shader>();
                                                    renderer.material.SetColor("_OutlineColor", Color.black);
                                                    renderer.material.mainTexture = CacheBuilder.Get(objectId);
                                                } else {
                                                    var spriteRenderer = instance.genericRenderers[i].Cast<SpriteRenderer>();
                                                    spriteRenderer.sprite = SpriteBuilder.createProjectile(CacheBuilder.Get(objectId));
                                                    if (Types.ContainsKey(objectId)) {
                                                        spriteRenderer.gameObject.AddComponent(Types[objectId]);
                                                    }
                                                }
                                            }
                                        }
                                        
                                        
                                        if (Actions.ContainsKey(objectId))
                                            Actions[objectId](instance);

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
                            nktmp.m_fontColorGradient = new(Color.red, Color.red, new(255, 255, 0), Color.white);
                            nktmp.capitalize = false;
                            udn = nudn;
                            onComplete.Invoke(udn);
                        }));
                    return false;
                }
                if (objectId.Equals("CashText")) {
                    UnityDisplayNode udn = null;
                    __instance.FindAndSetupPrototypeAsync("3dcdbc19136c60846ab944ada06695c0",
                        new System.Action<UnityDisplayNode>(oudn => {
                            var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                            nudn.name = objectId + "(Clone)";
                            nudn.isSprite = true;
                            nudn.RecalculateGenericRenderers();
                            var nktmp = nudn.GetComponentInChildren<NK_TextMeshPro>();
                            nktmp.m_fontColorGradient = new(Color.green, Color.green, new(35, 255, 35), Color.white);
                            nktmp.capitalize = false;
                            udn = nudn;
                            onComplete.Invoke(udn);
                        }));
                    return false;
                }

                return true;
            }

            public static void Build() {
                for (var i = 0; i < AdditionalTiers.towers.Count; i++) {
                    var assets = AdditionalTiers.towers[i].assetsToRead;
                    if (assets != null)
                        using (var enumerator = assets.GetEnumerator()) while (enumerator.MoveNext()) allAssetsKnown.Add(enumerator.Current);
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

        public static class AnimatedAssets {
            private static List<Sprite> _energySprites = new();

            public static List<Sprite> EnergySprites {
                get {
                    if (_energySprites == null || _energySprites.Count == 0) for (var index = 0; index < 64; index++) _energySprites.Add(SpriteBuilder.createProjectile(CacheBuilder.Get(string.Format("energy{0}", index))));
                    return _energySprites;
                }
            }
            private static List<Sprite> _flameSprites = new();

            public static List<Sprite> FlameSprites {
                get {
                    if (_flameSprites == null || _flameSprites.Count == 0) for (var index = 0; index < 10; index++) _flameSprites.Add(SpriteBuilder.createProjectile(CacheBuilder.Get(string.Format("Flame{0}", index)), 7.6f, 0.5f, 0));
                    return _flameSprites;
                }
            }
            private static List<Sprite> _darkFlameSprites = new();

            public static List<Sprite> DarkFlameSprites {
                get {
                    if (_darkFlameSprites == null || _darkFlameSprites.Count == 0) for (var index = 0; index < 10; index++) _darkFlameSprites.Add(SpriteBuilder.createProjectile(CacheBuilder.Get(string.Format("DarkFlame{0}", index)), 7.6f, 0.5f, 0));
                    return _darkFlameSprites;
                }
            }

            static AnimatedAssets() {
                var gcMe = EnergySprites;
                gcMe = FlameSprites;
                gcMe = DarkFlameSprites;
            }
        }
    }
}