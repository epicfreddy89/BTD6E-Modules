namespace AdditionalTiers.Utils {
    public sealed class Timer {
        public static IEnumerator Countdown(int seconds, Action after, Action<int> during) {
            var counter = seconds;
            while (counter > 0) {
                during(counter);
                yield return new WaitForSeconds(1);
                counter--;
            }
            after();
        }
        public static IEnumerator Countdown(float time, float decrement, float gap, Action after, Action<float> during) {
            var counter = time;
            while (counter > 0) {
                during(counter);
                yield return new WaitForSeconds(gap);
                counter -= decrement;
            }
            after();
        }
        public static IEnumerator Run<T>(Action<T>[] actions, T @object) {
            foreach (var action in actions) {
                action(@object);
                yield return new WaitForEndOfFrame();
            }
        }
        public static IEnumerator Run(Action[] actions) {
            foreach (var action in actions) {
                action();
                yield return new WaitForEndOfFrame();
            }
        }
        public static IEnumerator BuildAssetList() {
            while (AdditionalTiers.Towers == null)
                yield return new WaitForSeconds(2);

            for (var i = 0; i < AdditionalTiers.Towers.Length; i++) {
                var assets = AdditionalTiers.Towers[i].assetsToRead;
                if (assets != null) {
                    foreach (var asset in assets) {
                        if (!DisplayFactory.allAssetsKnown.Contains(asset))
                            DisplayFactory.allAssetsKnown.Add(asset);
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}