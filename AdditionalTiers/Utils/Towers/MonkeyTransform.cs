namespace AdditionalTiers.Utils.Towers {
    public static class MonkeyTransform {
        public static void TAdd(this TowerToSimulation tts, bool allSame = true, float scale1 = 0, float scale2 = 0, float scale3 = 0) {
            var curScale = tts.tower.display.scaleOffset;
            if (allSame)
                tts.tower.display.SetScaleOffset(new(curScale.x + scale1, curScale.y + scale1, curScale.z + scale1));
            else
                tts.tower.display.SetScaleOffset(new(curScale.x + scale1, curScale.y + scale2, curScale.z + scale3));
        }
        public static void TSub(this TowerToSimulation tts, bool allSame = true, float scale1 = 0, float scale2 = 0, float scale3 = 0) {
            var curScale = tts.tower.display.scaleOffset;
            if (allSame)
                tts.tower.display.SetScaleOffset(new(curScale.x - scale1, curScale.y - scale1, curScale.z - scale1));
            else
                tts.tower.display.SetScaleOffset(new(curScale.x - scale1, curScale.y - scale2, curScale.z - scale3));
        }
    }
}