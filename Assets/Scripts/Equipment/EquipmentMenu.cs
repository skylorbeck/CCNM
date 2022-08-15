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
    [SerializeField] private MicroCard cardMicroPrefab;
    [SerializeField] private EquipmentCardShell cardPrefab;
    [SerializeField] private ItemStatCompare cardCompare;
    
    [SerializeField] private Mode mode = Mode.LeftWheel;

    [SerializeField] private List<List<MicroCard>> menuEntries = new List<List<MicroCard>>();
    [SerializeField] private List<EquipmentCardShell> previews  = new List<EquipmentCardShell>();

    [SerializeField] private float yDistance = 3f;
    [SerializeField] private float xDistance = 1.5f;
    [SerializeField] private float yOffset = 0f;
    [SerializeField] private float xOffset = 0f;
    [SerializeField] private float cardScaleLower = 0.6f;
    [SerializeField] private float cardScaleUpper = 0.9f;
    [SerializeField] private float cardPreviewMax = 1.2f;
    [SerializeField] private bool sticky = false;
    [SerializeField] private float stickiness = 1f;
    [SerializeField] private bool userIsHolding = false;
    [field:SerializeField] public int selected{ get; private set; }
    [field:SerializeField] public int selectedMenu{ get; private set; } 
    [field:SerializeField] public DragMode dragMode{ get; private set; } 
    
    [SerializeField] private Transform cardContainer;
    [SerializeField] private Transform previewContainer;

    async void Start()
    {
        menuEntries = new List<List<MicroCard>>();
        sticky = PlayerPrefs.GetInt("StickyMenu",0) == 1;
        await Task.Delay(10);
        GameManager.Instance.inputReader.Back+=Back;
        Init();
    }

    private void Init()
    {
        GameManager.Instance.inputReader.DragWithContext += OnDrag;
        GameManager.Instance.inputReader.ClickEventWithContext += OnClick;
        GameManager.Instance.inputReader.Point += OnPoint;
        menuEntries = new List<List<MicroCard>>();
        for (var i = 0; i < GameManager.Instance.metaPlayer.playerInventory.Length; i++)
        {
            List<MicroCard> menuEntry = new List<MicroCard>();
            EquipmentList equipmentList = GameManager.Instance.metaPlayer.playerInventory[i];
            
            for (var j = 0; j < equipmentList.container.Count; j++)
            {
                MicroCard card = Instantiate(cardMicroPrefab, cardContainer);
                card.InsertItem(equipmentList.container[j]);
                if (GameManager.Instance.metaPlayer.GetEquippedCard(i).guid ==card.EquipmentData.guid)
                {
                    card.SetHighlighted(true);
                }
                menuEntry.Add(card);
            }
            menuEntries.Add(menuEntry);
            EquipmentCardShell preview = Instantiate(cardPrefab, previewContainer);
            preview.InsertItem(GameManager.Instance.metaPlayer.GetEquippedCard(i));
            preview.SetHighlighted(true);
            
            previews.Add(preview);
        }
        SetSelected(GameManager.Instance.metaPlayer.equippedSlots[selectedMenu]);
        
    }
    
    public void AddEntry(EquipmentDataContainer entry,int menuIndex,int index)
    {
        MicroCard menuEntry = Instantiate(cardMicroPrefab, transform);
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
                xOffset = -selected*xDistance;//which of these goes with which?
                break;
            case Mode.TopWheel:
            case Mode.BottomWheel:
                yOffset = -selected*yDistance;//these are super confusingly labeled
                break;
        }
    }

    public void Equip()
    {
        if (menuEntries[selectedMenu].Count<=0)
        {
            SoundManager.Instance.PlayUiDeny();
            return;
        }
        GameManager.Instance.metaPlayer.Equip(selectedMenu,menuEntries[selectedMenu][selected].EquipmentData);
        for (var i = 0; i < menuEntries.Count; i++)
        {
            List<MicroCard> menuEntry = menuEntries[i];
            for (var j = 0; j < menuEntry.Count; j++)
            {
                MicroCard card = menuEntry[j];
                    

                if (card.EquipmentData.guid == GameManager.Instance.metaPlayer.GetEquippedCard(i).guid)
                {
                    card.SetHighlighted(true);
                    previews[i].InsertItem(card.EquipmentData);
                    previews[i].SetHighlighted(true);
                }
                else
                {
                    card.SetHighlighted(false);
                }
            }
        }
        SoundManager.Instance.PlayUiAccept();
    }
    
    
    void Update()
    {
        if (menuEntries.Count != 0)
        {
            ProcessSelectedMenu();
            if (menuEntries[selectedMenu].Count != 0)
            {
                ProcessSelectedCard();
            }
        }

        CardPosUpdate();
    }

    private void CardPosUpdate()
    {
        if (menuEntries.Count == 0  || previews.Count == 0)
            return;
        for (var i = 0; i < menuEntries.Count; i++)
        {
            List<MicroCard> menuEntry = menuEntries[i];
            for (var index = 0; index < menuEntry.Count; index++)
            {
                Transform entryTransform = menuEntry[index].transform;
                Vector3 entryPosition = entryTransform.localPosition;
                Vector3 entryScale = entryTransform.localScale;
                switch (mode)
                {
                    case Mode.RightWheel:
                    case Mode.LeftWheel:
                        entryPosition.y = (i * yDistance) + yOffset;
                        if (i == selectedMenu)
                        {
                            entryPosition.x = (index * xDistance) + xOffset;
                            // menuEntry[index].Selected = true;

                            if (index == selected)
                            {
                                entryScale = Vector3.Lerp(entryScale, Vector3.one * cardScaleUpper, Time.deltaTime * 5f);
                                entryPosition.z = 0f;
                            }
                            else
                            {
                                entryScale = Vector3.Lerp(entryScale, Vector3.one * cardScaleLower, Time.deltaTime * 5f);
                                entryPosition.z = 1f;
                            }
                        }
                        else
                        {
                            entryPosition.x = Mathf.Lerp(entryPosition.x,
                                (index * xDistance) - (xDistance * GameManager.Instance.metaPlayer.equippedSlots[i]),
                                Time.deltaTime * 5f);
                            entryScale = Vector3.Lerp(entryScale, Vector3.one * cardScaleLower, Time.deltaTime * 5f);
                            entryPosition.z = 0;
                            // menuEntry[index].Selected = false;

                        }
                   
                        break;
                    case Mode.TopWheel:
                    case Mode.BottomWheel:
                        float xTarget =(i * xDistance) + xOffset;
                        float yTarget =(index * yDistance) + yOffset;
                        if (i == selectedMenu)
                        {
                            if (index == selected)
                            {
                                entryScale = Vector3.Lerp(entryScale, Vector3.one * cardScaleUpper, Time.deltaTime * 5f);
                                entryPosition.z = 0f;
                            }
                            else
                            {
                                xTarget -= 1.5f;
                                if ( index == selected-1|| index == selected+1)
                                {
                                    xTarget += 0.65f;
                                }
                                entryScale = Vector3.Lerp(entryScale, Vector3.one * cardScaleLower, Time.deltaTime * 5f);
                                entryPosition.z = 1f;
                            }
                        }
                        else
                        {
                            yTarget = (index * yDistance*0.75f) - (yDistance*0.75f * GameManager.Instance.metaPlayer.equippedSlots[i]);
                            entryScale = Vector3.Lerp(entryScale, Vector3.one * cardScaleLower, Time.deltaTime * 5f);
                            entryPosition.z = 0;
                            // menuEntry[index].Selected = false;
                            xTarget -= 1.5f;

                        }
                        entryPosition.x =Mathf.Lerp(entryPosition.x,xTarget,Time.deltaTime * 5f);   
                        entryPosition.y =Mathf.Lerp(entryPosition.y,yTarget,Time.deltaTime * 5f);

                        break;
                }

                entryTransform.localPosition = entryPosition;
                entryTransform.localScale = entryScale;
            }

            Vector3 transformLocalPosition = previews[i].transform.localPosition;
            Vector3 transformLocalScale = previews[i].transform.localScale;
            switch (mode)
            {
                default:
                case Mode.RightWheel:
                    previewContainer.transform.localPosition = new Vector3(-1.25f, 0, -1);
                    cardContainer.transform.localPosition = new Vector3(1.25f, 0, 0);
                    transformLocalPosition.y = (i * yDistance) + yOffset;
                    transformLocalPosition.x = 0;
                    break;
                case Mode.LeftWheel:
                    previewContainer.transform.localPosition = new Vector3(1.25f, 0, -1);
                    cardContainer.transform.localPosition = new Vector3(-1.25f, 0, 0);
                    transformLocalPosition.y = (i * yDistance) + yOffset;
                    transformLocalPosition.x = 0;
                    break;
                case Mode.TopWheel:
                    previewContainer.transform.localPosition = new Vector3(0,2.75f, -1);
                    cardContainer.transform.localPosition = new Vector3(0, 0, 0);
                    transformLocalPosition.y = 0;
                    transformLocalPosition.x = (i * xDistance) + xOffset;
                    break;
                case Mode.BottomWheel:
                    previewContainer.transform.localPosition = new Vector3(0,-2.75f, -1);
                    cardContainer.transform.localPosition = new Vector3(0, -1.4f, 0);
                    transformLocalPosition.y = 0;
                    transformLocalPosition.x = (i * xDistance) + xOffset;
                    break;
            }
            if (i == selectedMenu)
            {
                // previews[i].Selected = true;
                transformLocalScale = Vector3.Lerp(transformLocalScale, Vector3.one * cardPreviewMax, Time.deltaTime * 5f);
                transformLocalPosition.z = -1.5f;
            }
            else
            {
                // previews[i].Selected = false;
                transformLocalScale = Vector3.Lerp(transformLocalScale, Vector3.one, Time.deltaTime * 5f);
                transformLocalPosition.z = -0.5f;
            }

            previews[i].transform.localPosition = transformLocalPosition;
            previews[i].transform.localScale = transformLocalScale;
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
                target = (float)(Math.Round(xOffset / xDistance, MidpointRounding.AwayFromZero) * xDistance);

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
            cardCompare.InsertItemStats(GameManager.Instance.metaPlayer.EquippedCardExists(selectedMenu)?GameManager.Instance.metaPlayer.GetEquippedCard(selectedMenu):GameManager.Instance.metaPlayer.defaultEquipment[selectedMenu], menuEntries[selectedMenu][newSelected].EquipmentData);
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
        // if (userIsHolding)
        // {
        //     return;
        // }//todo fix this so it works on mobile

        switch (mode)
        {
            default:
            case Mode.RightWheel:
                if (pos.y > Screen.height * 0.35f && pos.y < Screen.height * 0.65f && pos.x > Screen.width*0.5f)
                {
                    dragMode = DragMode.Horizontal;
                }
                else
                {
                    dragMode = DragMode.Vertical;
                }
                break;
            case Mode.LeftWheel:
                if (pos.y > Screen.height * 0.35f && pos.y < Screen.height * 0.65f && pos.x < Screen.width*0.5f)
                {
                    dragMode = DragMode.Horizontal;
                }
                else
                {
                    dragMode = DragMode.Vertical;
                }
                break;
            case Mode.TopWheel:
                if (pos.y < Screen.height * 0.65f)
                {
                    dragMode = DragMode.Vertical;
                }
                else
                {
                    dragMode = DragMode.Horizontal;
                }
                break;
            case Mode.BottomWheel:
                if (pos.y > Screen.height * 0.4f)
                {
                    dragMode = DragMode.Vertical;
                }
                else
                {
                    dragMode = DragMode.Horizontal;
                }
                break;
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
        GameManager.Instance.inputReader.Back-=Back;
        
    }

    public void Back()
    {
        GameManager.Instance.saveManager.SaveRun();
        GameManager.Instance.LoadSceneAdditive("MainMenu","Equipment");
        //todo decide if equipment can be changed mid-run or not;
        /*if (GameManager.Instance.battlefield.runStarted || GameManager.Instance.battlefield.deckChosen)
        {
            GameManager.Instance.LoadSceneAdditive("MapScreen","Equipment");
        }
        else
        {
            GameManager.Instance.LoadSceneAdditive("Hotel","Equipment");
        }*/
    }
}
