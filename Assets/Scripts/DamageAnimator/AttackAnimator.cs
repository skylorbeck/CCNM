using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimator : MonoBehaviour
{
    private Animator animator;
    public TrailRenderer trail { get; private set; }
    [field:SerializeField]public bool ended { get; private set; } = false;
    public void End()
    {
        ended = true;
    }

    public void Play(AttackType attackType, float scale)
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            trail = GetComponentInChildren<TrailRenderer>();
        }
        trail.Clear();
        trail.widthMultiplier = scale;
        ended = false;
        switch (attackType)
        {
            case AttackType.Slash:
                animator.SetTrigger("slash");
                break;
            case AttackType.Headbutt:
                animator.SetTrigger("headbutt");
                break;
        }
    }

    public enum AttackType
    {
        Slash,
        Headbutt,
        None
    }
}
