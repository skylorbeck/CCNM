using System;
using System.Globalization;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Ability", menuName = "Combat/Ability/Base")]
[Serializable]
public class AbilityObject : ScriptableObject
{

    [field: SerializeField] public string descriptionA { get; private set; } = "description.a";
    [field: SerializeField] public string descriptionB { get; private set; } = "description.b";
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public int baseCost { get; private set; } = 1;

    [field: Header("Damage")]
    [field: SerializeField] public bool damageTarget { get; private set; } = false;
    [field: SerializeField] public float targetDamageMultiplier { get; private set; } = 1;
    [field: SerializeField] public int targetHitCount { get; private set; } = 1;
    [field: SerializeField] public bool damageUser { get; private set; } = false;
    [field: SerializeField] public float userDamageMultiplier { get; private set; } = 1;
    [field: SerializeField] public int userHitCount { get; private set; } = 1;


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
    [field: SerializeField] public StatusEffect targetStatus2 { get; private set; }

    [field: SerializeField] public bool statusTarget { get; private set; } = false;
    [field: SerializeField] public int targetStatusDuration { get; private set; } = 1;
    [field: SerializeField] public StatusEffect userStatus { get; private set; }
    [field: SerializeField] public bool statusUser { get; private set; } = false;
    [field: SerializeField] public int userStatusDuration { get; private set; } = 1;

    [field: Header("Other")]
    [field: SerializeField]
    public bool wideRange { get; private set; } = false;
    // [field: SerializeField] public GemShape gemShape { get; private set; } = GemShape.Square;
    // [field: SerializeField] public AbilityType abilityType { get; private set; } = AbilityType.Fire;

    [field: SerializeField] public AttackAnimator.AttackType attackAnimation { get; private set; } = AttackAnimator.AttackType.None;

    [field: SerializeField] public AudioClip soundEffect { get; private set; }

    public enum AbilityType
    {
        Ice,
        Fire,
        Shock,
        Rock,
    }
    public enum GemShape
    {
        Square,
        Circle,
        Plumbob,
        Diamond
    }
    
    public virtual void Execute(Shell user, Shell target)
    {
        
        bool crit = Random.Range(0, 100) < user.brain.GetCritChance();//todo check to make sure this is actually 0-100 and not 0f-1f
        bool blind = user.statusDisplayer.HasStatus(typeof(BlindEffect));
        bool dodged = Random.Range(0,100) < target.brain.GetDodgeChance();//todo check to make sure this is actually 0-100 and not 0f-1f
        
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
        if (healUser)
        {
            int heal = user.OnHeal(user, (int)(user.brain.GetHealthMax() * 0.25f));
            heal = (int)Math.Ceiling(heal * userHealMultiplier);
            user.Heal(heal);
        }
        if (statusUser)
        {
            user.AddStatusEffect(userStatus, user, userStatusDuration);
        }
        if (damageUser)
        {
            for (int i = 0; i < userHitCount; i++)
            {

                int damage = user.OnAttack(user, user.brain.GetDamage());
                damage = (int)(damage * userDamageMultiplier);
                if (crit)
                {
                    int critbonus = (int)(damage * user.brain.GetCritDamage());
                    damage += critbonus;
                }

                DamageAnimator.Instance.TriggerAttack(user, attackAnimation);
                user.Damage(user, damage, crit);
                // user.TestDeath();
            }
        }

        if (blind)
        {
            user.statusDisplayer.RemoveStatus(typeof(BlindEffect));
            TextPopController.Instance.PopNegative("Blind", user.transform.position, true);
            return;
        }
        
        if (healTarget)
        {
            int heal = target.OnHeal(target, (int)(user.brain.GetHealthMax() * 0.25f));
            heal = (int)Math.Ceiling(heal * targetHealMultiplier);
            target.Heal(heal);
        }
        if (statusTarget)
        {
            target.AddStatusEffect(targetStatus, user, targetStatusDuration);
            if (targetStatus2 != null)
            {
                target.AddStatusEffect(targetStatus2, user, targetStatusDuration);
            }
        }
        
        if (damageTarget)
        {
            for (int i = 0; i < targetHitCount; i++)
            {
                int damage = user.OnAttack(target, user.brain.GetDamage()); //process status influences
                damage = (int)(damage * targetDamageMultiplier); //process ability multiplier
                if (crit)
                {
                    int critbonus = (int)(damage * user.brain.GetCritDamage());
                    damage += critbonus;
                }

                if (dodged)
                {
                    target.statusDisplayer.OnDodge(user, target, damage);
                    TextPopController.Instance.PopPositive("Dodged", target.transform.position, true);
                }
                else
                {
                    DamageAnimator.Instance.TriggerAttack(target, attackAnimation); //play the animation
                    target.Damage(user, damage, crit); //damage the target
                }
            }
        }

        foreach (Relic brainRelic in user.brain.relics)
        {
            if (crit)
            {
                if (brainRelic.healCrit)
                {
                    user.Heal(Mathf.RoundToInt(user.brain.GetHeal()*brainRelic.healCritPercent));
                }

                if (brainRelic.shieldCrit)
                {
                    user.Shield(Mathf.RoundToInt(user.brain.GetShieldRate()*brainRelic.shieldCritPercent));
                }

                if (brainRelic.statusCrit)
                {
                    target.AddStatusEffect(brainRelic.critStatus,user,brainRelic.critStatusDuration);
                }
                
            }

            bool relicHit = Random.Range(0f, 1f) < brainRelic.hitChance;
            if (relicHit)
            {
                if (brainRelic.healHit)
                {
                    user.Heal(Mathf.RoundToInt(user.brain.GetHeal()*brainRelic.healHitPercent));
                }

                if (brainRelic.shieldHit)
                {
                    user.Shield(Mathf.RoundToInt(user.brain.GetShieldRate()*brainRelic.shieldHitPercent));
                }

                if (brainRelic.statusHit)
                {
                    target.AddStatusEffect(brainRelic.hitStatus,user,brainRelic.hitStatusDuration);
                }
            }

            if (dodged)
            {
                if (brainRelic.healDodge)
                {
                    user.Heal(Mathf.RoundToInt(user.brain.GetHeal()*brainRelic.healDodgePercent));
                }

                if (brainRelic.shieldDodge)
                {
                    user.Shield(Mathf.RoundToInt(user.brain.GetShieldRate()*brainRelic.shieldDodgePercent));
                }

                if (brainRelic.statusDodge)
                {
                    target.AddStatusEffect(brainRelic.dodgeStatus,user,brainRelic.dodgeStatusDuration);
                }
            }

            if (target.isDead)
            {
                if (brainRelic.healKill)
                {
                    user.Heal(Mathf.RoundToInt(user.brain.GetHeal()*brainRelic.healKillPercent));
                }

                if (brainRelic.shieldKill)
                {
                    user.Shield(Mathf.RoundToInt(user.brain.GetShieldRate()*brainRelic.shieldKillPercent));
                }
            }
        }
    }

    public string GetTranslatedDescriptionA(PlayerBrain playerBrain)//todo 
    {
        string description = descriptionA;
            int baseDamage = playerBrain.GetDamage();
            description = description.Replace("{damage}", (baseDamage * targetDamageMultiplier).ToString(CultureInfo.CurrentCulture));
            int baseHeal = playerBrain.GetHeal();
            description = description.Replace("{heal}", (baseHeal * targetHealMultiplier).ToString(CultureInfo.CurrentCulture));
            int baseArmor = playerBrain.GetShieldRate();
            description = description.Replace("{armor}", (baseArmor * targetArmorMultiplier).ToString(CultureInfo.CurrentCulture));

            if (targetStatus!=null)
            {
                description = description.Replace("{status}", targetStatus.name);
            } else if (userStatus!=null)
            {
                description = description.Replace("{status}", userStatus.name);
            }
            description = description.Replace("{statusdamage}", ((int)(playerBrain.GetStatusDamage()*targetDamageMultiplier)).ToString());
            description = description.Replace("{statusheal}",((int)(playerBrain.GetHeal()*targetHealMultiplier)).ToString());

        
        return description;
    }
    public string GetTranslatedDescriptionB(PlayerBrain playerBrain)
    {//todo clean these up
        string description = descriptionB;
            int baseDamage = playerBrain.GetDamage();
            description = description.Replace("{damage}", (baseDamage * userDamageMultiplier).ToString(CultureInfo.CurrentCulture));
            int baseHeal = playerBrain.GetHeal();
            description = description.Replace("{heal}", (baseHeal * userHealMultiplier).ToString(CultureInfo.CurrentCulture));
            int baseArmor = playerBrain.GetShieldRate();
            description = description.Replace("{armor}", (baseArmor * userArmorMultiplier).ToString(CultureInfo.CurrentCulture));
            if (userStatus!=null)
            {
                description = description.Replace("{status}", userStatus.name);
            }
            else if (targetStatus!=null)
            {
                description = description.Replace("{status}", targetStatus.name);
            }
            description = description.Replace("{statusdamage}", ((int)(playerBrain.GetStatusDamage()*userDamageMultiplier)).ToString());
            description = description.Replace("{statusheal}", ((int)(playerBrain.GetHeal()*userHealMultiplier)).ToString());
            description = description.Replace("{statusduration}", targetStatusDuration.ToString());
            
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
        this.abilityIndex = GameManager.Instance.abilityRegistry.GetAbilityIndex(ability.name);
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