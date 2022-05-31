using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwitcher : MonoBehaviour
{
    public Sprite[] sprites;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void SwapSprite(int index)
    {
        spriteRenderer.sprite = sprites[Math.Clamp(index, 0, sprites.Length - 1)];
    }
}
