using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleTextPopController : MonoBehaviour
{
    public Shadow shadow;
    public float distance = 10;
    void Start()
    {
        shadow = GetComponent<Shadow>();
    }

    void Update()
    {
        shadow.effectDistance = new Vector2(0, transform.position.z-distance);
    }

    void FixedUpdate()
    {
        
    }
}
