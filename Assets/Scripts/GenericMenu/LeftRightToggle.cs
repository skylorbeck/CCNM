using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftRightToggle : MonoBehaviour
{
    [SerializeField] public Image toggle;
    [SerializeField] public Sprite left;
    [SerializeField] public Sprite center;
    private int state = 0;
    private int delay=0;
    [SerializeField] int maxDelay=60;

    void Start()
    {
        GameManager.Instance.inputReader.Drag += OnDrag;
    }

    private void OnDrag(Vector2 arg0)
    {
        delay = 0;
        if (arg0.x > 0)
        {
            if (state<0)
            {
                state = 0;
                toggle.sprite = center;
                return;
            }
            state = 1;
            toggle.sprite = left;
            toggle.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (arg0.x < 0)
        {
            if (state>0)
            {
                state = 0;
                toggle.sprite = center;
                return;
            }
            state = -1;
            toggle.sprite = left;
            toggle.transform.localScale = new Vector3(-1, 1, 1);

        }
        
    }


    public void OnDestroy()
    {
        GameManager.Instance.inputReader.Drag -= OnDrag;
    }

    void Update()
    {
        if (state != 0)
        {
            if (delay < maxDelay)
            {
                delay++;
                return;
            }

            delay = 0;
            state = 0;
            toggle.sprite = center;
        }
    }

    void FixedUpdate()
    {
        
    }
}
