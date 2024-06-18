using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedTile : MonoBehaviour
{
   private Tile _selected; // The currently selected tile
  
   // Property to get/set the selected tile
   public Tile Selected { get => _selected; set => _selected = value; } 

   private void Update()
   {
      // If no tile is selected, exit the method
      if(_selected == null) return;
      
      // Debug.Log($"Selected Tile is {_selected.Coordinates}");
   }
}
