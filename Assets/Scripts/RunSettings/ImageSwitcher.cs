using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ImageSwitcher : MonoBehaviour
{
    public Sprite[] sprites;
    [SerializeField] private Image image;
    public int currentState { get; private set; } = 0;


    public void NextSprite()
    {
        currentState++;
        if (currentState >= sprites.Length)
        {
            currentState = 0;
        }
        image.sprite = sprites[currentState];
    }
}
