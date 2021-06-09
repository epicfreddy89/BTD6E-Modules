using System.Collections.Generic;
using UnityEngine;

namespace AdditionalTiers.Utils.Assets {
    public class SpriteBuilder {
        private static readonly Dictionary<Texture2D, Sprite> cache = new ();
        private static int _id;

        public static int ID { get { _id += 1; return _id; } }

        public static Sprite createProjectile(Texture2D tx, float pixelsPerUnit = 5.4f, float pivotx = 0.5f, float pivoty = 0.5f) {
            if (!cache.ContainsKey(tx)) {
                var sp = Sprite.Create(tx, new(0, 0, tx.width, tx.height), new(0.5f, 0.5f), pixelsPerUnit, 0, SpriteMeshType.Tight);
                sp.name = $"id {ID}";
                cache.Add(tx, sp);
            }

            return cache[tx];
        }
    }
}