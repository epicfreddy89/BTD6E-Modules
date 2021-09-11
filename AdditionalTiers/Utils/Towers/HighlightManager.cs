namespace AdditionalTiers.Utils.Towers {
    internal class HighlightManager {
        public static void Highlight(ref Tower __instance) {
            if (__instance?.Node?.graphic?.genericRenderers == null)
                return;

            for (int i = 0; i < __instance.Node.graphic.genericRenderers.Count; i++)
                __instance.Node.graphic.genericRenderers[i].material.SetColor("_OutlineColor", Color.white);

            if (__instance?.attackBehaviorsInDependants == null)
                return;

            for (int i = 0; i < __instance.attackBehaviorsInDependants.Count; i++) {
                try {
                    for (int i1 = 0; i1 < __instance.attackBehaviorsInDependants[i].entity.displayBehaviorCache.node.graphic.genericRenderers.Count; i1++)
                        __instance.attackBehaviorsInDependants[i].entity.displayBehaviorCache.node.graphic.genericRenderers[i1].material.SetColor("_OutlineColor", Color.white);
                } catch {
                    continue;
                }
            }
        }

        public static void UnHighlight(ref Tower __instance) {
            if (__instance?.Node?.graphic?.genericRenderers == null)
                return;

            for (int i = 0; i < __instance.Node.graphic.genericRenderers.Count; i++)
                __instance.Node.graphic.genericRenderers[i].material.SetColor("_OutlineColor", Color.black);

            if (__instance?.attackBehaviorsInDependants == null)
                return;

            for (int i = 0; i < __instance.attackBehaviorsInDependants.Count; i++) {
                try {
                    for (int i1 = 0; i1 < __instance.attackBehaviorsInDependants[i].entity.displayBehaviorCache.node.graphic.genericRenderers.Count; i1++)
                        __instance.attackBehaviorsInDependants[i].entity.displayBehaviorCache.node.graphic.genericRenderers[i1].material.SetColor("_OutlineColor", Color.black);
                } catch {
                    continue;
                }
            }
        }
    }
}