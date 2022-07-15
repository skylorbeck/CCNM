using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class Symbol : MonoBehaviour
{
    [field:SerializeField] public AbilityObject ability { get; private set; }
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer userStatusSprite;
    [SerializeField] private SpriteRenderer userStatusShadow;
    [SerializeField] private SpriteRenderer targetStatusSprite;
    [SerializeField] private SpriteRenderer targetStatusShadow;
    [SerializeField] float darknessRamp = 0.5f;
    private static Color negative = new Color(1, 0.364f, 0.364f);
    private static Color positive = new Color(0, 1, 0.67f);
    
    [field:SerializeField] public bool consumed { get; private set; } = false;


    private void OnDisable()
    {
        consumed = false;
    }

    void Update()
    {
        var localPosition = transform.localPosition;
        // spriteRenderer.enabled = localPosition.y < 2.5f;
        spriteRenderer.color = consumed
            ? Color.gray
            : Color.Lerp(Color.white, Color.black, Math.Abs(localPosition.y * darknessRamp));

        if (ability.userStatus)
        {
            userStatusSprite.color =
                Color.Lerp(
                    consumed ? Color.gray : ability.userStatus.isDebuff ? negative : positive,
                    Color.black, Math.Abs(localPosition.y / 2));
        }
        if (ability.targetStatus)
        {
            targetStatusSprite.color =
                Color.Lerp(
                    consumed ? Color.gray : ability.targetStatus.isDebuff ? negative : positive,
                    Color.black, Math.Abs(localPosition.y / 2));
        }
        
    }

    public void SetAbility(AbilityObject newAbility)
    {
        ability = newAbility;
        spriteRenderer.sprite = ability.icon;
        if (ability.statusSelf)
        {
            userStatusSprite.sprite = ability.userStatus.icon;
            userStatusShadow.sprite = ability.userStatus.icon;
        } else {
            userStatusSprite.sprite = null;
            userStatusShadow.sprite = null;
        }
        if (ability.statusTarget)
        {
            targetStatusSprite.sprite = ability.targetStatus.icon;
            targetStatusShadow.sprite = ability.targetStatus.icon;
        } else {
            targetStatusSprite.sprite = null;
            targetStatusShadow.sprite = null;
        }
        
    }

    public async Task Consume(Shell target,Shell user)
    {
        consumed = true;

        /*if (ability.armorTarget)
        {
            int shield = await target.OnShield(target,user.brain.baseShield);
            shield = (int)Math.Ceiling(shield * ability.targetArmorMultiplier);
            target.Shield(shield, ability.element);
        }
        if (ability.armorUser)
        {
            int shield = await user.OnShield(user,user.brain.baseShield);
            shield = (int)Math.Ceiling(shield * ability.userArmorMultiplier);
            user.Shield(shield, ability.element);
        }*/
        if (ability.statusTarget)
        {
            target.AddStatusEffect(ability.targetStatus);
        }
        if (ability.statusSelf)
        {
            user.AddStatusEffect(ability.userStatus);
        }
        if (ability.healTarget)
        {
            int heal = await target.OnHeal(target,(int)(user.brain.GetHealthMax()*0.25f));
            heal = (int)Math.Ceiling(heal * ability.targetHealMultiplier);
            target.Heal(heal, ability.element);
        }
        if (ability.healUser)
        {
            int heal = await user.OnHeal(user,(int)(user.brain.GetHealthMax()*0.25f));
            heal = (int)Math.Ceiling(heal * ability.userHealMultiplier);
            user.Heal(heal, ability.element);
        }
        if (ability.damageTarget)
        {
            int damage = await user.OnAttack(target,user.brain.GetDamage());//process status influences
            damage = (int)(damage * ability.targetDamageMultiplier);//process ability multiplier
            if (Random.Range(0,100)<user.brain.GetCritChance())
            {
               damage +=  (int)(damage * user.brain.GetCritDamage());
               //todo crit notification
            }
            DamageAnimator.Instance.TriggerAttack(target, ability.attackAnimation);//play the animation
            await target.Damage(user, damage, ability.element);//damage the target
            // target.TestDeath(); handled in target.damage
        }
        if (ability.damageUser)
        {
            int damage = await user.OnAttack(user,user.brain.GetDamage());
            damage = (int)(damage * ability.userDamageMultiplier);
            if (Random.Range(0,100)<user.brain.GetCritChance())
            {
                damage +=  (int)(damage * user.brain.GetCritDamage());
                //todo crit notification
            }
            DamageAnimator.Instance.TriggerAttack(user, ability.attackAnimation);
            await user.Damage(user,damage,ability.element);
            // user.TestDeath();
        }
        SoundManager.instance.PlaySound(ability.soundEffect);
    }
}
