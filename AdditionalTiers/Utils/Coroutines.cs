#pragma warning disable CS8632 // Stupid warning, dont speak

namespace AdditionalTiers.Utils {
    public sealed class Coroutines {
        private static List<ACoroutine> Active = new();
        private static readonly List<ACoroutine> ToRemove = new();

        public struct ACoroutine {
            public IEnumerator Enumerable;
            public System.Type? ValueTaskType;
            public dynamic ValueTask;
            public bool FinishedExecution;
            public bool HasValue;
            public readonly ulong ID = URandom.GetNextInt();

            public ACoroutine(IEnumerator enumerator, System.Type? valuetasktype) {
                (Enumerable, ValueTaskType, FinishedExecution) = (enumerator, valuetasktype, false);
                if (valuetasktype == null) {
                    HasValue = false;
                    ValueTask = Activator.CreateInstance(typeof(NoValueTask));
                } else {
                    HasValue = true;
                    var type = typeof(ValueTask<>).MakeGenericType(valuetasktype);
                    ValueTask = Activator.CreateInstance(type);
                }
            }
        }

        public static void AddCoroutine(ACoroutine ACoroutine) => Active.Add(ACoroutine);

        public static void StopCoroutine(StopToken stop) {
            var active = Active.ToArray();
            for (int i = 0; i < active.Length; i++) {
                if (active[i].ID == stop) {
                    if (stop.WaitUntilNextCycle)
                        ToRemove.Add(Active[i]);
                    else
                        active[i].FinishedExecution = true;

                    if (active[i].HasValue)
                        active[i].ValueTask.Exit = new ExitCode(stop, active[i].ValueTask.Value == null);
                    else
                        active[i].ValueTask.Exit = new ExitCode(stop);
                }
            }
            Active = new(active);
        }

        public static void UpdateCoroutines() {
            lock (Active) {
                for (int i = 0; i < Active.Count; i++) {
                    if (!Active[i].Enumerable.MoveNext())
                        Active.Remove(Active[i]);
                }
            }
        }

        // System.Void can't be used as a type :1984:
        public struct ValueTask<T> {
            public T Value { get; set; }
            public ExitCode Exit { get; set; }
        }
        public struct NoValueTask {
            public ExitCode Exit { get; set; }
        }

        public struct ExitCode {
            public bool? Successful { get; set; }
            public int Code { get; set; }
            public string Message { get; set; }
            public ExitCode(bool successful, int code, string message) => (Successful, Code, Message) = (successful, code, message);
            public ExitCode(StopToken stop, bool successful) => (Successful, Code, Message) = (successful, stop.Code, stop.Message);
            public ExitCode(StopToken stop) => (Successful, Code, Message) = (null, stop.Code, stop.Message);
        }

        public struct StopToken : IEquatable<ulong>, IComparable<ulong> {
            public ulong ID { get; set; }
            public bool WaitUntilNextCycle { get; set; }
            public int Code { get; set; }
            public string Message { get; set; }

            public int CompareTo(ulong other) => ID.CompareTo(other);
            public bool Equals(ulong other) => ID == other;
            public override bool Equals(object other) => ID.Equals(other);
            public override int GetHashCode() => ID.GetHashCode();
            public static bool operator ==(ulong other, StopToken self) => self.ID == other;
            public static bool operator !=(ulong other, StopToken self) => self.ID != other;
        }
    }
}
