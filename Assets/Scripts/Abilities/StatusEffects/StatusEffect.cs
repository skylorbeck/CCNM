using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "Combat/StatusEffect")]
public class StatusEffect : ScriptableObject
{
    [field: SerializeField] public string titleTranslationKey { get; private set; } = "unset";
    [field: SerializeField] public string descriptionTranslationKey { get; private set; } = "unset.description";
    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public int duration { get; private set; }

}
