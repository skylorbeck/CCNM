using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EffectInstance : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Shell target;
    public StatusEffect statusEffect { get; private set; }
    [field:SerializeField] public int duration { get; private set; }
    public bool isActive { get; private set; } = true;
    private TextMeshPro durationText;

    public void Tick()
    {
        if (!statusEffect.isPersistent)
        {
            duration--;
        }
        
        if (duration <= 0)
        {
            Expire();
        }
        statusEffect.Tick(target);
        UpdateDurationText();

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
        AddDuration(statusEffect.duration);
        durationText = GetComponentInChildren<TextMeshPro>();
        UpdateDurationText();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = statusEffect.icon;
    }
    
    public void AddDuration(int duration)
    {
        if (statusEffect.isStackable)
        {
            if (statusEffect.maxStacks == 0)
                this.duration += duration;
            else
                this.duration = Mathf.Min(this.duration + duration, statusEffect.maxStacks);
        }

        statusEffect.OnApply(target);
        UpdateDurationText();
    }
    
    public void UpdateDurationText()
    {
        durationText.text = duration>0 ? duration.ToString() : "";
    }
}
