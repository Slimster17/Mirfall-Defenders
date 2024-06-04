using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerArcherSoundsPlayer : MonoBehaviour
{
   public void PlayShootingSound()
   {
      SoundManager.PlaySound(SoundType.ArrowShot);
   }
}
