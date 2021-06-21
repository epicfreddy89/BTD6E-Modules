using Assets.Scripts.Simulation.Towers;
using UnityEngine;

namespace AdditionalTiers.Utils.Towers {
    internal class HighlightManager {
        public static void Highlight(ref Tower __instance) {
            if (__instance?.Node?.graphic?.genericRenderers == null)
                return;

            foreach (var graphicGenericRenderer in __instance.Node.graphic.genericRenderers)
                graphicGenericRenderer.material.SetColor("_OutlineColor", Color.white);
        }

        public static void UnHighlight(ref Tower __instance) {
            if (__instance?.Node?.graphic?.genericRenderers == null)
                return;

            foreach (var graphicGenericRenderer in __instance.Node.graphic.genericRenderers)
                graphicGenericRenderer.material.SetColor("_OutlineColor", Color.black);
        }
    }
}