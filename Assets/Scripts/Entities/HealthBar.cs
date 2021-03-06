using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Shell shell;
    public LineRenderer healthBar;
    public LineRenderer shieldBar;
    public LineRenderer backBar;
    public LineRenderer backplate;
    public TextMeshPro healthText;
    public TextMeshPro shieldText;
    void Start()
    {
        
    }

    public void ManualUpdate()
    {
        if (shell.isDead)
        {
            healthBar.enabled = false;
            backBar.enabled = false;
            backplate.enabled = false;
        }
        else
        {          
            healthBar.enabled = true;
            backBar.enabled = true;
            backplate.enabled = true;
            healthBar.SetPosition(1, new Vector3( Math.Clamp((float)shell.currentHealth / shell.brain.GetHealthMax(),0,1), 0, 0));
        }
        if (!shell.hasShield || shell.brain.GetShieldMax() == 0) 
        {
            shieldBar.enabled = false;
        }
        else
        {
            shieldBar.enabled = true;
            shieldBar.SetPosition(1, new Vector3(Math.Clamp((float)shell.shield / shell.brain.GetShieldMax(),0,1), 0, 0));
        }

        healthText.text = shell.currentHealth>0? ""+shell.currentHealth:"";
        shieldText.text = shell.shield>0? "{"+shell.shield+"}":"";
    }

    void FixedUpdate()
    {
        
    }
}
