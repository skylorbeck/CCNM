using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
[CreateAssetMenu(fileName = "ConsumableCard", menuName = "Cards/ConsumableCard")]
public class ConsumableCard : CardObject
{
    public new CardType cardType { get; protected set; } = CardType.Consumable;

    public async Task Consume()
    {
        //todo
    }

    public enum ConsumableType
    {
        Potion,
        Scroll,
        Food,
        Other
    }
}