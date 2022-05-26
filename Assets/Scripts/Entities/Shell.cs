using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

public class Shell : MonoBehaviour
{
    [field: SerializeField] public Brain brain { get; private set; }
    
    [field: SerializeField] public string title { get; private set; } = "entity.name";
    [field: SerializeField] public string description { get; private set; } = "entity.description";
    [field: SerializeField] public int health { get; private set; } = 10;
    [field: SerializeField] public int maxHealth { get; private set; } = 10;
    [field: SerializeField] public int shield { get; private set; } = 0;
    [field: SerializeField] public int maxShield { get; private set; } = 0;
    [field: SerializeField] public AbilitySO[] abilities { get; private set; } = new AbilitySO[0];
    [SerializeField] public StatusDisplayer statusDisplayer;
    public bool isDead => health <= 0;
    public bool hasShield => shield > 0;

    private SpriteRenderer spriteRenderer;

    public async Task Attack(Shell target,Symbol attack)
    {
        await attack.Consume(target,this);
    }

    public async Task<int> OnAttack(Shell target,int baseDamage)
    {
        return await statusDisplayer.OnAttack(target,this,baseDamage);
    }
    
    public async Task Damage([CanBeNull] Shell source,int baseDamage, StatusEffect.Element element)
    {
        int damage = await statusDisplayer.OnDamage(source,this,baseDamage);
        
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

    public void Kill()
    {
        spriteRenderer.sprite = null;
        statusDisplayer.Clear();
    }
    
    public void Heal(int heal)
    {
        health += heal;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
    
    public void Shield(int amount)
    {
        shield += amount;
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
    
    public void InsertBrain(Brain brain)
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
        shield = 0;
        maxShield = 0;
        abilities = brain.abilities;
    }

    public async Task OnTurnEnd()
    {
        if (health<=0)
        {
            Kill();
        }
        await TickStatusEffects();
        
    }
}
