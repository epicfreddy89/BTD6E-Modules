namespace AdditionalTiers.Utils {
    public class Timer {
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
            var index = 0;
            while (index < actions.Length) {
                actions[index](@object);
                yield return new WaitForEndOfFrame();
                index++;
            }
        }
        public static IEnumerator Run(Action[] actions) {
            var index = 0;
            while (index < actions.Length) {
                actions[index]();
                yield return new WaitForEndOfFrame();
                index++;
            }
        }
    }
}