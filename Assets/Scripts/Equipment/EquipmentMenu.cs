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
using Random = UnityEngine.Random;

public class EquipmentMenu : MonoBehaviour
{
    [SerializeField] private MicroCard cardMicroPrefab;
    [SerializeField] private EquipmentCardShell cardPrefab;
    [SerializeField] private ItemStatCompare cardCompare;
    
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

    public TMP_Dropdown sortDropdown;

    async void Start()
    {
        menuEntries = new List<List<MicroCard>>();
        sticky = PlayerPrefs.GetInt("StickyMenu",0) == 1;
        await Task.Delay(10);
        GameManager.Instance.inputReader.Back+=Back;
        Init();
        GameManager.Instance.uiStateObject.Ping("Manage Equipment");
        await Task.Delay(10);
        cardCompare.InsertItemStats(
            GameManager.Instance.metaPlayer.EquippedCardExists(0)?
                GameManager.Instance.metaPlayer.GetEquippedCard(0):GameManager.Instance.metaPlayer.defaultEquipment[0], 
            menuEntries[0][selected].EquipmentData);
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

        sortDropdown.value = PlayerPrefs.GetInt("EquipmentSortMode",0);
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
        yOffset = -selected*yDistance;

    }
    
    public void SetSortMode()
    {
        foreach (List<MicroCard> list in menuEntries)
        {
            switch (sortDropdown.value)
            {
                default:
                    list.Sort((card1, card2) => Random.Range(-1, 1));
                    break;
                case 1:
                    //sort by quality then level
                    list.Sort((card1, card2) =>
                        {
                            if (card1.EquipmentData.quality == card2.EquipmentData.quality)
                            {
                                return card1.EquipmentData.level.CompareTo(card2.EquipmentData.level)*-1;
                            }
                            return card1.EquipmentData.quality.CompareTo(card2.EquipmentData.quality)*-1;
                        }
                    );
                    break;
                case 2:
                    list.Sort((card1, card2) => card1.EquipmentData.level.CompareTo(card2.EquipmentData.level)*-1);
                    break;
                case 3:
                    list.Sort((card1, card2) => card1.EquipmentData.gemSlots.CompareTo(card2.EquipmentData.gemSlots)*-1);
                    break;
                case 4:
                    list.Sort((card1, card2) =>
                        {
                            if (card1.EquipmentData.quality == card2.EquipmentData.quality)
                            {
                                return card1.EquipmentData.level.CompareTo(card2.EquipmentData.level)*-1;
                            }
                            return card1.EquipmentData.quality.CompareTo(card2.EquipmentData.quality);
                        }
                    );
                    break;
                case 5:
                    list.Sort((card1, card2) => card1.EquipmentData.level.CompareTo(card2.EquipmentData.level));
                    break;
                case 6:
                    list.Sort((card1, card2) => card1.EquipmentData.gemSlots.CompareTo(card2.EquipmentData.gemSlots));
                    break;
            }
            
        }
        PlayerPrefs.SetInt("EquipmentSortMode",sortDropdown.value);
        SetSelected( menuEntries[selectedMenu].FindIndex(x => x.EquipmentData.Equals(GameManager.Instance.metaPlayer.GetEquippedCard(selectedMenu))));

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
        cardCompare.InsertItemStats(GameManager.Instance.metaPlayer.EquippedCardExists(selectedMenu)?GameManager.Instance.metaPlayer.GetEquippedCard(selectedMenu):GameManager.Instance.metaPlayer.defaultEquipment[selectedMenu], menuEntries[selectedMenu][selected].EquipmentData);

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
                    yTarget = (index * yDistance*0.75f) - (yDistance*0.75f * menuEntries[i].FindIndex(x => x.EquipmentData.Equals(GameManager.Instance.metaPlayer.GetEquippedCard(i))));
                    entryScale = Vector3.Lerp(entryScale, Vector3.one * cardScaleLower, Time.deltaTime * 5f);
                    entryPosition.z = 0;
                    // menuEntry[index].Selected = false;
                    xTarget -= 1.5f;

                }
                entryPosition.x =Mathf.Lerp(entryPosition.x,xTarget,Time.deltaTime * 5f);   
                entryPosition.y =Mathf.Lerp(entryPosition.y,yTarget,Time.deltaTime * 5f);


                entryTransform.localPosition = entryPosition;
                entryTransform.localScale = entryScale;
            }

            Vector3 transformLocalPosition = previews[i].transform.localPosition;
            Vector3 transformLocalScale = previews[i].transform.localScale;
            
            previewContainer.transform.localPosition = new Vector3(0,-2.75f, -1);
            cardContainer.transform.localPosition = new Vector3(0, -1.4f, 0);
            transformLocalPosition.y = 0;
            transformLocalPosition.x = (i * xDistance) + xOffset;
            
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
        
        target = (float)(Math.Round(xOffset / xDistance, MidpointRounding.AwayFromZero) * xDistance);

        if (sticky || !userIsHolding)
        {
            xOffset = Mathf.Lerp(xOffset, target, Time.deltaTime * xDistance * stickiness);
        }

        xOffset = Mathf.Clamp(xOffset, xDistance - menuEntries.Count * xDistance, 0);

        newSelected = Mathf.Abs((int)Math.Round(xOffset / xDistance, MidpointRounding.AwayFromZero));

        UpdateSelectedMenu(newSelected);
    }

    private void ProcessSelectedCard()
    {
        float target;
        int newSelected;
        
        target = (float)(Math.Round(yOffset / yDistance, MidpointRounding.AwayFromZero) * yDistance);

        if (sticky || !userIsHolding)
        {
            yOffset = Mathf.Lerp(yOffset, target, Time.deltaTime * yDistance * stickiness);
        }

        yOffset = Mathf.Clamp(yOffset, yDistance - menuEntries[selectedMenu].Count * yDistance, 0);

        newSelected = Mathf.Abs((int)Math.Round(yOffset / yDistance, MidpointRounding.AwayFromZero));

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
            int index = menuEntries[selectedMenu].FindIndex(x =>
                x.EquipmentData.Equals(GameManager.Instance.metaPlayer.GetEquippedCard(selectedMenu)));
            SetSelected(index);
            if (index!=-1)
            {
                cardCompare.InsertItemStats(
                    GameManager.Instance.metaPlayer.EquippedCardExists(newSelected)?
                        GameManager.Instance.metaPlayer.GetEquippedCard(newSelected): GameManager.Instance.metaPlayer.defaultEquipment[newSelected],
                    menuEntries[newSelected][index].EquipmentData);
            }
            SoundManager.Instance.PlayUiClick();
        }
    }

    public void OnPoint(Vector2 pos)
    {
        // if (userIsHolding)
        // {
        //     return;
        // }//todo fix this so it works on mobile

        if (pos.y > Screen.height * 0.4f)
        {
            dragMode = DragMode.Vertical;
        }
        else
        {
            dragMode = DragMode.Horizontal;
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
