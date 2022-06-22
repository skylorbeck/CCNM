using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] pauseText;

    void Start()
    {
        pauseText[0].text = GameManager.Instance.battlefield.player.damageBonus.ToString();
        pauseText[1].text = GameManager.Instance.battlefield.player.shieldBonus.ToString();
        pauseText[2].text = GameManager.Instance.battlefield.player.healthBonus.ToString();
        pauseText[3].text = GameManager.Instance.battlefield.player.speedBonus.ToString();
        pauseText[4].text = GameManager.Instance.battlefield.player.skillBonus.ToString();
        foreach (TextMeshProUGUI text in pauseText)
        {
            text.CrossFadeAlpha(0, 0, true);
        }
    }

    public void FadeInOut()
    {
        foreach (TextMeshProUGUI text in pauseText)
        {
            text.CrossFadeAlpha(GameManager.Instance.uiStateObject.isPaused ?1 :0, 0.25f,true);
        }
    }
}
