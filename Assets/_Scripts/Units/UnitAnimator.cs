using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private Animator _animator; // Reference to the Animator component

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void PlayWalkingAnimation()
    {
        _animator.SetBool("Walking", true);
        _animator.SetBool("Attacking", false);
    }

    public void PlayAttackingAnimation()
    {
        _animator.SetBool("Walking", false);
        _animator.SetBool("Attacking", true);
    }

    public void PlayDeathAnimation()
    {
        _animator.SetTrigger("Dying");
    }

    public void StopAnimations()
    {
        _animator.SetBool("Walking", false);
        _animator.SetBool("Attacking", false);
        _animator.SetBool("Dying",false);
    }
}
