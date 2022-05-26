using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "Combat/StatusEffect/Blank")]
public class StatusEffect : ScriptableObject
{
    [field: SerializeField] public string titleTranslationKey { get; private set; } = "unset";
    [field: SerializeField] public string descriptionTranslationKey { get; private set; } = "unset.description";
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public int duration { get; private set; } = 1;
    [field: SerializeField] public int maxStacks { get; private set; } = 0;
    [field: SerializeField] public bool isPersistent { get; private set; } = false;
    [field: SerializeField] public bool isStackable { get; private set; } = false;
    [field: SerializeField] public bool isDebuff { get; private set; } = false;
    [field: SerializeField] public bool isBuff { get; private set; } = false;
    [field: SerializeField] public Element element { get; private set; } = Element.None;

    public bool isElemental => element != Element.None;


    //called before onDamage and returns modified damage done
    public virtual async Task<int> OnAttack(Shell target, Shell attacker, int baseDamage)
    {
        await Task.Delay(100);
        //do something
        return baseDamage;
    }
    //called after OnAttack and returns modified damage taken
    public virtual async Task<int> OnDamage([CanBeNull] Shell attacker, Shell defender, int baseDamage)
    {
        await Task.Delay(100);
        //do something
        return baseDamage;
    }
    //called when status effect is applied to a shell
    public virtual async void OnApply(Shell target)
    {
        await Task.Delay(100);
        //do something to the shell
    }
    //called when status effect is removed from a shell
    public virtual async void OnRemove(Shell target)
    {
        await Task.Delay(100);
        //do something to the shell
    }
    //called at the end of the users turn
    public virtual async Task Tick(Shell target)
    {
        //todo animation call
        await Task.Delay(100);
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
