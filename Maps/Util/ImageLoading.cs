
using System;
using System.Collections.Generic;
using System.IO;
using HarmonyLib;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace BTD6_Expansion.Utilities
{
    public class ImageLoading : MonoBehaviour
    {
        static Exception nullTexture2dException = new NullReferenceException("Texture2D texture can't be null!");
        static Exception spriteCantBeCreatedException = new("Sprite can't be created!");
        public static ImageLoading instance = new();
        private static readonly Dictionary<String, Sprite> cachedSprites = new();

        public Image GetActiveImage()
        {
            return GetComponent<Image>().MemberwiseClone().Cast<Image>();
        }

        public static void SaveToPNG(Sprite s, String name)
        {
            byte[] b = ImageConversion.EncodeToPNG(s.texture);
            byte[] bytes = new byte[b.Length];

            for (int i = 0; i < b.Length; i++)
            {
                bytes[i] = b[i];
            }

            File.Create(name).Write(bytes, 0, bytes.Length);
        }

        public static Sprite LoadSpriteFromPNG(String identifier, byte[] encoded, float PixelsPerUnit = 100.0f,
            SpriteMeshType spriteType = SpriteMeshType.Tight, uint extrude = 0, Vector2 pivot = default)
        {
            if (cachedSprites.ContainsKey(identifier))
                return cachedSprites[identifier];

            try
            {
                var SpriteTexture = LoadTextureFromBytes(encoded);
                var s = Sprite.Create(SpriteTexture, new(0, 0, SpriteTexture.width, SpriteTexture.height), pivot, PixelsPerUnit, extrude, spriteType);
                return s;
            }
            catch (Exception)
            {
                throw spriteCantBeCreatedException;
            }
        }

        public static Sprite LoadSpriteB64(string encoded, float PixelsPerUnit = 100.0f,
            SpriteMeshType spriteType = SpriteMeshType.Tight)
        {
            try
            {
                Texture2D SpriteTexture = LoadTextureFromBytes(Convert.FromBase64String(encoded));
                Sprite NewSprite = Sprite.Create(SpriteTexture,
                    new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit, 0,
                    spriteType);
                return NewSprite;
            }
            catch (Exception)
            {
                throw spriteCantBeCreatedException;
            }
        }

        public static Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight)
        {
            try
            {
                Texture2D SpriteTexture = LoadTexture(FilePath);
                Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);
                return NewSprite;
            }
            catch (Exception)
            {
                throw spriteCantBeCreatedException;
            }
        }

        public static Sprite ConvertTextureToSprite(Texture2D texture, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight)
        {
            if (texture != null)
            {
                Sprite NewSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0, 0), PixelsPerUnit, 0, spriteType);

                return NewSprite;
            }
            else
            {
                throw nullTexture2dException;
            }
        }

        public static Texture2D LoadTexture(string FilePath)
        {

            Texture2D Tex2D;
            byte[] FileData;

            if (File.Exists(FilePath))
            {
                FileData = File.ReadAllBytes(FilePath);
                Tex2D = new Texture2D(2, 2);

                if (ImageConversion.LoadImage(Tex2D, FileData))
                    return Tex2D;
            }
            return null;
        }

        public static Texture2D LoadTextureFromBytes(byte[] FileData)
        {

            Texture2D Tex2D = new Texture2D(2, 2);

            if (ImageConversion.LoadImage(Tex2D, FileData)) return Tex2D;

            return null;
        }
    }
}
