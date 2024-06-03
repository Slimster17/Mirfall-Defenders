using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedTile : MonoBehaviour
{
   private Tile _selected;
   public Tile Selected { get => _selected; set => _selected = value; }

   private void Update()
   {
      if(_selected == null) return;
      
      // Debug.Log($"Selected Tile is {_selected.Coordinates}");
   }
}
