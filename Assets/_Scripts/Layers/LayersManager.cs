using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayersManager 
{
    // Method to get the LayerMask for a given ProjectLayer
    public static LayerMask GetLayerMask(ProjectLayers layer) 
    {
        return 1 << (int)layer;
    }
    // Method to check if a LayerMask contains a specific ProjectLayer
    public static bool HasLayer(LayerMask mask, ProjectLayers layer)
    {
        LayerMask layerMask = GetLayerMask(layer);
        return (mask & layerMask) == layerMask;
    }
    // Overloaded method to check if a ProjectLayer contains another specific ProjectLayer
    public static bool HasLayer(ProjectLayers maskLayer, ProjectLayers layer)
    {
        LayerMask mask = GetLayerMask(maskLayer);
        LayerMask layerMask = GetLayerMask(layer);
        return (mask & layerMask) == layerMask;
    }
    // Method to get the layer index of a GameObject
    public static int GetLayerIndex(GameObject obj)
    {
        return obj.layer;
    }
}
