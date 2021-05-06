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
            public static Dictionary<string, UnityDisplayNode> protos = new();

            [HarmonyPrefix]
            public static bool Prefix(Factory __instance, string objectId, Il2CppSystem.Action<UnityDisplayNode> onComplete) {
                if (!protos.ContainsKey(objectId)) {
                    var assets = shaderBundle.LoadAllAssets();
                    if (objectId.Equals("WhiteWedding")) {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("e6c683076381222438dfc733a602c157",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                    if (nudn.genericRenderers[i].GetIl2CppType() ==
                                        Il2CppType.Of<SkinnedMeshRenderer>()) {
                                        var smr = nudn.genericRenderers[i].Cast<SkinnedMeshRenderer>();
                                        smr.material.shader = assets[0].Cast<Shader>();
                                        smr.material.mainTexture = CacheBuilder.Get(objectId);
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }
                    if (objectId.Equals("SuperFly")) {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("06b880ab7e2941b4f9de3e132ba1e11e",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                    if (nudn.genericRenderers[i].GetIl2CppType() ==
                                        Il2CppType.Of<SkinnedMeshRenderer>()) {
                                        var smr = nudn.genericRenderers[i].Cast<SkinnedMeshRenderer>();
                                        smr.material.shader = assets[0].Cast<Shader>();
                                        smr.material.mainTexture = CacheBuilder.Get(objectId);
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }
                    if (objectId.Equals("BigJuggus"))
                    {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("b194c58ed09f1aa468e935b453c6843c",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++)
                                {
                                    if (nudn.genericRenderers[i].GetIl2CppType() ==
                                        Il2CppType.Of<SkinnedMeshRenderer>())
                                    {
                                        var smr = nudn.genericRenderers[i].Cast<SkinnedMeshRenderer>();
                                        smr.material.shader = assets[0].Cast<Shader>();
                                        smr.material.mainTexture = CacheBuilder.Get(objectId);
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }
                    if (objectId.Equals("YellowSubmarine"))
                    {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("62ff4c3f34f9c3c4c9fce1ac3d122ee0",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++)
                                {
                                    if (nudn.genericRenderers[i].GetIl2CppType() ==
                                        Il2CppType.Of<SkinnedMeshRenderer>())
                                    {
                                        var smr = nudn.genericRenderers[i].Cast<SkinnedMeshRenderer>();
                                        smr.material.shader = assets[0].Cast<Shader>();
                                        smr.material.mainTexture = CacheBuilder.Get(objectId);
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }
                    
                    if (objectId.Equals("BigJuggusProj")) {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("c4b8e7aa3e07d764fb9c3c773ceec2ab",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                    if (nudn.genericRenderers[i].GetIl2CppType() == Il2CppType.Of<MeshRenderer>()) {
                                        var smr = nudn.genericRenderers[i].Cast<MeshRenderer>();
                                        smr.material.shader = assets[0].Cast<Shader>();
                                        smr.material.mainTexture = CacheBuilder.Get(objectId);
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }
                    if (objectId.Equals("SmallerJuggus")) {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("b194c58ed09f1aa468e935b453c6843c",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                    if (nudn.genericRenderers[i].GetIl2CppType() ==
                                        Il2CppType.Of<SkinnedMeshRenderer>()) {
                                        var smr = nudn.genericRenderers[i].Cast<SkinnedMeshRenderer>();
                                        smr.material.shader = assets[0].Cast<Shader>();
                                        smr.material.mainTexture = CacheBuilder.Get(objectId);
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }
                    if (objectId.Equals("BlackHoleSun")) {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("8f3b1daf26cefc34cbf78aa45210317b",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                    if (nudn.genericRenderers[i].GetIl2CppType() ==
                                        Il2CppType.Of<SkinnedMeshRenderer>()) {
                                        var smr = nudn.genericRenderers[i].Cast<SkinnedMeshRenderer>();
                                        smr.material.shader = assets[0].Cast<Shader>();
                                        smr.material.SetColor("Outline Color", Color.black);
                                        smr.material.mainTexture = CacheBuilder.Get(objectId);
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }
                    if (objectId.Equals("BlackHoleSunProjectile")) {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("ae8cebf807b15984daf0219b66f42897",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                    if (nudn.genericRenderers[i].GetIl2CppType() == Il2CppType.Of<SpriteRenderer>()) {
                                        var smr = nudn.genericRenderers[i].Cast<SpriteRenderer>();
                                        smr.sprite = SpriteBuilder.createProjectile(CacheBuilder.Get(objectId));
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }
                    if (objectId.Equals("NinjaSexParty")) {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("9cd388b906451874abb35c8608c1d6ed",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                    if (nudn.genericRenderers[i].GetIl2CppType() ==
                                        Il2CppType.Of<SkinnedMeshRenderer>()) {
                                        var smr = nudn.genericRenderers[i].Cast<SkinnedMeshRenderer>();
                                        smr.material.shader = assets[0].Cast<Shader>();
                                        smr.material.mainTexture = CacheBuilder.Get(objectId);
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }
                    if (objectId.Equals("NinjaSexPartyProj")) {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("ae8cebf807b15984daf0219b66f42897",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                    if (nudn.genericRenderers[i].GetIl2CppType() == Il2CppType.Of<SpriteRenderer>()) {
                                        var smr = nudn.genericRenderers[i].Cast<SpriteRenderer>();
                                        smr.sprite = SpriteBuilder.createProjectile(CacheBuilder.Get(objectId));
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }
                    if (objectId.Equals("PONR")) {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("31a16eecf9211a64b8dcdfad2ff7974e",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                    if (nudn.genericRenderers[i].GetIl2CppType() ==
                                        Il2CppType.Of<SkinnedMeshRenderer>()) {
                                        var smr = nudn.genericRenderers[i].Cast<SkinnedMeshRenderer>();
                                        smr.material.shader = assets[0].Cast<Shader>();
                                        smr.material.mainTexture = CacheBuilder.Get(objectId);
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }
                    if (objectId.Equals("Underworld")) {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("8ccff862eab169c4884bac8bbd878529",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                    if (nudn.genericRenderers[i].GetIl2CppType() ==
                                        Il2CppType.Of<SkinnedMeshRenderer>()) {
                                        var smr = nudn.genericRenderers[i].Cast<SkinnedMeshRenderer>();
                                        smr.material.shader = assets[0].Cast<Shader>();
                                        smr.material.mainTexture = CacheBuilder.Get(objectId);
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }
                    if (objectId.Equals("UnderworldInverted")) {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("8ccff862eab169c4884bac8bbd878529",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                    if (nudn.genericRenderers[i].GetIl2CppType() ==
                                        Il2CppType.Of<SkinnedMeshRenderer>()) {
                                        var smr = nudn.genericRenderers[i].Cast<SkinnedMeshRenderer>();
                                        smr.material.shader = assets[0].Cast<Shader>();
                                        smr.material.mainTexture = CacheBuilder.Get(objectId);
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }
                    if (objectId.Equals("SkyHigh")) {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("f7a1b5c14ded01146b80bd7121f3fcd7",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                    if (nudn.genericRenderers[i].GetIl2CppType() ==
                                        Il2CppType.Of<SkinnedMeshRenderer>()) {
                                        var smr = nudn.genericRenderers[i].Cast<SkinnedMeshRenderer>();
                                        smr.material.shader = assets[0].Cast<Shader>();
                                        smr.material.mainTexture = CacheBuilder.Get(objectId);
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }
                    if (objectId.Equals("SkyHighProj")) {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("ae8cebf807b15984daf0219b66f42897",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                    if (nudn.genericRenderers[i].GetIl2CppType() == Il2CppType.Of<SpriteRenderer>()) {
                                        var smr = nudn.genericRenderers[i].Cast<SpriteRenderer>();
                                        smr.sprite = SpriteBuilder.createProjectile(CacheBuilder.Get(objectId));
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }
                    if (objectId.Equals("Survivor")) {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("1001186d3e8034b45929adb7ab6f048e",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                    if (nudn.genericRenderers[i].GetIl2CppType() ==
                                        Il2CppType.Of<SkinnedMeshRenderer>()) {
                                        var smr = nudn.genericRenderers[i].Cast<SkinnedMeshRenderer>();
                                        smr.material.shader = assets[0].Cast<Shader>();
                                        smr.material.mainTexture = CacheBuilder.Get(objectId);
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }
                    if (objectId.Equals("PONRProj")) {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("e5edd901992846e409326a506d272633",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                    if (nudn.genericRenderers[i].GetIl2CppType() ==
                                        Il2CppType.Of<MeshRenderer>()) {
                                        var smr = nudn.genericRenderers[i].Cast<MeshRenderer>();
                                        smr.material.shader = assets[0].Cast<Shader>();
                                        smr.material.mainTexture = CacheBuilder.Get(objectId);
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }
                    if (objectId.Equals("SmallerJuggusProj")) {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("c4b8e7aa3e07d764fb9c3c773ceec2ab",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                    if (nudn.genericRenderers[i].GetIl2CppType() == Il2CppType.Of<MeshRenderer>()) {
                                        var smr = nudn.genericRenderers[i].Cast<MeshRenderer>();
                                        smr.material.shader = assets[0].Cast<Shader>();
                                        smr.material.mainTexture = CacheBuilder.Get(objectId);
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
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

                    if (objectId.Equals("WhiteWeddingProjectile")) {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("ae8cebf807b15984daf0219b66f42897",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                    if (nudn.genericRenderers[i].GetIl2CppType() == Il2CppType.Of<SpriteRenderer>()) {
                                        var smr = nudn.genericRenderers[i].Cast<SpriteRenderer>();
                                        smr.sprite = SpriteBuilder.createProjectile(CacheBuilder.Get(objectId));
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }

                    if (objectId.Equals("WhiteWeddingOrbitProjectile")) {
                        UnityDisplayNode udn = null;
                        __instance.FindAndSetupPrototypeAsync("e23d594d3bf5af44c8b1e2445fe10a9e",
                            new System.Action<UnityDisplayNode>(oudn => {
                                var nudn = Object.Instantiate(oudn, __instance.PrototypeRoot);
                                nudn.name = objectId + "(Clone)";
                                nudn.isSprite = true;
                                nudn.RecalculateGenericRenderers();
                                for (var i = 0; i < nudn.genericRenderers.Length; i++) {
                                    if (nudn.genericRenderers[i].GetIl2CppType() == Il2CppType.Of<SpriteRenderer>()) {
                                        var smr = nudn.genericRenderers[i].Cast<SpriteRenderer>();
                                        smr.sprite = SpriteBuilder.createProjectile(CacheBuilder.Get(objectId));
                                        nudn.genericRenderers[i] = smr;
                                    }
                                }

                                udn = nudn;
                                onComplete.Invoke(udn);
                            }));
                        return false;
                    }
                }

                if (protos.ContainsKey(objectId)) {
                    onComplete.Invoke(protos[objectId]);
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(ResourceLoader), nameof(ResourceLoader.LoadSpriteFromSpriteReferenceAsync))]
        public record ResourceLoader_Patch
        {
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