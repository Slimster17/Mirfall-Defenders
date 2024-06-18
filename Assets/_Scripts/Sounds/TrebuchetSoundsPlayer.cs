using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrebuchetSoundsPlayer : MonoBehaviour
{
   public void PlayRotationSound() // Play the sound of the trebuchet rotating
   {
      SoundManager.PlaySound(SoundType.TrebuchetRotation);
   }

   public void PlayFireSound() // Play the sound of the trebuchet firing
   {
      SoundManager.PlaySound(SoundType.TrebuchetFire);
   }
}
