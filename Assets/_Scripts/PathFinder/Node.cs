using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Node
{
   public Vector2Int coordinates; // Coordinates of the node in the grid
   public bool isWalkable;  // Indicates whether the node is walkable
   public bool isExplored; // Indicates whether the node has been explored
   public bool isPath; // Indicates whether the node is part of a path
   public Node connectedTo; // Reference to the connected node
   public ProjectLayers requiredLayer; // Required layer for the node

   // Constructor to initialize a node with coordinates and walkable status
   public Node(Vector2Int coordinates, bool isWalkable)
   {
      this.coordinates = coordinates;
      this.isWalkable = isWalkable;
   }
}
