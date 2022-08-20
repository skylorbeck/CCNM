using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableMenu : MonoBehaviour
{
    public bool isOpen { get; private set; }
    [SerializeField] private Transform backgroundLeft;
    [SerializeField] private Transform backgroundRight;
    [SerializeField] private Button buttonLeft;
    [SerializeField] private Button buttonRight;
    [SerializeField] private Button[] consumableButtons;
    [SerializeField] private TextMeshPro[] consumableValues;
    [SerializeField] private PlayerShell playerShell;
    [SerializeField] private SpriteRenderer[] consumableSprites;
    void Start()
    {
        UpdateTexts();
        UpdateIcons();
    }

    public void UpdateTexts()
    {
        for (int i = 0; i < consumableButtons.Length; i++)
        {
            consumableValues[i].text = "x"+ GameManager.Instance.runPlayer.consumables[i];
        }
    }

    public void UpdateIcons()
    {
        for (int i = 0; i < consumableSprites.Length; i++)
        {
            consumableSprites[i].color = GameManager.Instance.runPlayer.consumables[i] > 0 ? Color.white : Color.gray;
        }
    }
    
    public void OnDestroy()
    {
    }

    void Update()
    {
        backgroundLeft.transform.localPosition = Vector3.Lerp(backgroundLeft.transform.localPosition, new Vector3(isOpen? -1.5f:-5, -0.85f, 0), Time.deltaTime * 10);
        backgroundRight.transform.localPosition = Vector3.Lerp(backgroundRight.transform.localPosition, new Vector3(isOpen? 1.5f:5, -0.85f, 0), Time.deltaTime * 10);

    }

    void FixedUpdate()
    {
        
    }
    
    public void ToggleMenu()
    {
        isOpen = !isOpen;
        foreach (Button button in consumableButtons)
        {
            button.interactable = isOpen;
        }
    }
    public void Back()
    {
        
        buttonLeft.interactable = !buttonLeft.interactable;
        buttonRight.interactable = !buttonRight.interactable;
    }

    public void Pie()
    {
        if (GameManager.Instance.runPlayer.consumables[1]>0)
        {
            playerShell.Heal(playerShell.brain.GetHealthMax(),StatusEffect.Element.None);
            GameManager.Instance.runPlayer.consumables[1]--;
            UpdateIcons();
            UpdateTexts();
            ToggleMenu();
        }
       
    }

    public void Coffee()
    {
        if (GameManager.Instance.runPlayer.consumables[0]>0)
        {
            playerShell.Shield(playerShell.brain.GetShieldMax(),StatusEffect.Element.None);
            GameManager.Instance.runPlayer.consumables[0]--;
            UpdateIcons();
            UpdateTexts();
            ToggleMenu();
        }
    }

    public void Tea()
    {
        if (GameManager.Instance.runPlayer.consumables[2]>0)
        {
            playerShell.statusDisplayer.Clear();
            GameManager.Instance.uiStateObject.Ping("Cleared Status Effects");
            TextPopController.Instance.PopPositive("Cleansed",playerShell.transform.position,true);
            GameManager.Instance.runPlayer.consumables[2]--;
            UpdateIcons();
            UpdateTexts();
            ToggleMenu();
        }
        
        
      
    }
}
