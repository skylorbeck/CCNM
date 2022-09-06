using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Ability", menuName = "Combat/Ability/Base")]
[Serializable]
public class AbilityObject : ScriptableObject
{

    [field: SerializeField] public string title { get; private set; } = "title";
    [field: SerializeField] public string descriptionA { get; private set; } = "description.a";
    [field: SerializeField] public string descriptionB { get; private set; } = "description.b";
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

    public virtual void Execute(Shell user, Shell target)
    {
          /*if (armorTarget)
        {
            int shield = await target.OnShield(target,user.brain.baseShield);
            shield = (int)Math.Ceiling(shield * targetArmorMultiplier);
            target.Shield(shield, element);
        }
        if (armorUser)
        {
            int shield = await user.OnShield(user,user.brain.baseShield);
            shield = (int)Math.Ceiling(shield * userArmorMultiplier);
            user.Shield(shield, element);
        }*/
        if (statusTarget)
        {
            target.AddStatusEffect(targetStatus);
        }
        if (statusSelf)
        {
            user.AddStatusEffect(userStatus);
        }
        if (healTarget)
        {
            int heal = target.OnHeal(target,(int)(user.brain.GetHealthMax()*0.25f));
            heal = (int)Math.Ceiling(heal * targetHealMultiplier);
            target.Heal(heal, element);
        }
        if (healUser)
        {
            int heal = user.OnHeal(user,(int)(user.brain.GetHealthMax()*0.25f));
            heal = (int)Math.Ceiling(heal * userHealMultiplier);
            user.Heal(heal, element);
        }
        if (damageTarget)
        {
            int damage = user.OnAttack(target,user.brain.GetDamage());//process status influences
            damage = (int)(damage * targetDamageMultiplier);//process ability multiplier
            if (Random.Range(0,100)<user.brain.GetCritChance())
            {
               damage +=  (int)(damage * user.brain.GetCritDamage());
               //todo crit notification
            }
            DamageAnimator.Instance.TriggerAttack(target, attackAnimation);//play the animation
            target.Damage(user, damage, element);//damage the target
            // target.TestDeath(); handled in target.damage
        }
        if (damageUser)
        {
            int damage = user.OnAttack(user,user.brain.GetDamage());
            damage = (int)(damage * userDamageMultiplier);
            if (Random.Range(0,100)<user.brain.GetCritChance())
            {
                damage +=  (int)(damage * user.brain.GetCritDamage());
                //todo crit notification
            }
            DamageAnimator.Instance.TriggerAttack(user, attackAnimation);
            user.Damage(user,damage,element);
            // user.TestDeath();
        }
    }
    public string GetTranslatedDescriptionA(PlayerShell playerShell)
    {
        string description = descriptionA;
            int baseDamage = playerShell.brain.GetDamage();
            description = description.Replace("{damage}", (baseDamage * targetDamageMultiplier).ToString());
            int baseHeal = playerShell.brain.GetHeal();
            description = description.Replace("{heal}", (baseHeal * targetHealMultiplier).ToString());
            int baseArmor = playerShell.brain.GetShieldRate();
            description = description.Replace("{armor}", (baseArmor * targetArmorMultiplier).ToString());

            if (targetStatus!=null)
            {
                description = description.Replace("{status}", targetStatus.title);
            } else if (userStatus!=null)
            {
                description = description.Replace("{status}", userStatus.title);
            }
            description = description.Replace("{statusdamage}", ((int)(playerShell.brain.GetStatusDamage()*targetDamageMultiplier)).ToString());
            description = description.Replace("{statusheal}",((int)(playerShell.brain.GetHeal()*targetHealMultiplier)).ToString());

        
        return description;
    }
    public string GetTranslatedDescriptionB(PlayerShell playerShell)
    {
        string description = descriptionB;
            int baseDamage = playerShell.brain.GetDamage();
            description = description.Replace("{damage}", (baseDamage * userDamageMultiplier).ToString());
            int baseHeal = playerShell.brain.GetHeal();
            description = description.Replace("{heal}", (baseHeal * userHealMultiplier).ToString());
            int baseArmor = playerShell.brain.GetShieldRate();
            description = description.Replace("{armor}", (baseArmor * userArmorMultiplier).ToString());
            if (userStatus!=null)
            {
                description = description.Replace("{status}", userStatus.title);
            }
            else if (targetStatus!=null)
            {
                description = description.Replace("{status}", targetStatus.title);
            }
            description = description.Replace("{statusdamage}", ((int)(playerShell.brain.GetStatusDamage()*userDamageMultiplier)).ToString());
            description = description.Replace("{statusheal}", ((int)(playerShell.brain.GetHeal()*userHealMultiplier)).ToString());

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