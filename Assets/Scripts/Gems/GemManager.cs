using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using static GenericMenuV1.Mode;

public class GemManager : MonoBehaviour
{
 [SerializeField] private GemShell gemShellPrefab;
    [SerializeField] private EquipmentCardShell cardPrefab;
    
    [SerializeField] private List<GemShell> menuEntries = new List<GemShell>();
    [SerializeField] private List<EquipmentCardShell> previews  = new List<EquipmentCardShell>();
    [SerializeField] private ItemStatCompare cardCompare;

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
    [field:SerializeField] public int selectedGem{ get; private set; }
    [field:SerializeField] public int selectedMenu{ get; private set; } 
    [field:SerializeField] public DragMode dragMode{ get; private set; } 
    
    [SerializeField] private Transform cardContainer;
    [SerializeField] private Transform previewContainer;
    [SerializeField] private SpriteRenderer selector;

    async void Start()
    {
        menuEntries = new List<GemShell>();
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
        for (var i = 0; i < GameManager.Instance.metaPlayer.ownedGems.Length; i++)
        {
            AbilityGem abilityGem = GameManager.Instance.metaPlayer.ownedGems[i];

            if (abilityGem.abilityIndex!=-1)
            {
                GemShell gem = Instantiate(gemShellPrefab, cardContainer);
                gem.InsertAbility(abilityGem);
                menuEntries.Add(gem);
            }
        }
        for (var i = 0; i < GameManager.Instance.metaPlayer.playerInventory.Length; i++)
        {
            EquipmentCardShell preview = Instantiate(cardPrefab, previewContainer);
            preview.InsertItem(GameManager.Instance.metaPlayer.GetEquippedCard(i));
            preview.SetHighlighted(true);
            previews.Add(preview);
        }
      
        cardCompare.InsertAbilityGem(menuEntries[selected].ability);

    }

    public void SelectGemSlot(int slot)
    {
        if (slot < 0 || slot >= previews[selectedMenu].EquipmentData.gemSlots || slot == selectedGem)
        {
            selectedGem = -1;
            selector.transform.position = new Vector3(-1000, -1000, -1000);
            cardCompare.Clear();
            return;
        }
        selectedGem = slot;
        selector.transform.position = new Vector3(1.4f,1.425f-(slot*1.325f),0);
        cardCompare.InsertAbilityGem(GameManager.Instance.metaPlayer.GetEquippedCard(selectedMenu).GetAbility(selectedGem));

    }
    
    public void AddEntry(AbilityGem entry,int index)
    {
        GemShell menuEntry = Instantiate(gemShellPrefab, transform);
        menuEntry.InsertAbility(entry);
        menuEntries.Insert(index, menuEntry);
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
        xOffset = -selected*xDistance;//which of these goes with which?

    }

    public void Equip()
    {
        if (menuEntries.Count<=0)
        {
            SoundManager.Instance.PlayUiDeny();
            TextPopController.Instance.PopNegative("No Gems",Vector3.zero, false);
            return;
        }
        if (selectedGem==-1)
        {
            SoundManager.Instance.PlayUiDeny();
            TextPopController.Instance.PopNegative("No Slot Selected",Vector3.zero, false);
            return;
        }
        bool success = GameManager.Instance.metaPlayer.GetEquippedCard(selectedMenu).InsertAbility(menuEntries[selected].ability,selectedGem);
        if (success)
        {
            previews[selectedMenu].InsertItem(GameManager.Instance.metaPlayer.GetEquippedCard(selectedMenu));
            SoundManager.Instance.PlayUiAccept();
            GameManager.Instance.metaPlayer.RemoveGem(selected,1);
            menuEntries[selected].InsertAbility(GameManager.Instance.metaPlayer.ownedGems[selected]);
            GameManager.Instance.saveManager.SaveMeta();
        }
        else
        {
            TextPopController.Instance.PopNegative("Gem Slot Full",Vector3.zero, false);
            SoundManager.Instance.PlayUiDeny();
        }
    }

    public void Unsocket()
    {
        if (selectedGem==-1)
        {
            SoundManager.Instance.PlayUiDeny();
            TextPopController.Instance.PopNegative("No Slot Selected",Vector3.zero, false);
            return;
        }
        EquipmentDataContainer card = GameManager.Instance.metaPlayer.GetEquippedCard(selectedMenu);

        if (card.indestructible)
        {
            SoundManager.Instance.PlayUiDeny();
            TextPopController.Instance.PopNegative("It's Stuck",Vector3.zero, false);
            return;
        }
        AbilityGem gem = card.GetAbility(selectedGem);

        if (gem == null || gem.abilityIndex == -1)
        {
            SoundManager.Instance.PlayUiDeny();
            TextPopController.Instance.PopNegative("No Gem Socketed",Vector3.zero, false);
            return;
        }

        int totalGems = 0;
        for (int i = 0; i < card.gemSlots; i++)
        {
            totalGems += card.GetAbility(i) != null && card.GetAbility(i).abilityIndex!=-1 ? 1 : 0;
        }

        if (totalGems <=1)
        {
            SoundManager.Instance.PlayUiDeny();
            TextPopController.Instance.PopNegative("You Cannot Remove",Vector3.zero, false);
            TextPopController.Instance.PopNegative("The Last Gem",new Vector3(0,-0.5f,0), false);
            return;
        }


        if (GameManager.Instance.metaPlayer.cardSouls[(int)card.quality]>=card.level)
        {
            bool success = card.RemoveAbility(selectedGem);
            if (success)
            {
                previews[selectedMenu].InsertItem(GameManager.Instance.metaPlayer.GetEquippedCard(selectedMenu));
                SoundManager.Instance.PlayUiAccept();
                GameManager.Instance.metaPlayer.AddAbilityGem(gem);
                
                for (var j = 0; j < menuEntries.Count; j++)
                {
                    if (menuEntries[j].ability.abilityIndex == gem.abilityIndex && menuEntries[j].ability.gemLevel == gem.gemLevel)
                    {
                        menuEntries[j].InsertAbility(GameManager.Instance.metaPlayer.ownedGems[j]);
                        break;
                    }
                }
                GameManager.Instance.metaPlayer.SpendSouls((int)card.quality, card.level);
                GameManager.Instance.saveManager.SaveMeta();
            }
        }
        else
        {
            SoundManager.Instance.PlayUiDeny();
            TextPopController.Instance.PopNegative("Not Enough "+card.quality+" Souls",Vector3.zero, false);
        }
        
      
    }
    
    void Update()
    {
        if (menuEntries.Count != 0)
        {
            ProcessSelectedGem();
        }
        if (previews.Count != 0)
        {
            ProcessSelectedMenu();
        }

        CardPosUpdate();
    }

    private void CardPosUpdate()
    {
        if (menuEntries.Count == 0)
            return;
        for (var i = 0; i < menuEntries.Count; i++)
        {
            GemShell menuEntry = menuEntries[i];

            Transform entryTransform = menuEntry.transform;
            Vector3 entryPosition = entryTransform.localPosition;
            Vector3 entryScale = entryTransform.localScale;

            float yTarget = (i * yDistance) + xOffset;
            float xTarget = (xDistance) /*+ xOffset*/;

            if (i == selected)
            {
                entryScale = Vector3.Lerp(entryScale, Vector3.one * cardScaleUpper, Time.deltaTime * 5f);
                entryPosition.z = 0f;
            }
            else
            {
                xTarget -= 1.5f;
                if (i == selected - 1 || i == selected + 1)
                {
                    xTarget += 0.65f;
                }

                entryScale = Vector3.Lerp(entryScale, Vector3.one * cardScaleLower, Time.deltaTime * 5f);
                entryPosition.z = 1f;
            }

            entryPosition.x = Mathf.Lerp(entryPosition.x, xTarget, Time.deltaTime * 5f);
            entryPosition.y = Mathf.Lerp(entryPosition.y, yTarget, Time.deltaTime * 5f);

            entryTransform.localPosition = entryPosition;
            entryTransform.localScale = entryScale;


        }

        for (int i = 0; i < 3; i++)
        {
            if (previews.Count == 0)
            {
                return;
            }

            Vector3 transformLocalPosition = previews[i].transform.localPosition;
            Vector3 transformLocalScale = previews[i].transform.localScale;

            previewContainer.transform.localPosition = new Vector3(3.5f, 0, -1);
            cardContainer.transform.localPosition = new Vector3(-1.25f, 0, 0);
            transformLocalPosition.y = (i * yDistance*5) + yOffset;
            transformLocalPosition.x = 0;

            if (i == selectedMenu)
            {
                // previews[i].Selected = true;
                transformLocalScale = Vector3.Lerp(transformLocalScale, Vector3.one * cardPreviewMax,
                    Time.deltaTime * 5f);
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
        float yDistance = this.yDistance*5f;
        
        target = (float)(Math.Round(yOffset / yDistance, MidpointRounding.AwayFromZero) * yDistance);

        if (sticky || !userIsHolding)
        {
            yOffset = Mathf.Lerp(yOffset, target, Time.deltaTime * yDistance * stickiness);
        }

        yOffset = Mathf.Clamp(yOffset, yDistance - 3 * yDistance, 0);

        newSelected = Mathf.Abs((int)Math.Round(yOffset / yDistance, MidpointRounding.AwayFromZero));

        UpdateSelectedMenu(newSelected);
    }

    private void ProcessSelectedGem()
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

        UpdateSelected(newSelected);
    }

    void UpdateSelected(int newSelected)
    {
        if (newSelected != selected)
        {
            selected = newSelected;
            cardCompare.InsertAbilityGem(menuEntries[selected].ability);
            SoundManager.Instance.PlayUiClick();
        }
    }

    void UpdateSelectedMenu(int newSelected)
    {
        if (newSelected != selectedMenu)
        {
            selectedMenu = newSelected;
            SelectGemSlot(-1);
            SoundManager.Instance.PlayUiClick();
        }
    }

    public void OnPoint(Vector2 pos)
    {
        // if (userIsHolding)
        // {
        //     return;
        // }//todo fix this so it works on mobile

        if (pos.x > Screen.width*0.5f)
        {
            dragMode = DragMode.RightHalf;
        }
        else
        {
            dragMode = DragMode.LeftHalf;
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
                case DragMode.RightHalf:
                    yOffset += delta.y * Time.deltaTime * PlayerPrefs.GetFloat("TouchSensitivity", 1f);
                    break;
                case DragMode.LeftHalf:
                    xOffset += delta.y * Time.deltaTime * PlayerPrefs.GetFloat("TouchSensitivity", 1f);
                    break;
            }
        }
        
    }

    public enum DragMode
    {
        None,
        LeftHalf,
        RightHalf
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
        GameManager.Instance.LoadSceneAdditive("MainMenu","Gems");
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
