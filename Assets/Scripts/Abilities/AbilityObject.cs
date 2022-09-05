using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Combat/Ability")]
[Serializable]
public class AbilityObject : ScriptableObject
{

    [field: SerializeField] public string title { get; private set; } = "ability.title";
    [field: SerializeField] public string descriptionA { get; private set; } = "ability.description.a";//todo GetDescription() where the damage is calculated based on your stats before returning
    [field: SerializeField] public string descriptionB { get; private set; } = "ability.description.b";
    [field: SerializeField] public Sprite icon { get; private set; }
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
    
    
    public string GetTranslatedDescriptionA(PlayerShell playerShell)
    {
        string description = descriptionA;
        if (damageTarget)
        {
            int baseDamage = playerShell.brain.GetDamage();
            description = description.Replace("{damage}", (baseDamage * targetDamageMultiplier).ToString());
        }

        if (healTarget)
        {
            int baseHeal = playerShell.brain.GetHeal();
            description = description.Replace("{heal}", (baseHeal * targetHealMultiplier).ToString());
        }

        /*if (armorTarget)
        {
            int baseArmor = playerShell.brain.GetShieldRate();
            description = description.Replace("{armor}", (baseArmor * targetArmorMultiplier).ToString());
        }*/

        if (statusTarget)
        { 
            description = description.Replace("{status}", targetStatus.title);
            description = description.Replace("{damage}", playerShell.brain.GetStatusDamage().ToString());
            description = description.Replace("{heal}", (playerShell.brain.GetHeal()).ToString());

        }
        
        return description;
    }
    public string GetTranslatedDescriptionB(PlayerShell playerShell)
    {
        string description = descriptionB;
        if (damageUser)
        {
            int baseDamage = playerShell.brain.GetDamage();
            description = description.Replace("{damage}", (baseDamage * userDamageMultiplier).ToString());
        }
        
        if (healUser)
        {
            int baseHeal = playerShell.brain.GetHeal();
            description = description.Replace("{heal}", (baseHeal * userHealMultiplier).ToString());
        }
        
        /*if (armorUser)
        {
            int baseArmor = playerShell.brain.GetShieldRate();
            description = description.Replace("{armor}", (baseArmor * userArmorMultiplier).ToString());
        }*/
        
        if (statusSelf)
        {
            description = description.Replace("{status}", userStatus.title);
            description = description.Replace("{damage}", playerShell.brain.GetStatusDamage().ToString());
            description = description.Replace("{heal}", (playerShell.brain.GetHeal()).ToString());

        }

        return description;
    }
    
}
[Serializable]
public class AbilityGem
{
    [field: SerializeField] public int abilityIndex { get; private set; }
    [field: SerializeField] public int gemLevel { get; private set; }
    [field: SerializeField] public int amountOwned { get; private set; }

    public AbilityGem(AbilityGem oldGem)
    {
            this.abilityIndex = oldGem.abilityIndex;
            this.gemLevel = oldGem.gemLevel;
            this.amountOwned = oldGem.amountOwned;
    }
    public AbilityGem(int abilityIndex, int gemLevel)
    {
        this.abilityIndex = abilityIndex;
        this.gemLevel = gemLevel;
        this.amountOwned = 0;   
    }
    public AbilityGem(int abilityIndex)
    {
        this.abilityIndex = abilityIndex;
        this.gemLevel = 0;
        this.amountOwned = 0;   
    }
    
    public AbilityGem(AbilityObject ability, int gemLevel)
    {
        this.abilityIndex = GameManager.Instance.abilityRegistry.GetAbilityIndex(ability.title);
        this.gemLevel = gemLevel;
        this.amountOwned = 0;
    }
    public void SetGem(int abilityIndex, int gemLevel)
    {
        this.abilityIndex = abilityIndex;
        this.gemLevel = gemLevel;
    }
    
    public void SetLevel(int gemLevel)
    {
        this.gemLevel = gemLevel;
    }
    
    public void SetAmountOwned(int amountOwned)
    {
        this.amountOwned = amountOwned;
    }
    
    public void AddAmountOwned(int amount)
    {
        this.amountOwned += amount;
    }
    
    public AbilityObject GetAbility()
    {
        if (abilityIndex == -1)
            return null;
        return GameManager.Instance.abilityRegistry.GetAbility(abilityIndex);
    }
    
}