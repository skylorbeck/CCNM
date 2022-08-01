using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuEntry : MonoBehaviour
{
    public TextMeshPro TitleText;
    public SpriteRenderer Icon;
    
   public void InsertData(string title, Sprite icon)
    {
        TitleText.text = title;
        Icon.sprite = icon;
    }
}
