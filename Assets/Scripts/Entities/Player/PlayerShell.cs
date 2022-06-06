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
        health = maxHealth = playerBrain.healthBonus;
    }
    public override Task<int> OnAttack(Shell target, int baseDamage)
    {
        baseDamage += playerBrain.damageBonus;
        return base.OnAttack(target, baseDamage);
    }

    public override void Shield(int amount)
    {
        amount += playerBrain.shieldBonus;
        base.Shield(amount);
    }
}