using System.Threading.Tasks;

public class PlayerShell : Shell
{
    public PlayerBrain playerBrain
    {
        get { return (PlayerBrain)brain; }
        private set => InsertBrain(value);
    }
    public override void InsertBrain(Brain brain)
    {
        base.InsertBrain(brain);
        health = maxHealth += playerBrain.healthBonus;
    }
    public override Task<int> OnAttack(Shell target, int baseDamage)
    {
        baseDamage += playerBrain.damageBonus;
        GameManager.Instance.uiStateObject.Ping("You hit " + target.name + " for " + baseDamage + " damage!");
        return base.OnAttack(target, baseDamage);
    }

    public override void Shield(int amount)
    {
        amount += playerBrain.shieldBonus;
        GameManager.Instance.uiStateObject.Ping("You gained " + amount + " shield!");
        base.Shield(amount);
    }

    public override void Kill()
    {
        GameManager.Instance.uiStateObject.Ping("You died");
        base.Kill();
    }

    public override void Heal(Shell source, int baseHeal, StatusEffect.Element element)
    {
        GameManager.Instance.uiStateObject.Ping("You healed for " + baseHeal + "!");
        base.Heal(source, baseHeal, element);
    }
}