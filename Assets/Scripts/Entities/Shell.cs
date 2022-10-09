using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Shell : MonoBehaviour
{
    [field: SerializeField] public Brain brain { get; private set; }
    [field: SerializeField] public string title { get; private set; } = "entity.name";
    [field: SerializeField] public string description { get; private set; } = "entity.description";
    [field: SerializeField] public int currentHealth { get; protected set; } = 0;
    [field: SerializeField] public int shield { get; protected set; } = 0;
    [field: SerializeField] public int shieldDelayCurrent { get; protected set; } = 0;
    [SerializeField] public StatusDisplayer statusDisplayer;
    [SerializeField] public HealthBar healthBar;
    public bool isDead => currentHealth <= 0;
    public bool hasDied = false;
    public bool hasBrain = false;
    public bool hasShield => shield > 0;
    public bool isPlayer => brain is PlayerBrain;
    private SpriteRenderer spriteRenderer;
    
  

    public void Start()
    {
        Attacked+=OnAttacked;
        Damaged+=OnDamaged;
        ShieldBreak+=OnShieldBreak;
        ShieldRegen+=OnShieldRegen;
        Healed += OnHealed;
        Died+=OnDied;
    }
    public void OnDestroy()
    {
        foreach (Relic relic in brain.relics)
        {
            relic.Unsubscribe(this);
        }
        Attacked-=OnAttacked;
        Damaged-=OnDamaged;
        ShieldBreak-=OnShieldBreak;
        ShieldRegen-=OnShieldRegen;
        Healed -= OnHealed;
        Died-=OnDied;
    }

    #region Event Broadcasts
    public event UnityAction<int> ShieldBreak = delegate { };
    public event UnityAction ShieldRegen = delegate { };
    public event UnityAction Died = delegate { };
    public event UnityAction<int> Healed = delegate { };
    public event UnityAction<Shell,Symbol> Attacked = delegate { };
    public event UnityAction<Shell,int> Damaged = delegate { };

    public virtual void OnDied()
    {
    }

    public virtual void OnShieldRegen()
    {
    }

    public virtual void OnHealed(int amtHealed)
    {
    }

    public virtual void OnShieldBreak(int shieldDamTaken)
    {
    }

    public virtual void OnDamaged(Shell attacker, int damageTaken)
    {
    }

    public virtual void OnAttacked(Shell target,Symbol symbol)
    {
    }
    
    #endregion
   
    public virtual void Attack(Shell target,Symbol attack)
    {
        Attacked.Invoke(target,attack);
        attack.Consume(target,this);
    }

    public virtual int OnAttack(Shell target,int baseDamage)
    {
        return statusDisplayer.OnAttack(target,this,baseDamage);
    }
    
    public virtual int OnShield(Shell target,int baseShield)
    {
        return statusDisplayer.OnShield(target,this,baseShield);
    }
    public virtual int OnHeal(Shell target,int baseHeal)
    {
        return statusDisplayer.OnHeal(target,this,baseHeal);
    }
    
    public void Damage(Shell source,int baseDamage,bool crit)
    {
        int damage = statusDisplayer.OnDamage(source,this,baseDamage);
        if (damage > 0)
        {
            if (crit)
            {
                TextPopController.Instance.PopCritical(damage, transform.position);
            }
            else
            {
                TextPopController.Instance.PopDamage(damage, transform.position);
            }
            Damaged.Invoke(source, damage);
            shieldDelayCurrent = 1;
            foreach (Relic brainRelic in brain.relics)
            {
                if (brainRelic.modifyShieldDelay)
                {
                    shieldDelayCurrent += Mathf.RoundToInt(shieldDelayCurrent*brainRelic.modifyShieldDelayPercent);
                }

                if (brainRelic.reflect)
                {
                    if (source != this)
                    {
                        source.Damage(this, Mathf.RoundToInt(damage*brainRelic.reflectPercent), false);
                    }
                }
            }
            
        }

        if (shield > 0)
        {
            shield -= damage;
            if (shield < 0)
            {
                ShieldBreak.Invoke(damage + shield);
                ModifyCurrentHealth(shield);
                shield = 0;
            }
        }
        else
        {
            ModifyCurrentHealth(-damage);
        }
        healthBar.ManualUpdate();

       TestDeath();
    }

    
    public virtual void Kill()
    {
        currentHealth = 0;
        Died.Invoke();
        spriteRenderer.sprite = null;
        statusDisplayer.Clear();
        healthBar.ManualUpdate();
        hasDied = true;
        SoundManager.Instance.PlayDeathSound();
    }
    
    public virtual void Heal(int baseHeal)
    {
        Healed.Invoke(baseHeal);
        TextPopController.Instance.PopHeal(baseHeal,transform.position);

        // brain.ModifyCurrentHealth(baseHeal);
        ModifyCurrentHealth(baseHeal);
        healthBar.ManualUpdate();
    }
    
    public virtual void Shield(int amount)
    {
        if (shield +amount> brain.GetShieldMax())
        {
            amount = (int)(brain.GetShieldMax() - shield);
        }
        shield += amount;
        if (amount>0)
        {
            TextPopController.Instance.PopShield(amount,transform.position);
        }
        healthBar.ManualUpdate();

    }

    public void AddStatusEffect(StatusEffect statusEffect, Shell source, int duration)
    {
        foreach (Relic brainRelic in brain.relics)
        {
            if (brainRelic.immuneToStatus)
            {
                if (statusEffect.GetType().Equals(brainRelic.statusTaken.GetType()))
                {
                    return;
                }
            }
        }

        foreach (Relic brainRelic in source.brain.relics)
        {
            if (brainRelic.addStatusEffectDuration)
            {
                if (brainRelic.statusGiven.GetType().Equals(statusEffect.GetType()))
                {
                    duration += brainRelic.addStatusEffectDurationAmount;
                }
            }

            if (brainRelic.doubleStatus)
            {
                if (brainRelic.statusGiven.GetType().Equals(statusEffect.GetType()))
                {
                    duration *= 2;
                }
            }
        }

        statusDisplayer.AddStatus(statusEffect, this, source, duration);
    }

    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        statusDisplayer.RemoveStatus(statusEffect);
    }

    public async Task TickStatusEffects()
    {
        await statusDisplayer.Tick();
    }

    public void ShieldCheck()
    {
        shieldDelayCurrent--;
        if (shieldDelayCurrent < 0)
        {
            // shieldDelayCurrent = 0;
            ShieldRegen.Invoke();
            Shield((int)brain.GetShieldRate());
        }
    }

    public virtual void InsertBrain(Brain brain)
    {
        this.brain = brain;
        hasBrain = true;
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.sprite = brain.icon;
        title = brain.title;
        description = brain.description;
        shield = (int)brain.GetShieldMax();
        shieldDelayCurrent = 1;
        for (var index = 0; index < brain.relics.Length; index++)
        {
            Relic relic = brain.relics[index];
            relic.Subscribe(this);
        }
        healthBar.ManualUpdate();
    }

    public async Task OnTurnEnd()
    {
        healthBar.ManualUpdate();
        if (!isDead)
        {
            TestDeath();
            ShieldCheck();
        }
    }
    public void SetCurrentHealth(int health)
    {
        if (health>brain.GetHealthMax())
        {
            health = brain.GetHealthMax();
        }
        currentHealth = health;
        healthBar.ManualUpdate();

    }
    public void ModifyCurrentHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > brain.GetHealthMax())
        {
            SetCurrentHealth(brain.GetHealthMax());
        }
        healthBar.ManualUpdate();

    }
    public void TestDeath()
    {
        if (isDead && !hasDied)
        {
            Kill();
        }
    }
}
