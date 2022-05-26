using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class EffectInstance : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Shell target;
    [field:SerializeField] public StatusEffect statusEffect { get; private set; }
    [field:SerializeField] public int duration { get; private set; }
    public bool isActive { get; private set; } = true;
    private TextMeshPro durationText;

    public void Start()
    {
        durationText = GetComponentInChildren<TextMeshPro>();
    }

    public async Task<int> OnAttack(Shell target,Shell attacker,int baseDamage)
    {
        return await statusEffect.OnAttack(target, attacker,baseDamage);
        
    }
    public async Task<int> OnDamage([CanBeNull] Shell attacker,Shell defender,int baseDamage)
    {
        return await statusEffect.OnDamage(attacker, defender, baseDamage);
    }

    public async Task Tick()
    {
        if (!statusEffect.isPersistent)
        {
            duration--;
        }
        UpdateDurationText();

        await statusEffect.Tick(target);

        if (duration <= 0)
        {
            Expire();
        }

    }

    public void Expire()
    {
        isActive = false;
        statusEffect.OnRemove(target);
    }

    public void SetStatusEffect(StatusEffect statusEffect, Shell shell)
    {
        target = shell;
        this.statusEffect = statusEffect;
        durationText = GetComponentInChildren<TextMeshPro>();
        AddDuration(statusEffect.duration);
        UpdateDurationText();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = statusEffect.icon;
    }

    public void AddDuration(int duration)
    {
        if (statusEffect.maxStacks == 0)
            this.duration += duration;
        else
            this.duration = Mathf.Min(this.duration + duration, statusEffect.maxStacks);

        statusEffect.OnApply(target);
        UpdateDurationText();
    }

    public void UpdateDurationText()
    {
        durationText.text = duration>0 ? duration.ToString() : "";
    }
}
