namespace AdditionalTiers.Utils.Assets {
    [StructLayout(LayoutKind.Sequential)]
    public sealed class AssetStack : IDisposable {
        AssetInfo[] assetInfos = new AssetInfo[0];

        public IEnumerator<AssetInfo> GetEnumerator() {
            for (int i = 0; i < assetInfos.Length; i++)
                yield return assetInfos[i];
        }

        public void Dispose() => assetInfos = null; // Cry

        public void Add(AssetInfo asset) {
            var aInfo = new AssetInfo[assetInfos.Length + 1];
            for (int i = 0; i < aInfo.Length; i++)
                if (i < assetInfos.Length)
                    aInfo[i] = assetInfos[i];
                else
                    aInfo[i] = asset;
            assetInfos = aInfo;
        }
    }
}