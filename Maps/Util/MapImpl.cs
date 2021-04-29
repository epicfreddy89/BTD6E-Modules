using Assets.Scripts.Data.MapSets;

namespace Maps.Util {
    public interface MapImpl {
        void Create(out MapDetails ret);
        MapDetails GetCreated();
        void Destroy();
    }
}