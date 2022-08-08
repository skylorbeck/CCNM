using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using static GenericMenuV1;

public class EquipmentMenu : MonoBehaviour
{
    [SerializeField] private EquipmentCardShell cardPrefab;
    [SerializeField] private Mode mode = Mode.LeftWheel;

    [SerializeField] private List<List<EquipmentCardShell>> menuEntries = new List<List<EquipmentCardShell>>();

    [SerializeField] private float yDistance = 3f;
    [SerializeField] private float xDistance = 1.5f;
    [SerializeField] private float yOffset = 0f;
    [SerializeField] private float xOffset = 0f;
    [SerializeField] private bool sticky = false;
    [SerializeField] private float stickiness = 1f;
    [SerializeField] private bool userIsHolding = false;
    [field:SerializeField] public int selected{ get; private set; }
    [field:SerializeField] public int selectedMenu{ get; private set; } 
    [field:SerializeField] public DragMode dragMode{ get; private set; } 
    
    [SerializeField] private Transform cardContainer;

    async void Start()
    {
        menuEntries = new List<List<EquipmentCardShell>>();
        sticky = PlayerPrefs.GetInt("StickyMenu",0) == 1;
        await Task.Delay(10);
        Init();
    }

    private void Init()
    {
        GameManager.Instance.inputReader.DragWithContext += OnDrag;
        GameManager.Instance.inputReader.ClickEventWithContext += OnClick;
        GameManager.Instance.inputReader.Point += OnPoint;
        menuEntries = new List<List<EquipmentCardShell>>();
        for (var i = 0; i < GameManager.Instance.metaPlayer.playerInventory.Length; i++)
        {
            List<EquipmentCardShell> menuEntry = new List<EquipmentCardShell>();
            EquipmentList equipmentList = GameManager.Instance.metaPlayer.playerInventory[i];
            
            for (var j = 0; j < equipmentList.container.Count; j++)
            {
                EquipmentCardShell card = Instantiate(cardPrefab, cardContainer);
                card.InsertItem(equipmentList.container[j]);
                menuEntry.Add(card);
            }
            menuEntries.Add(menuEntry);
        }
    }
    
    public void AddEntry(EquipmentDataContainer entry,int menuIndex,int index)
    {
        EquipmentCardShell menuEntry = Instantiate(cardPrefab, transform);
        menuEntry.InsertItem(entry);
        menuEntries[menuIndex].Insert(index, menuEntry);
    }

    public void SetXOffset(float offset)
    {
        this.xOffset = offset;
    }
    public void SetYOffset(float offset)
    {
        this.yOffset = offset;
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
                xOffset = -selected*yDistance;
                break;
            case Mode.TopWheel:
            case Mode.BottomWheel:
                yOffset = -selected*xDistance;
                break;
        }
    }

    void Update()
    {
        if (menuEntries.Count == 0)
            return;
        
        ProcessSelectedMenu();
        ProcessSelectedCard();

        for (var i = 0; i < menuEntries.Count; i++)
        {
            List<EquipmentCardShell> menuEntry = menuEntries[i];
            for (var index = 0; index < menuEntry.Count; index++)
            {

                Transform entryTransform = menuEntry[index].transform;
                Vector3 entryPosition = entryTransform.position;

                switch (mode)
                {
                    case Mode.RightWheel:
                        entryPosition.y = (i * yDistance) + yOffset;
                        if (i == selectedMenu)
                        {
                            entryPosition.x = (index * xDistance) +xOffset;
                        }
                        break;
                    case Mode.LeftWheel:
                        entryPosition.y = (i * yDistance) + xOffset;
                        entryPosition.x = -Mathf.Cos(Mathf.Abs(entryPosition.y * 0.5f)) * xDistance;
                        break;
                    case Mode.TopWheel:
                        entryPosition.x = (i * xDistance) + xOffset;
                        entryPosition.y = Mathf.Cos(Mathf.Abs(entryPosition.x * 0.5f)) * yDistance;
                        break;
                    case Mode.BottomWheel:
                        entryPosition.x = (i * xDistance) + xOffset;
                        entryPosition.y = -Mathf.Cos(Mathf.Abs(entryPosition.x * 0.5f)) * yDistance;
                        break;
                }

                entryTransform.localPosition = entryPosition;
            }
        }
    }

    private void ProcessSelectedMenu()
    {
        float target;
        int newSelected;
        switch (mode)
        {
            default:
            case Mode.LeftWheel:
            case Mode.RightWheel:
                target = (float)(Math.Round(yOffset / yDistance, MidpointRounding.AwayFromZero) * yDistance);

                if (sticky || !userIsHolding)
                {
                    yOffset = Mathf.Lerp(yOffset, target, Time.deltaTime * yDistance * stickiness);
                }

                yOffset = Mathf.Clamp(yOffset, yDistance - menuEntries.Count * yDistance, 0);

                newSelected = Mathf.Abs((int)Math.Round(yOffset / yDistance, MidpointRounding.AwayFromZero));
                break;
            case Mode.TopWheel:
            case Mode.BottomWheel:
                target = (float)(Math.Round(xOffset / xDistance, MidpointRounding.AwayFromZero) * xDistance);

                if (sticky || !userIsHolding)
                {
                    xOffset = Mathf.Lerp(xOffset, target, Time.deltaTime * xDistance * stickiness);
                }

                xOffset = Mathf.Clamp(xOffset, xDistance - menuEntries.Count * xDistance, 0);

                newSelected = Mathf.Abs((int)Math.Round(xOffset / xDistance, MidpointRounding.AwayFromZero));
                break;
        }

        UpdateSelectedMenu(newSelected);
    }

    private void ProcessSelectedCard()
    {
        float target;
        int newSelected;
        switch (mode)
        {
            default:
            case Mode.LeftWheel:
            case Mode.RightWheel:
                target = (float)(Math.Round(xOffset / yDistance, MidpointRounding.AwayFromZero) * xDistance);

                if (sticky || !userIsHolding)
                {
                    xOffset = Mathf.Lerp(xOffset, target, Time.deltaTime * xDistance * stickiness);
                }

                xOffset = Mathf.Clamp(xOffset, xDistance - menuEntries[selectedMenu].Count * xDistance, 0);

                newSelected = Mathf.Abs((int)Math.Round(xOffset / xDistance, MidpointRounding.AwayFromZero));
                break;
            case Mode.TopWheel:
            case Mode.BottomWheel:
                target = (float)(Math.Round(yOffset / yDistance, MidpointRounding.AwayFromZero) * yDistance);

                if (sticky || !userIsHolding)
                {
                    yOffset = Mathf.Lerp(yOffset, target, Time.deltaTime * yDistance * stickiness);
                }

                yOffset = Mathf.Clamp(yOffset, yDistance - menuEntries[selectedMenu].Count * yDistance, 0);

                newSelected = Mathf.Abs((int)Math.Round(yOffset / yDistance, MidpointRounding.AwayFromZero));
                break;
        }

        UpdateSelected(newSelected);
    }

    void UpdateSelected(int newSelected)
    {
        if (newSelected != selected)
        {
            selected = newSelected;
            SoundManager.Instance.PlayUiClick();
        }
    }

    void UpdateSelectedMenu(int newSelected)
    {
        if (newSelected != selectedMenu)
        {
            selectedMenu = newSelected;
            SetSelected(GameManager.Instance.metaPlayer.equippedSlots[selectedMenu]);
            SoundManager.Instance.PlayUiClick();
        }
    }

    public void OnPoint(Vector2 pos)
    {
        Debug.Log(pos);
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
            if (delta.magnitude > 0.1f)
            {
                userIsHolding = true;
                dragMode = DragMode.None;
            }
            else
            {
                userIsHolding = false;
            }
            switch (dragMode)
            {
                default:
                case DragMode.Vertical:
                    yOffset += delta.y * Time.deltaTime * PlayerPrefs.GetFloat("TouchSensitivity", 1f);
                    break;
                case DragMode.Horizontal:
                    xOffset += delta.x * Time.deltaTime * PlayerPrefs.GetFloat("TouchSensitivity", 1f);
                    break;
            }
            /*switch (mode)
            {
                case Mode.TopWheel:
                case Mode.BottomWheel:
                    
                    break;
                case Mode.LeftWheel:
                case Mode.RightWheel:
                    xOffset += delta.y * Time.deltaTime * PlayerPrefs.GetFloat("TouchSensitivity", 1f);
                    break;
            }*/
        }
        
    }

    public enum DragMode
    {
        None,
        Horizontal,
        Vertical
    }
    private void OnDestroy()
    {
        GameManager.Instance.inputReader.DragWithContext -= OnDrag;
        GameManager.Instance.inputReader.ClickEventWithContext -= OnClick;
        GameManager.Instance.inputReader.Point -= OnPoint;
    }

    
}
