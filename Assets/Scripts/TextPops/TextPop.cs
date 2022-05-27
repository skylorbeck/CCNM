using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TextPop : MonoBehaviour
{
    private Animator animator;
    private TextMeshProUGUI text;
    public bool finished {get; private set;} = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    public void Pop(string displayText,PopTypes popType,Vector3 worldPos)
    {
        finished = false;
        if (animator==null)
        {
            animator = GetComponent<Animator>();
            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        // transform.localPosition = Camera.main.WorldToViewportPoint(worldPos);
        transform.position = Camera.main.WorldToScreenPoint(worldPos);
        text.text = displayText;
        switch (popType)
        {
            case PopTypes.Positive:
                animator.SetTrigger("positive");
                break;
            case PopTypes.Negative:
                animator.SetTrigger("negative");
                break;
            case PopTypes.Heal:
                animator.SetTrigger("heal");
                break;
            case PopTypes.Damage:
                animator.SetTrigger("damage");
                break;
            case PopTypes.Shield:
                animator.SetTrigger("shield");
                break;
            case PopTypes.Critical:
                animator.SetTrigger("critical");
                break;
        }
    }

    public void Done()
    {
        finished = true;
    }
    
    public enum PopTypes
    {
        Positive,
        Negative,
        Heal,
        Damage,
        Shield,
        Critical
    }
}
