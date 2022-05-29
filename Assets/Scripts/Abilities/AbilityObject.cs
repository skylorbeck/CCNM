using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Combat/Ability")]
public class AbilityObject : ScriptableObject
{
    
    [field: SerializeField] public string title  { get; private set; } = "ability.title";
    [field: SerializeField] public string descriptionA  { get; private set; } = "ability.description.a";
    [field: SerializeField] public string descriptionB  { get; private set; } = "ability.description.b";
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public int baseCost { get; private set; } = 1;

    [field: Header("Damage")]
    [field: SerializeField] public int targetDamage { get; private set; } = 0;
    public bool damageTarget => targetDamage > 0;
    [field: SerializeField] public int userDamage { get; private set; } = 0;
    public bool damageUser => userDamage > 0;
    
    [field: Header("Healing")]
    [field: SerializeField] public int targetHeal { get; private set; } = 0;
    public bool healTarget => targetHeal > 0;
    [field: SerializeField] public int userHeal { get; private set; } = 0;
    public bool healUser => userHeal > 0;

    [field: Header("Shielding")]
    [field: SerializeField] public int targetShield { get; private set; } = 0;
    public bool shieldTarget => targetShield > 0;
    [field: SerializeField] public int userShield { get; private set; } = 0;
    public bool shieldUser => userShield > 0;
    
    [field: Header("Status Effects")]
    [field: SerializeField] public StatusEffect targetStatus { get; private set; }

    [field: SerializeField] public bool statusTarget { get; private set; } = false;
    [field: SerializeField] public StatusEffect userStatus { get; private set; }
    [field: SerializeField] public bool statusSelf { get; private set; } = false;
    
    [field: Header("Other")]
    [field: SerializeField] public StatusEffect.Element element{ get; private set; } = StatusEffect.Element.None;

    [field: SerializeField] public AttackAnimator.AttackType attackAnimation { get; private set; } = AttackAnimator.AttackType.None;

}