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
    public static bool HasLayer(ProjectLayers maskLayer, ProjectLayers layer)
    {
        LayerMask mask = GetLayerMask(maskLayer);
        LayerMask layerMask = GetLayerMask(layer);
        return (mask & layerMask) == layerMask;
    }
    
    public static int GetLayerIndex(GameObject obj)
    {
        return obj.layer;
    }
}
