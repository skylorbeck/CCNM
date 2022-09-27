using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class EffectInstance : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Shell target;
    [SerializeField] private Shell source;
    [field: SerializeField] public StatusEffect statusEffect { get; private set; }
    [field: SerializeField] public int duration { get; private set; }
    [field: SerializeField] public int power { get; private set; }
    public bool isActive { get; private set; } = true;
    private TextMeshPro durationText;

    public void Start()
    {
        durationText = GetComponentInChildren<TextMeshPro>();
    }

    public int OnAttack(Shell target, Shell attacker, int baseDamage)
    {
        return  statusEffect.OnAttack(target, attacker, baseDamage);
    }

    public int OnDamage([CanBeNull] Shell attacker, Shell defender, int baseDamage)
    {
        return  statusEffect.OnDamage(attacker, defender, baseDamage);
    }
    public int OnDodge([CanBeNull] Shell attacker, Shell defender, int baseDamage)
    {
        return  statusEffect.OnDodge(attacker, defender, baseDamage);
    }
    public int OnHeal([CanBeNull] Shell healer, Shell target, int baseHeal)
    {
        return  statusEffect.OnHeal(healer, target, baseHeal);
    }
    public int OnShield([CanBeNull] Shell shielder, Shell target, int baseShield)
    {
        return  statusEffect.OnShield(shielder, target, baseShield);
    }

    public void Tick()
    {
        if (!statusEffect.isPersistent)
        {
            duration--;
        }

        UpdateDurationText();

        statusEffect.Tick(target,source,duration,power);

        if (duration <= 0 || statusEffect.alwaysExpires)
        {
            Expire();
        }

    }

    public void OnDestroy()
    {
        statusEffect.OnRemove(target);
    }

    public void Expire()
    {
        isActive = false;
    }

    public void SetStatusEffect(StatusEffect statusEffect, Shell target, Shell source,int duration,int power)
    {
        this.target = target;
        this.source = source;
        this.statusEffect = statusEffect;
        this.power = power;
        durationText = GetComponentInChildren<TextMeshPro>();
        AddDuration(duration);
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

        statusEffect.OnApply(target,source,duration,power);
        UpdateDurationText();
    }

    public void MoreOrEqualPower(int power)
    {
        if (power > this.power)
        {
            this.power = power;
        }
    }

    public void UpdateDurationText()
    {
        durationText.text = duration > 0 ? duration.ToString() : "";
    }

    public void EnableVisuals()
    {
        spriteRenderer.color = Color.white;
        durationText.renderer.enabled = true;
    }
    public void DisableVisuals()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
        durationText.renderer.enabled = false;
    }
}
