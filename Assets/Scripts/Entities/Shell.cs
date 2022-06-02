using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

public class Shell : MonoBehaviour
{
    [field: SerializeField] public Brain brain { get; private set; }
    
    [field: SerializeField] public string title { get; private set; } = "entity.name";
    [field: SerializeField] public string description { get; private set; } = "entity.description";
    [field: SerializeField] public int health { get; protected set; } = 10;
    [field: SerializeField] public int maxHealth { get; protected set; } = 10;
    [field: SerializeField] public int shield { get; protected set; } = 0;
    [field: SerializeField] public int maxShield { get; protected set; } = 0;
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

    public async Task<int> OnAttack(Shell target,int baseDamage)
    {
        return await statusDisplayer.OnAttack(target,this,baseDamage);
    }
    
    public async Task Damage([CanBeNull] Shell source,int baseDamage, StatusEffect.Element element)
    {
        int damage = await statusDisplayer.OnDamage(source,this,baseDamage);
        TextPopController.Instance.PopDamage(damage,transform.position);

        if (shield > 0)
        {
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
    
    public async void Heal([CanBeNull] Shell source,int baseHeal, StatusEffect.Element element)
    {
        int heal = await statusDisplayer.OnHeal(source,this,baseHeal);
        TextPopController.Instance.PopHeal(heal,transform.position);

        health += heal;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
    
    public virtual void Shield(int amount)
    {
        shield += amount;
        TextPopController.Instance.PopShield(amount,transform.position);

        if (shield > maxShield)
        {
            maxShield = shield;
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
        shield = brain.startingShield;
        maxShield = brain.startingShield;
        abilities = brain.abilities;
    }

    public async Task OnTurnEnd()
    {
        // await TickStatusEffects();
        TestDeath();

    }

    public void TestDeath()
    {
        if (health <= 0)
        {
            Kill();
        }
    }
}
