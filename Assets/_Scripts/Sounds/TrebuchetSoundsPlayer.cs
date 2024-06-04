using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrebuchetSoundsPlayer : MonoBehaviour
{
   public void PlayRotationSound()
   {
      SoundManager.PlaySound(SoundType.TrebuchetRotation);
   }

   public void PlayFireSound()
   {
      SoundManager.PlaySound(SoundType.TrebuchetFire);
   }
}
