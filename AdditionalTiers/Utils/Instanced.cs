namespace AdditionalTiers.Utils {
    public class Instanced<T> {
        public static T VALUE { get; }

        static Instanced() => VALUE = Activator.CreateInstance<T>();
    }
}