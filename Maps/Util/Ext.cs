using System;
using System.Collections.Generic;

using UnhollowerBaseLib;

namespace SixthTiers.Utils {
    public static class Ext {
        public static Il2CppReferenceArray<T> Remove<T>(this Il2CppReferenceArray<T> reference, Func<T, bool> predicate) where T : Il2CppObjectBase {
            List<T> bases = new List<T>();
            foreach (var tmp in reference)
                if (!predicate(tmp))
                    bases.Add(tmp);

            return new(bases.ToArray());
        }
        public static Il2CppReferenceArray<T> Add<T>(this Il2CppReferenceArray<T> reference, params T[] newPart) where T : Il2CppObjectBase {
            var bases = new List<T>();
            foreach (var tmp in reference)
                bases.Add(tmp);

            foreach (var tmp in newPart)
                bases.Add(tmp);

            return new(bases.ToArray());
        }
        public static Il2CppReferenceArray<T> AddAll<T>(this Il2CppReferenceArray<T> reference, params T[] newPart) where T : Il2CppObjectBase => Add(reference, newPart);

        public static T[] Reversed<T>(this T[] reference) {
            var bases = new List<T>();

            for (int i = reference.Length - 1; i >= 0; i--)
                bases.Add(reference[i]);

            return bases.ToArray();
        }
    }
}