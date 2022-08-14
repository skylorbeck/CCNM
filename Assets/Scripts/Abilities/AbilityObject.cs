using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Combat/Ability")]
[Serializable]
public class AbilityObject : ScriptableObject
{

    [field: SerializeField] public string title { get; private set; } = "ability.title";
    [field: SerializeField] public string descriptionA { get; private set; } = "ability.description.a";
    [field: SerializeField] public string descriptionB { get; private set; } = "ability.description.b";
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public Sprite gemIcon { get; private set; }
    [field: SerializeField] public int baseCost { get; private set; } = 1;

    [field: Header("Damage")]
    [field: SerializeField] public bool damageTarget { get; private set; } = false;
    [field: SerializeField] public float targetDamageMultiplier { get; private set; } = 1;
    [field: SerializeField] public bool damageUser { get; private set; } = false;
    [field: SerializeField] public float userDamageMultiplier { get; private set; } = 1;


    [field: Header("Healing")]
    [field: SerializeField] public bool healTarget { get; private set; } = false;
    [field: SerializeField] public float targetHealMultiplier { get; private set; } = 1;

    [field: SerializeField] public bool healUser { get; private set; } = false;
    [field: SerializeField] public float userHealMultiplier { get; private set; } = 1;


    [field: Header("Armor (Unused)")]
    [field: SerializeField] public bool armorTarget { get; private set; } = false;
    [field: SerializeField] public float targetArmorMultiplier { get; private set; } = 1;
    [field: SerializeField] public bool armorUser { get; private set; } = false;
    [field: SerializeField] public float userArmorMultiplier { get; private set; } = 1;



    [field: Header("Status Effects")]
    [field: SerializeField] public StatusEffect targetStatus { get; private set; }

    [field: SerializeField] public bool statusTarget { get; private set; } = false;
    [field: SerializeField] public StatusEffect userStatus { get; private set; }
    [field: SerializeField] public bool statusSelf { get; private set; } = false;

    [field: Header("Other")]
    [field: SerializeField]
    public EquipmentDataContainer.SlotType slotType { get; private set; } = EquipmentDataContainer.SlotType.Offense;
    [field: SerializeField] public StatusEffect.Element element { get; private set; } = StatusEffect.Element.None;

    [field: SerializeField] public AttackAnimator.AttackType attackAnimation { get; private set; } = AttackAnimator.AttackType.None;

    [field: SerializeField] public AudioClip soundEffect { get; private set; }
}
[Serializable]
public class AbilityGem
{
    [field: SerializeField] public int abilityIndex { get; private set; }
    [field: SerializeField] public int gemLevel { get; private set; }
    public AbilityGem(int abilityIndex, int gemLevel)
    {
        this.abilityIndex = abilityIndex;
        this.gemLevel = gemLevel;
    }
    
    public AbilityGem(AbilityObject ability, int gemLevel)
    {
        this.abilityIndex = GameManager.Instance.abilityRegistry.GetAbilityIndex(ability.title);
        this.gemLevel = gemLevel;
    }
    public void SetGem(int abilityIndex, int gemLevel)
    {
        this.abilityIndex = abilityIndex;
        this.gemLevel = gemLevel;
    }
    
    public AbilityObject GetAbility()
    {
        return GameManager.Instance.abilityRegistry.GetAbility(abilityIndex);
    }
}