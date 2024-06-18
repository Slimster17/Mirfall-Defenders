using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSoundsPlayer : MonoBehaviour
{
    public void PlaySwordSlashSound() // Play sound when sword slash
    {
        SoundManager.PlaySound(SoundType.SwordSlash);
    }

    public void PlayDeathSound() // Play sound when unit die
    {
        SoundManager.PlaySound(SoundType.ManDeath);
    }

  
}
