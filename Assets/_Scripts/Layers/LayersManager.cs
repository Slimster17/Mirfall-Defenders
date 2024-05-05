using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayersManager 
{
    public static LayerMask GetLayerMask(ProjectLayers layer)
    {
        return 1 << (int)layer;
    }

    public static bool HasLayer(LayerMask mask, ProjectLayers layer)
    {
        LayerMask layerMask = GetLayerMask(layer);
        return (mask & layerMask) == layerMask;
    }
}
