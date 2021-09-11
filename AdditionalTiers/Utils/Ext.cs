namespace AdditionalTiers.Utils {
    public static class Ext {
        public static Il2CppReferenceArray<T> Remove<T>(this Il2CppReferenceArray<T> reference, Func<T, bool> predicate) where T : Model {
            var bases = new List<T>();
            foreach (var tmp in reference)
                if (!predicate(tmp))
                    bases.Add(tmp);

            return new(bases.ToArray());
        }
        public static Il2CppReferenceArray<T> Add<T>(this Il2CppReferenceArray<T> reference, params T[] newPart) where T : Model => ConcatArrayParams(reference, newPart);

        public static Il2CppReferenceArray<T> Add<T>(this Il2CppReferenceArray<T> reference, IEnumerable<T> enumerable) where T : Model => ConcatArrayEnumerable(reference, enumerable);

        public static Il2CppSystem.Collections.Generic.IEnumerable<TC> SelectI<T, TR, TC>(this IEnumerable<T> enumerable, Func<T, TR> predicate) where TR : Model where TC : Model {
            var bases = new Il2CppSystem.Collections.Generic.List<TC>();
            var enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
                bases.Add(predicate(enumerator.Current).Cast<TC>());
            return bases.Cast<Il2CppSystem.Collections.Generic.IEnumerable<TC>>();
        }

        //https://stackoverflow.com/a/33803440
        public static IntPtr GetPtr(this object o) {
            var d = new DynamicMethod("GetPtr", typeof(IntPtr), new System.Type[] { typeof(object) }, Assembly.GetExecutingAssembly().ManifestModule);
            var il = d.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ret);
            return (IntPtr)d.Invoke(null, new object[] { o });
        }

        public static bool Is<T>(this Il2CppSystem.Object obj, out T ex) where T : Il2CppObjectBase {
            if (obj.GetIl2CppType() == Il2CppType.Of<T>()) {
                ex = obj.Cast<T>();
                return true;
            }

            ex = default;
            return false;
        }

        public static Il2CppReferenceArray<T> ToIl2CppArray<T>(this T[] array) where T : Il2CppObjectBase => new Il2CppReferenceArray<T>(array);
        public static Il2CppSystem.Collections.Generic.List<T> ToIl2CppList<T>(this T[] array) where T : Il2CppObjectBase {
            Il2CppSystem.Collections.Generic.List<T> list = new();
            for (var i = 0; i < array.Length; i++)
                list.Add(array[i]);

            return list;
        }

        public static T CloneCast<T>(this Model obj) where T : Model => obj.Clone().Cast<T>();
        public static T CloneCast<T>(this T obj) where T : Model => obj.Clone().Cast<T>();

        private static T[] ConcatArray<T>(T[] a, T[] b) {
            var m = a.Length;
            var n = b.Length;
            var arr = new T[m + n];

            for (var i = 0; i < m + n; i++)
                if (i < m)
                    arr[i] = a[i];
                else
                    arr[i] = b[i - m];

            return arr;
        }

        private static T[] ConcatArrayParams<T>(T[] a, params T[] b) => ConcatArray(a, b);

        private static T[] ConcatArrayEnumerable<T>(T[] a, IEnumerable<T> e) => ConcatArray(a, e.ToArray());
    }
}