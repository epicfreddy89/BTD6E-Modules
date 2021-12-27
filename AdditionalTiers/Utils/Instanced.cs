namespace AdditionalTiers.Utils {
    public class Instanced<T> {
        public static T VALUE { get; protected set; }

        static Instanced() => VALUE = Activator.CreateInstance<T>();
    }
}