using System.Collections.Generic;

namespace SixthTiers.Utils {
    public class AssetStack<T> : Stack<T> {
        public void PushAll(params T[] t) {
            for (var i = 0; i < t.Length; i++) Push(t[i]);
        }
    }
}