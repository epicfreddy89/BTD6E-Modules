using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace AdditionalTiers.Utils {
    internal static class AnimatedTextureManager {
        public static List<TextureInfo> textures = new();

        public static void OnUpdate() {
            /*List<TextureInfo> toRemove = new();
            for (var i = 0; i < textures.Count; i++)
                unsafe {
                    var val = Marshal.PtrToStructure(textures[i].Renderer, typeof(SpriteRenderer));
                    if (val == null) {
                        toRemove.Add(textures[i]);
                        continue;
                    }

                    val.sprite = SpriteBuilder.createProjectile(textures[i].Textures[textures[i].tick % textures[i].Textures.Length]);
                    textures[i].tick++;
                }

            foreach (var remove in toRemove) textures.Remove(remove);*/
        }

        public class TextureInfo {
            public object Renderer { get; }
            public Texture2D[] Textures { get; }
            public int tick;
            
            public TextureInfo(object renderer, Texture2D[] textures) {
                Renderer = renderer;
                Textures = textures;
            }
        }
    }
}