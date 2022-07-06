using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "Combat/StatusEffect/Blank")]
public class StatusEffect : ScriptableObject
{
    [field: SerializeField] public string title { get; private set; } = "unset";
    [field: SerializeField] public string description { get; private set; } = "unset.description";
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public int duration { get; private set; } = 1;
    [field: SerializeField] public int maxStacks { get; private set; } = 0;
    [field: SerializeField] public bool isPersistent { get; private set; } = false;
    [field: SerializeField] public bool isStackable { get; private set; } = false;
    [field: SerializeField] public bool isDebuff { get; private set; } = false;
    [field: SerializeField] public bool isHidden { get; private set; } = false;
    [field: SerializeField] public bool alwaysExpires { get; private set; } = false;
    [field: SerializeField] public Element element { get; private set; } = Element.None;

    public bool isElemental => element != Element.None;


    //called before onDamage and returns modified damage done
    public virtual async Task<int> OnAttack(Shell target, Shell attacker, int baseDamage)
    {
        //do something
        return baseDamage;
    }
    
    //called after OnAttack and returns modified damage taken
    public virtual async Task<int> OnDamage([CanBeNull] Shell attacker, Shell defender, int baseDamage)
    {
        //do something
        return baseDamage;
    }
    public virtual async Task<int> OnDodge([CanBeNull] Shell attacker, Shell defender, int baseDamage)
    {
        //do something
        return baseDamage;
    }
    public virtual async Task<int> OnHeal([CanBeNull] Shell healer, Shell target, int baseHeal)
    {
        //do something
        return baseHeal;
    }
    public virtual async Task<int> OnShield([CanBeNull] Shell shielder, Shell target, int baseShield)
    {
        //do something
        return baseShield;
    }
    
    //called when status effect is applied to a shell
    public virtual async void OnApply(Shell target)
    {
        if (isHidden)
        {
            return;
        }
        if (isDebuff)
        {
            TextPopController.Instance.PopNegative("+"+title, target.transform.position,target.isPlayer);
        } else
        {
            TextPopController.Instance.PopPositive("+"+title, target.transform.position,target.isPlayer);
        }
        //do something to the shell
    }
    
    //called when status effect is removed from a shell
    public virtual async void OnRemove(Shell target)
    {
        /*if (!isDebuff)
        {
            TextPopController.Instance.PopNegative("-"+titleTranslationKey, target.transform.position,target.isPlayer);
        } else
        {
            TextPopController.Instance.PopPositive("-"+titleTranslationKey, target.transform.position,target.isPlayer);
        }*/
        //do something to the shell
    }
    
    //called at the end of the users turn
    public virtual async Task Tick(Shell target)
    {
        //todo animation call
        //do something to the shell
    }
    
    public enum Element
    {
        None,
        Fire,
        Water,
        Earth,
        Air,
    }

    
}
