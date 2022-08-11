using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GenericMenuV1 : MonoBehaviour
{
    [SerializeField] private MenuEntry menuPrefab;
    [SerializeField] private Mode mode = Mode.LeftWheel;
    [SerializeField] TextMeshProUGUI buttonText;

    [SerializeField] private List<GenericMenuEntry> entries;
    [SerializeField] private List<MenuEntry> menuEntries = new List<MenuEntry>();

    [SerializeField] private float yDistance = 3f;
    [SerializeField] private float xDistance = 1.5f;
    [SerializeField] private float rotationalSpeed = 1f;
    [SerializeField] private float offset = 0f;
    [SerializeField] private bool sticky = false;
    [SerializeField] private float stickiness = 1f;
    [SerializeField] private bool userIsHolding = false;
    [field:SerializeField] public int selected{ get; private set; }
    
    async void Start()
    {
        buttonText.text = entries[selected].name;
        sticky = PlayerPrefs.GetInt("StickyMenu",0) == 1;
        await Task.Delay(10);
        Init();
    }

    private void Init()
    {
        GameManager.Instance.inputReader.DragWithContext += OnDrag;
        GameManager.Instance.inputReader.ClickEventWithContext += OnClick;

        foreach (GenericMenuEntry generic in entries)
        {
            MenuEntry entry = Instantiate(menuPrefab, transform);
            entry.InsertData(generic.description, generic.icon);
            menuEntries.Add(entry);
        }
    }
    
    public void AddEntry(GenericMenuEntry entry,int index)
    {
        MenuEntry menuEntry = Instantiate(menuPrefab, transform);
        menuEntry.InsertData(entry.description, entry.icon);
        entries.Insert(index, entry);
        menuEntries.Insert(index, menuEntry);
    }
    
    public void RemoveEntry(int index)
    {
        Destroy(menuEntries[index].gameObject);
        menuEntries.RemoveAt(index);
    }
    
    public void AddEntry(GenericMenuEntry entry)
    {
        MenuEntry menuEntry = Instantiate(menuPrefab, transform);
        menuEntry.InsertData(entry.description, entry.icon);
        entries.Add(entry);
        menuEntries.Add(menuEntry);
    }
    
    public void SetOffset(float offset)
    {
        this.offset = offset;
    }
    
    
    public void SetStickiness(float stickiness)
    {
        this.stickiness = stickiness;
    }
    
    public void SetSelected(int selected)
    {
        this.selected = selected;
        switch (mode)
        {
            default:
            case Mode.RightWheel:
            case Mode.LeftWheel:
                offset = -selected*yDistance;
                break;
            case Mode.TopWheel:
            case Mode.BottomWheel:
                offset = -selected*xDistance;
                break;
        }
    }

    void Update()
    {
        if (menuEntries.Count == 0)
            return;
        float target;
        int newSelected;
        switch (mode)
        {
            /*case Mode.FullWheel:
                target = (float)(Math.Round(offset / menuEntries.Count, MidpointRounding.AwayFromZero)) *
                         (menuEntries.Count + 0.25f);
                
                if (sticky || !userIsHolding)
                {
                    offset = Mathf.Lerp(offset, target, Time.deltaTime * menuEntries.Count);
                }

                newSelected = (int)Math.Round(offset / menuEntries.Count, MidpointRounding.AwayFromZero);

                break;*/
            default:
            case Mode.LeftWheel:
            case Mode.RightWheel:
                target = (float)(Math.Round(offset / yDistance, MidpointRounding.AwayFromZero) * yDistance);
                
                if (sticky || !userIsHolding)
                {
                    offset = Mathf.Lerp(offset, target, Time.deltaTime * yDistance * stickiness);
                }
                
                offset = Mathf.Clamp(offset, yDistance - menuEntries.Count * yDistance, 0);
                
                newSelected = Mathf.Abs((int)Math.Round(offset / yDistance, MidpointRounding.AwayFromZero));
                break;
            case Mode.TopWheel:
            case Mode.BottomWheel:
                target = (float)(Math.Round(offset / xDistance, MidpointRounding.AwayFromZero) * xDistance);
                
                if (sticky || !userIsHolding)
                {
                    offset = Mathf.Lerp(offset, target, Time.deltaTime * xDistance * stickiness);
                }
                
                offset = Mathf.Clamp(offset, xDistance - menuEntries.Count * xDistance, 0);

                newSelected = Mathf.Abs((int)Math.Round(offset / xDistance, MidpointRounding.AwayFromZero));
                break;
        }


        UpdateSelected(newSelected);

        for (var i = 0; i < menuEntries.Count; i++)
        {
            MenuEntry menuEntry = menuEntries[i];
            Transform entryTransform = menuEntry.transform;
            Vector3 entryPosition = entryTransform.position;

            switch (mode)
            {
                /*case Mode.FullWheel:
                    float angle = i * Mathf.PI * 2 / menuEntries.Count;
                    angle += offset / menuEntries.Count;
                    entryPosition.y = Mathf.Sin(angle) * yDistance;
                    entryPosition.x = Mathf.Cos(angle) * xDistance;
                    break;*/
                case Mode.RightWheel:
                    entryPosition.y = (i * yDistance) + offset;
                    entryPosition.x = Mathf.Cos(Mathf.Abs(entryPosition.y * 0.5f)) * xDistance;
                    entryTransform.rotation = Quaternion.Euler(0, 0, entryPosition.y * rotationalSpeed);
                    break;
                case Mode.LeftWheel:
                    entryPosition.y = (i * yDistance) + offset;
                    entryPosition.x = -Mathf.Cos(Mathf.Abs(entryPosition.y * 0.5f)) * xDistance;
                    entryTransform.rotation = Quaternion.Euler(0, 0, entryPosition.y * rotationalSpeed);
                    break;
                case Mode.TopWheel:
                    entryPosition.x = (i * xDistance) + offset;
                    entryPosition.y = Mathf.Cos(Mathf.Abs(entryPosition.x * 0.5f)) * yDistance;
                    entryTransform.rotation = Quaternion.Euler(0, 0, entryPosition.x * rotationalSpeed);
                    break;
                case Mode.BottomWheel:
                    entryPosition.x = (i * xDistance) + offset;
                    entryPosition.y = -Mathf.Cos(Mathf.Abs(entryPosition.x * 0.5f)) * yDistance;
                    entryTransform.rotation = Quaternion.Euler(0, 0, entryPosition.x * rotationalSpeed);
                    break;
            }

            entryTransform.localPosition = entryPosition;
        }
    }

    void UpdateSelected(int newSelected)
    {
        if (newSelected != selected)
        {
            selected = newSelected;
            buttonText.text = entries[selected].name;
            SoundManager.Instance.PlayUiClick();
        }
    }

    public void OnClick(InputAction.CallbackContext context)
    { 
        if (context.started)
        {
            userIsHolding = true;
        }

        if (context.canceled)
        {
            userIsHolding = false;
        }
    }
    public void OnDrag(InputAction.CallbackContext context)
    {
       
        if (context.performed)
        {
            Vector2 delta = context.ReadValue<Vector2>();
            switch (mode)
            {
                case Mode.TopWheel:
                case Mode.BottomWheel:
                    offset += delta.x * Time.deltaTime * PlayerPrefs.GetFloat("TouchSensitivity", 1f);
                    break;
                case Mode.LeftWheel:
                case Mode.RightWheel:
                    offset += delta.y * Time.deltaTime * PlayerPrefs.GetFloat("TouchSensitivity", 1f);
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.inputReader.DragWithContext -= OnDrag;
        GameManager.Instance.inputReader.ClickEventWithContext -= OnClick;
    }

    public enum Mode
    {
        RightWheel,
        LeftWheel,
        TopWheel,
        BottomWheel,
    }
    
    public void InvokeSelected()
    {
        entries[selected].method.Invoke();
    }
}
[Serializable]
public class GenericMenuEntry
{
    public string name;
    public string description;
    public Sprite icon;
    public UnityEvent method;
}
