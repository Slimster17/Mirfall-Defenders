using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSoundsPlayer : MonoBehaviour
{
    public void PlaySwordSlashSound()
    {
        SoundManager.PlaySound(SoundType.SwordSlash);
    }

    public void PlayDeathSound()
    {
        SoundManager.PlaySound(SoundType.ManDeath);
    }

  
}
