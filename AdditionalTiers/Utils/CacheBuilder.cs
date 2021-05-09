using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.ServiceModel.Channels;
using MelonLoader;
using AdditionalTiers.Resources;
using Newtonsoft.Json;
using UnityEngine;

namespace AdditionalTiers.Utils {
    public class CacheBuilder {
        private static readonly Dictionary<string, string> built = new();
        private static readonly Dictionary<string, byte[]> builtBytes = new();

        public static void Build() {
            ResourceSet resourceSet = Images.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry v in resourceSet) {
                if (v.Key is string id && v.Value is Byte[] bytes) {
                    built.Add(id, Convert.ToBase64String(bytes));
                }
            }
        }

        public static Texture2D Get(string key) {
            if (builtBytes.ContainsKey(key)) {
                var text = LoadTextureFromBytes(builtBytes[key]);
                return text;
            }

            var bytes = Convert.FromBase64String(built[key]);
            builtBytes.Add(key, bytes);
            var textNew = LoadTextureFromBytes(Convert.FromBase64String(built[key]));
            return textNew;
        }

        public static void Flush() => built.Clear();

        private static Texture2D LoadTextureFromBytes(byte[] FileData) {
            Texture2D Tex2D = new Texture2D(2, 2);
            if (ImageConversion.LoadImage(Tex2D, FileData)) return Tex2D;

            return null;
        }
    }
}