using Assets.Scripts.Utils;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace AdditionalTiers.Utils.Components {
    [RegisterTypeInIl2Cpp]
    public class SetHeight1 : MonoBehaviour {
        public SetHeight1(IntPtr obj0) : base(obj0) { ClassInjector.DerivedConstructorBody(this); }

        public SetHeight1() : base(ClassInjector.DerivedConstructorPointer<SetHeight1>()) { }

        private void Update() {
            if (gameObject != null) {
                if (gameObject.transform.position.z < 1) {
                    var pos = gameObject.transform.position;
                    pos.z = 8;
                    gameObject.transform.position = pos;
                }
            }
        }
    }
}
