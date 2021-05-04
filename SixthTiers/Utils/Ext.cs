using System;
using System.Collections.Generic;
using Assets.Scripts.Models;
using UnhollowerBaseLib;

namespace SixthTiers.Utils {
    public static class Ext {
        public static Il2CppReferenceArray<T> Remove<T>(this Il2CppReferenceArray<T> reference, Func<T, bool> predicate) where T : Model
        {
            List<T> bases = new List<T>();
            foreach (var tmp in reference)
                if (!predicate(tmp))
                    bases.Add(tmp);

            return new (bases.ToArray());
        }
        public static Il2CppReferenceArray<T> Add<T>(this Il2CppReferenceArray<T> reference, params T[] newPart) where T : Model
        {
            var bases = new List<T>();
            foreach (var tmp in reference)
                bases.Add(tmp);

            foreach (var tmp in newPart)
                bases.Add(tmp);

            return new(bases.ToArray());
        }
        public static Il2CppReferenceArray<T> Add<T>(this Il2CppReferenceArray<T> reference, IEnumerable<T> enumerable) where T : Model
        {
            var bases = new List<T>();
            foreach (var tmp in reference)
                bases.Add(tmp);

            var enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext()) bases.Add(enumerator.Current);

            return new(bases.ToArray());
        }
    }
}