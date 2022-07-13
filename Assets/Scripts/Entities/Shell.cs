using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
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
    public bool isDead => currentHealth <= 0;
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
   
    public virtual async Task Attack(Shell target,Symbol attack)
    {
        Attacked.Invoke(target,attack);
        await attack.Consume(target,this);
    }

    public virtual async Task<int> OnAttack(Shell target,int baseDamage)
    {
        return await statusDisplayer.OnAttack(target,this,baseDamage);
    }
    
    public virtual async Task<int> OnShield(Shell target,int baseShield)
    {
        return await statusDisplayer.OnShield(target,this,baseShield);
    }
    public virtual async Task<int> OnHeal(Shell target,int baseHeal)
    {
        return await statusDisplayer.OnHeal(target,this,baseHeal);
    }
    
    public async Task Damage([CanBeNull] Shell source,int baseDamage, StatusEffect.Element element)
    {
        if (Random.Range(0,100) < brain.GetDodgeChance())
        {
            await statusDisplayer.OnDodge(source,this,baseDamage);
            TextPopController.Instance.PopPositive("Dodged",transform.position,true);
            return;
        }
        int damage = await statusDisplayer.OnDamage(source,this,baseDamage);
        TextPopController.Instance.PopDamage(damage,transform.position);
        Damaged.Invoke(source,damage);
        if (shield > 0)
        {
            shieldDelayCurrent = 1;
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
            ModifyCurrentHealth(damage);
        }

       TestDeath();
    }

    
    public virtual void Kill()
    {
        Died.Invoke();
        spriteRenderer.sprite = null;
        statusDisplayer.Clear();
    }
    
    public virtual void Heal(int baseHeal, StatusEffect.Element element)
    {
        Healed.Invoke(baseHeal);
        TextPopController.Instance.PopHeal(baseHeal,transform.position);

        brain.ModifyCurrentHealth(baseHeal);
        
    }
    
    public virtual void Shield(int amount, StatusEffect.Element element)
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
    }
    
    public void AddStatusEffect(StatusEffect statusEffect)
    {
        statusDisplayer.AddStatus(statusEffect,this);
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
            Shield((int)brain.GetShieldRate(), StatusEffect.Element.None);
        }
    }

    public virtual void InsertBrain(Brain brain)
    {
        this.brain = brain;
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
    }

    public async Task OnTurnEnd()
    {
        // await TickStatusEffects();
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
    }
    public void ModifyCurrentHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > brain.GetHealthMax())
        {
            SetCurrentHealth(brain.GetHealthMax());
        }
    }
    public void TestDeath()
    {
        if (currentHealth <= 0)
        {
            Kill();
        }
    }
}
