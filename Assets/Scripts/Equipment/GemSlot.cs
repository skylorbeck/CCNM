using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GemSlot : MonoBehaviour
{
    [field: SerializeField] public bool isFront { get; set; } = false;
    [field: SerializeField] public SpriteRenderer slotRenderer { get; set; }
    [field: SerializeField] public GemShell gemShell { get; set; }
    [field: SerializeField] public SpriteRenderer lockRenderer { get; set; }
    [field: SerializeField] public TextMeshPro text { get; set; }


    public void SetQuality(EquipmentDataContainer.Quality quality)
    {
        slotRenderer.color = GameManager.Instance.colors[(int)quality];
        lockRenderer.color = GameManager.Instance.colors[(int)quality];
    }
    public void SetGem(AbilityGem gem)
    {
        gemShell.InsertAbility(gem);
        if (isFront)
        {
            return;
        }
        text.text = gem.GetAbility().title;
    }

    public void ClearGem()
    {
        gemShell.ClearAbility();
        if (isFront)
        {
            return;
        }
        text.text ="Open Slot";
    }

    public void SetLock(bool isLocked)
    {
        lockRenderer.enabled = isLocked;
    }


    void Start()
    {

    }

    void Update()
    {

    }

    void FixedUpdate()
    {

    }
}
