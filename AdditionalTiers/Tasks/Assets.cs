using AdditionalTiers.Resources;
using AdditionalTiers.Utils.Assets;
using AdditionalTiers.Utils.Components;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Utils;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib;
using UnityEngine;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;
using Object = UnityEngine.Object;
using Type = Il2CppSystem.Type;

namespace AdditionalTiers.Tasks {
    public class Assets {
        [HideFromIl2Cpp]
        public static Dictionary<string, Type> Types { get; set; } = new() {
            { "WhitesnakeProj", Il2CppType.Of<AnimatedEnergyTexture>() },
            { "WhitesnakePheonixProj", Il2CppType.Of<AnimatedFlameTexture>() },
            { "WhitesnakeDarkPheonixProj", Il2CppType.Of<AnimatedDarkFlameTexture>() }
        };
        [HideFromIl2Cpp]
        public static Dictionary<string, Action<UnityDisplayNode>> Actions { get; set; } = new() {
            {
                "GlaiveDominusSilver",
                udn => {
                    var light = udn.gameObject.AddComponent<Light>();
                    light.type = LightType.Directional;
                    light.renderMode = LightRenderMode.ForceVertex;

                    var assets = Particles;
                    var _obj = assets[0].Cast<GameObject>();
                    var obj = Object.Instantiate(_obj, udn.transform);
                    obj.SetActive(true);
                    var ps = obj.transform.GetComponentInChildren<ParticleSystem>();
                    ps.emissionRate = 15;
                    ps.transform.localScale = new(10, 10, 10);

                    // Can you all stop copying every little thing I program? At the very least, don't cite my work for an example but call it cancerous??
                }
            },
            {
                "APMGold2",
                udn => {
                    for (int i = 0; i < udn.genericRenderers.Length; i++) {
                        udn.genericRenderers[i].material.SetFloat("_OutlineWidth", 0.02f);
                    }
                }
            },
            {
                "GlaiveDominusSilverOrbit",
                udn => {
                    var ps = udn.transform.GetComponentInChildren<ParticleSystem>();
                    ps.startColor = new Color(1, 0.25f, 0);
                    udn.gameObject.AddComponent<FastRotation>();
                }
            },
            {
                "GlaiveDominusSilverOrbit2",
                udn => {
                    var ps = udn.transform.GetComponentInChildren<ParticleSystem>();
                    ps.startColor = new Color(1, 1, 1);
                    for (int i = 0; i < 2; i++)
                        udn.gameObject.AddComponent<FastRotation>();
                }
            },
            {
                "GlaiveDominusSilverOrbit3",
                udn => {
                    var ps = udn.transform.GetComponentInChildren<ParticleSystem>();
                    ps.startColor = new Color(0.3f, 1, 0.3f);
                    for (int i = 0; i < 5; i++)
                        udn.gameObject.AddComponent<FastRotation>();
                }
            },
            {
                "GlaiveDominusSilverAbility",
                udn => {
                    var ps = udn.transform.GetComponentInChildren<ParticleSystem>();
                    ps.startColor = new Color(1f, 0, 0);
                }
            },
            {
                "BTD4SunGod",
                udn => {
                    udn.transform.GetComponentInChildren<ParticleSystem>().gameObject.active = false;
                    udn.gameObject.AddComponent<SetHeight1>();
                }
            },
            {
                "BTD4SunGodV",
                udn => {
                    udn.transform.GetComponentInChildren<ParticleSystem>().gameObject.active = false;
                    udn.gameObject.AddComponent<SetHeight1>();
                }
            }
        };
        [HideFromIl2Cpp]
        public static Dictionary<string, Func<string, Sprite>> SpriteCreation { get; set; } = new() {
            { "ScaryMonstersProj", objectId => SpriteBuilder.createProjectile(CacheBuilder.Get(objectId), 10.8f) },
            { "GlaiveDominusSilverOrbit2", objectId => SpriteBuilder.createProjectile(CacheBuilder.Get(objectId), 10.8f) },
            { "BTD4SunGod", objectId => SpriteBuilder.createProjectile(CacheBuilder.Get(objectId), 43.2f, pivoty: 0.7f) },
            { "BTD4SunGodV", objectId => SpriteBuilder.createProjectile(CacheBuilder.Get(objectId), 43.2f, pivoty: 0.7f) }
        };


        [HideFromIl2Cpp]
        public static Dictionary<string, Func<Object, bool>> SpecialShaderIndicies { get; set; } = new() {
            { "GlaiveDominusSilver", obj => obj.name.StartsWith("Unlit/Metallic") }
        };

        private static AssetBundle _shader = null;

        public static AssetBundle ShaderBundle {
            get {
                if (_shader == null)
                    _shader = AssetBundle.LoadFromMemory(Images.shader);
                return _shader;
            }
        }

        private static AssetBundle _particle = null;

        public static AssetBundle ParticleBundle {
            get {
                if (_particle == null)
                    _particle = AssetBundle.LoadFromMemory(Images.particle);
                return _particle;
            }
        }

        private static Object[] _particles = null;
        public static Object[] Particles {
            get {
                if (_particles == null)
                    _particles = ParticleBundle.LoadAllAssets();
                return _particles;
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
                                new Action<UnityDisplayNode>(
                                    btdUdn => {
                                        var instance = Object.Instantiate(btdUdn, __instance.PrototypeRoot);
                                        instance.name = objectId + "(Clone)";
                                        if (curAsset.RendererType == RendererType.SPRITERENDERER)
                                            instance.isSprite = true;
                                        instance.RecalculateGenericRenderers();

                                        Type rendererType = null;
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
                                            case RendererType.PARTICLESYSTEMRENDERER:
                                                rendererType = Il2CppType.Of<ParticleSystemRenderer>();
                                                break;
                                        }

                                        if (rendererType == null)
                                            throw new NullReferenceException("rendererType is still null, don't leave things unset.");

                                        for (var i = 0; i < instance.genericRenderers.Length; i++) {
                                            if (instance.genericRenderers[i].GetIl2CppType() == rendererType) {
                                                if (curAsset.RendererType != RendererType.SPRITERENDERER && curAsset.RendererType != RendererType.PARTICLESYSTEMRENDERER) {
                                                    var renderer = instance.genericRenderers[i].Cast<Renderer>();
                                                    if (!SpecialShaderIndicies.ContainsKey(objectId))
                                                        renderer.material.shader = assets.First(a => a.name.StartsWith("Unlit/CelShading")).Cast<Shader>();
                                                    else
                                                        renderer.material.shader = assets.First(SpecialShaderIndicies[objectId]).Cast<Shader>();
                                                    renderer.material.SetColor("_OutlineColor", Color.black);
                                                    renderer.material.mainTexture = CacheBuilder.Get(objectId);
                                                } else if (curAsset.RendererType == RendererType.SPRITERENDERER) {
                                                    var spriteRenderer = instance.genericRenderers[i].Cast<SpriteRenderer>();
                                                    if (SpriteCreation.ContainsKey(objectId))
                                                        spriteRenderer.sprite = SpriteCreation[objectId](objectId);
                                                    else
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
                        new Action<UnityDisplayNode>(oudn => {
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
                        new Action<UnityDisplayNode>(oudn => {
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
                for (var i = 0; i < AdditionalTiers.towers.Length; i++) {
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
            public static void Postfix(SpriteReference reference, Image image) {
                if (reference != null) {
                    var bitmap = Images.ResourceManager.GetObject(reference.guidRef) as byte[];
                    if (bitmap != null) {
                        var texture = new Texture2D(0, 0);
                        ImageConversion.LoadImage(texture, bitmap);
                        image.canvasRenderer.SetTexture(texture);
                        image.sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), new(), 10.2f);
                    } else {
                        var b = Images.ResourceManager.GetObject(reference.guidRef);
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