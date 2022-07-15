using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLink : MonoBehaviour
{
    public string scene;
    public Sprite sprite;
    public Image image;
    public string text;

    private void Start()
    {
        image.sprite = sprite;
    }
}
