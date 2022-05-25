using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "Combat/StatusEffect")]
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


    public void OnApply(Shell target)
    {
        //do something to the shell
    }

    public void OnRemove(Shell target)
    {
        //do something to the shell
    }
    
    public void Tick(Shell target)
    {
        //do something to the shell
    }

    public enum Element
    {
        None,
        Fire,
        Water,
        Earth,
        Air,
        Light,
        Dark
    }
}
