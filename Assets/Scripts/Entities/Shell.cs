using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shell : MonoBehaviour
{
    [field: SerializeField] public Brain brain { get; private set; }
    
    [field: SerializeField] public string title { get; private set; } = "entity.name";
    [field: SerializeField] public string description { get; private set; } = "entity.description";
    [field: SerializeField] public int health { get; protected set; } = 10;
    [field: SerializeField] public int maxHealth { get; protected set; } = 10;
    [field: SerializeField] public int shield { get; protected set; } = 0;
    [field: SerializeField] public int shieldMax { get; protected set; } = 0;
    [field: SerializeField] public int shieldDelay { get; protected set; } = 0;
    [field: SerializeField] public int shieldDelayMax { get; protected set; } = 0;
    [field: SerializeField] public int shieldRate { get; protected set; } = 0;
    [field: SerializeField] public int dodgeChance { get; protected set; } = 0;
    [field: SerializeField] public AbilityObject[] abilities { get; private set; } = new AbilityObject[0];
    [SerializeField] public StatusDisplayer statusDisplayer;
    public bool isDead => health <= 0;
    public bool hasShield => shield > 0;
    public bool isPlayer => !(brain is EnemyBrain);

    protected SpriteRenderer spriteRenderer;

    public virtual async Task Attack(Shell target,Symbol attack)
    {
        await attack.Consume(target,this);
        await Task.Delay(500);
        target.TestDeath();
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
        if (Random.Range(0,100) < dodgeChance)
        {
            await statusDisplayer.OnDodge(source,this,baseDamage);
            TextPopController.Instance.PopPositive("Dodged",transform.position,true);
            return;
        }
        int damage = await statusDisplayer.OnDamage(source,this,baseDamage);
        TextPopController.Instance.PopDamage(damage,transform.position);

        if (shield > 0)
        {
            shieldDelay = shieldDelayMax;
            shield -= damage;
            if (shield < 0)
            {
                health += shield;
                shield = 0;
            }
        }
        else
        {
            health -= damage;
        }

        if (health <=0)
        {
            health = 0;
        }
    }

    
    public virtual void Kill()
    {
        spriteRenderer.sprite = null;
        statusDisplayer.Clear();
    }
    
    public virtual void Heal(int baseHeal, StatusEffect.Element element)
    {
        TextPopController.Instance.PopHeal(baseHeal,transform.position);

        health += baseHeal;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
    
    public virtual void Shield(int amount, StatusEffect.Element element)
    {
        if (shield +amount> shieldMax)
        {
            amount = shieldMax - shield;
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
        shieldDelay--;
        if (shieldDelay < 0)
        {
            shieldDelay = 0;
            Shield(shieldRate, StatusEffect.Element.None);
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
        health = brain.maxHealth;
        maxHealth = brain.maxHealth;
        shield = brain.baseShield;
        shieldMax = brain.baseShield;
        shieldRate = brain.baseShieldRate;
        shieldDelay = brain.baseShieldDelay;
        shieldDelayMax = brain.baseShieldDelay;
        dodgeChance = brain.baseDodge;
        abilities = brain.abilities;
    }

    public async Task OnTurnEnd()
    {
        // await TickStatusEffects();
        TestDeath();
        if (!isDead)
        {
            ShieldCheck();
        }
    }

    public void TestDeath()
    {
        if (health <= 0)
        {
            Kill();
        }
    }
}
