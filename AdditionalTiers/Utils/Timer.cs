using System;
using System.Collections;
using UnityEngine;

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
    }
}