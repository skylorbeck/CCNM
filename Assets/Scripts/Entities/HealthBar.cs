using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class HealthBar : MonoBehaviour
{
    public Entity entity;
    public LineRenderer healthBar;
    public LineRenderer shieldBar;
    public TextMeshPro healthText;
    public TextMeshPro shieldText;
    void Start()
    {
        
    }

    void Update()
    {
        healthBar.SetPosition(1, new Vector3( Math.Clamp((float)entity.health / entity.maxHealth,0,1), 0, 0));
        shieldBar.SetPosition(1, new Vector3(Math.Clamp((float)entity.shield / entity.maxShield,0,1), 0, 0));
        healthText.text = entity.health.ToString();
        shieldText.text = entity.shield>0? "{"+entity.shield+"}":"";
    }

    void FixedUpdate()
    {
        
    }
}
