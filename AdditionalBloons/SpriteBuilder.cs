using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdditionalBloons.Utils {
    public class SpriteBuilder {
        private static readonly Dictionary<Texture2D, Sprite> cache = new ();

        public static Sprite createBloon(Texture2D tx) {
            if (!cache.ContainsKey(tx))
            cache.Add(tx, Sprite.Create(tx, new(0,0,tx.width,tx.height), new (0.5f,0.5f), 7.6f, 0, SpriteMeshType.Tight));
            return cache[tx];
        }
    }
}