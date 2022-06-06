using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedPreview : MonoBehaviour
{
    public SpriteRenderer background;
    public SpriteRenderer icon;
    public void SetEquipped(EquipmentDataContainer data)
    {
        background.color = GameManager.Instance.colors[(int)data.quality];
        icon.sprite = data.itemCore.icon;
    }
    
    public void Clear()
    {
        background.color = Color.white;
        icon.sprite = null;
    }
}
