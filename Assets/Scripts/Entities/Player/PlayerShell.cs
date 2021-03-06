using System.Threading.Tasks;

public class PlayerShell : Shell
{
    #region Event Broadcasts
    public override void OnDied()
    {
    }

    public override void OnShieldRegen()
    {
    }

    public override void OnHealed(int amtHealed)
    {
    }

    public override void OnShieldBreak(int shieldDamTaken)
    {
    }

    public override void OnDamaged(Shell attacker, int damageTaken)
    {
    }

    public override void OnAttacked(Shell target,Symbol symbol)
    {
    }
    
    #endregion
    public PlayerBrain playerBrain
    {
        get { return (PlayerBrain)brain; }
        private set => InsertBrain(value);
    }
    public override void InsertBrain(Brain brain)
    {
        base.InsertBrain(brain);
        SetCurrentHealth(brain.currentHealth);
    }
 
    public override Task<int> OnAttack(Shell target, int baseDamage)
    {
        GameManager.Instance.uiStateObject.Ping("You hit " + target.name + " for " + baseDamage + " damage!");//todo make this reflect base.onattack
        return base.OnAttack(target, baseDamage);
    }
    public override Task<int> OnHeal(Shell target, int baseShield)
    {
        return base.OnShield(target, baseShield);
    }

    public override void Shield(int amount, StatusEffect.Element element)
    {
        GameManager.Instance.uiStateObject.Ping("You gained " + amount + " shield!");
        base.Shield(amount, element);
    }

    public override void Kill()
    {
        GameManager.Instance.uiStateObject.Ping("You died");
        base.Kill();
    }

    public override void Heal(int baseHeal, StatusEffect.Element element)
    {
        GameManager.Instance.uiStateObject.Ping("You healed for " + baseHeal + "!");
        base.Heal(baseHeal, element);
    }
}