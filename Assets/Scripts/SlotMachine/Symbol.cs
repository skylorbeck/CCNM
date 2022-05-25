using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Symbol : MonoBehaviour
{
    [field:SerializeField] public AbilitySO ability { get; private set; }
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer statusSpriteRenderer;
    private int statusSpriteIndex = 0;
    [SerializeField] float darknessRamp = 0.5f;
    [field:SerializeField] public bool consumed { get; private set; } = false;

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = transform.localPosition.y < 2.5f;
        spriteRenderer.sprite = ability.icon;
        statusSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        GameManager.Instance.FixedSecond += CycleStatusEffect;
    }

    private void OnDisable()
    {
        consumed = false;
        GameManager.Instance.FixedSecond -= CycleStatusEffect;
    }

    void Update()
    {
        var localPosition = transform.localPosition;
        spriteRenderer.enabled = localPosition.y < 2.5f;
        spriteRenderer.color =consumed?Color.gray: Color.Lerp(Color.white, Color.black, Math.Abs(localPosition.y * darknessRamp));
        if (ability.statusEffects.Length > 0)
        {
            statusSpriteRenderer.color =
                Color.Lerp(Color.white, Color.black, Math.Abs(localPosition.y / 2));
        }
    }

    void CycleStatusEffect()
    {
        if (ability.statusEffects.Length > 0)
        {
            statusSpriteIndex++;
            if (statusSpriteIndex >= ability.statusEffects.Length)
            {
                statusSpriteIndex = 0;
            }
            statusSpriteRenderer.sprite = ability.statusEffects[statusSpriteIndex].icon;
        }
    }

    public void SetAbility(AbilitySO newAbility)
    {
        ability = newAbility;
        spriteRenderer.sprite = ability.icon;
        if (ability.statusEffects.Length > 0)
        {
            statusSpriteRenderer.sprite = ability.statusEffects[0].icon;
        } else {
            statusSpriteRenderer.sprite = null;
        }
        
    }

    public void Consume(Shell target,Shell user)
    {
        if (ability.statusTarget)
        {
            foreach (StatusEffect statusEffect in ability.statusEffects)
            {
                target.AddStatusEffect(statusEffect);
            }
        }
        if (ability.statusSelf)
        {
            foreach (StatusEffect statusEffect in ability.statusEffects)
            {
                user.AddStatusEffect(statusEffect);
            }
        }
        if (ability.healTarget)
        {
            target.Heal(ability.baseHeal);
        }
        if (ability.healUser)
        {
            user.Heal(ability.baseHeal);
        }
        if (ability.damageTarget)
        {
            target.Damage(ability.baseDamage);
        }
        if (ability.damageUser)
        {
            user.Damage(ability.baseDamage);
        }
        consumed = true;
    }
}
