using System;
using System.Collections.Generic;
using MelonLoader;
using UnhollowerBaseLib;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace AdditionalTiers.Utils {
    public class AnimatedSpriteRenderer : MonoBehaviour {
        private int _totalTime;
        private Texture2D[] _frames = new Texture2D[0];

        private SpriteRenderer _renderer;

        public AnimatedSpriteRenderer(IntPtr obj0) : base(obj0) {
            ClassInjector.DerivedConstructorBody(this);
            System.Collections.Generic.List<Texture2D> texts = new();
            for (var index = 0; index < 64; index++) texts.Add(CacheBuilder.Get("energy" + index));
            _frames = texts.ToArray();
        }

        public AnimatedSpriteRenderer() : base(ClassInjector.DerivedConstructorPointer<AnimatedSpriteRenderer>()) {
        }

        private void Start() => _renderer = GetComponent<SpriteRenderer>();

        private void Update() {
            _totalTime++;
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            if (_renderer is null) { MelonLogger.Msg("Looking for the renderer :("); _renderer = GetComponent<SpriteRenderer>();}
            if (_frames.Length == 0) {MelonLogger.Msg($"No textures for {_frames.GetPtr()}"); return;}
            _renderer.sprite = SpriteBuilder.createProjectile(_frames[_totalTime%_frames.Length]);
        }

        [HideFromIl2Cpp]
        public void SetTextures(List<Texture2D> texture2Ds) {
            _frames = texture2Ds.ToArray();
            MelonLogger.Msg($"Set textures for {_frames.GetPtr()}");
            Update();
        }
    }
}