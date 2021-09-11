

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
    }
}