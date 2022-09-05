using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class Symbol : MonoBehaviour
{
    [field:SerializeField] public AbilityObject ability { get; private set; }//todo replace with AbilityGem?
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
        ability.Execute(user,target);
        SoundManager.Instance.PlaySound(ability.soundEffect);
    }
}
