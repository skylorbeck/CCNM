using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TradeHPForShield", menuName = "Cards/Relic/TradeHPForShield")]
public class RelicTradeHpForShield : Relic
{
    public float hpPercent = 0.5f;
    public override void Subscribe(Shell shell)
    {
        
    }
    
    public override void Unsubscribe(Shell shell)
    {
        
    }

    public int GetShieldBonus(int hp)
    {
        return (int)(hp* hpPercent);
    }
    
    public int GetHpPenalty(int hp)
    {
        return (int)(hp* hpPercent);
    }
}
