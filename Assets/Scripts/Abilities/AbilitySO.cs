using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Combat/Ability")]
public class AbilitySO : ScriptableObject
{
    
    [field: SerializeField] public string title  { get; private set; } = "ability.title";
    [field: SerializeField] public string description  { get; private set; } = "ability.description";
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public int baseCost { get; private set; } = 1;

    [field: Header("Damage")]
    [field: SerializeField] public bool damageTarget{ get; private set; } = false;
    [field: SerializeField] public bool damageUser{ get; private set; } = false;
    [field: SerializeField] public int baseDamage { get; private set; } = 3;
    [field: Header("Healing")]
    [field: SerializeField] public bool healTarget{ get; private set; } = false;
    [field: SerializeField] public bool healUser{ get; private set; } = false;
    [field: SerializeField] public int baseHeal { get; private set; } = 3;
    [field: Header("Shielding")]
    [field: SerializeField] public bool shieldTarget{ get; private set; } = false;
    [field: SerializeField] public bool shieldUser{ get; private set; } = false;
    [field: SerializeField] public int baseShield { get; private set; } = 3;
    [field: Header("Status Effects")]
    [field: SerializeField] public bool statusTarget{ get; private set; } = false;
    [field: SerializeField] public bool statusSelf{ get; private set; } = false;
    [field: SerializeField] public StatusEffect[] statusEffects{ get; private set; } = new StatusEffect[0];
    [field: Header("Other")]
    [field: SerializeField] public StatusEffect.Element element{ get; private set; } = StatusEffect.Element.None;
    
}