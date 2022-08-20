using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private float deathPenaltyRatio = 0.1f;
    [SerializeField] private int score;
    [SerializeField] private int cardPacks;
    [SerializeField] private int highScore;
    [SerializeField] private int credits;
    [SerializeField] private int creditsLost;
    [SerializeField] private int ego;
    [SerializeField] private int egoLost;
    [SerializeField] private int minionsKilled;
    [SerializeField] private int bossesKilled;

    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI killsMultiText;
    [SerializeField] private TextMeshProUGUI bossKillsText;
    [SerializeField] private TextMeshProUGUI bossKillsMultiText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private TextMeshProUGUI creditsLostText;
    [SerializeField] private TextMeshProUGUI egoText;
    [SerializeField] private TextMeshProUGUI egoLostText;


    [SerializeField] private int delay = 250;
    [SerializeField] private int minionRatio = 10;
    [SerializeField] private int bossRatio = 100;

    async void Start()
    {
        credits = GameManager.Instance.runPlayer.credits;
        ego = GameManager.Instance.runPlayer.ego;
        if (GameManager.Instance.runPlayer.isDead)
        {
            credits = (int)(credits * deathPenaltyRatio);
            creditsLost = GameManager.Instance.runPlayer.credits - credits;
            ego = (int)(ego * deathPenaltyRatio);
            egoLost = GameManager.Instance.runPlayer.ego - ego;
            creditsLostText.gameObject.SetActive(true);
            egoLostText.gameObject.SetActive(true);
        }

        highScore = PlayerPrefs.GetInt("highScore", 0);
        highScoreText.text = highScore.ToString();
        score = GameManager.Instance.runStats.bossesKilled * bossRatio +
                GameManager.Instance.runStats.minionsKilled * minionRatio;
        minionsKilled = GameManager.Instance.runStats.minionsKilled;
        bossesKilled = GameManager.Instance.runStats.bossesKilled;
        
        
        GameManager.Instance.metaPlayer.CopyEgo(GameManager.Instance.runPlayer);
        GameManager.Instance.metaPlayer.CopyCards(GameManager.Instance.runPlayer);
        GameManager.Instance.metaPlayer.CopyCredits(GameManager.Instance.runPlayer);
        GameManager.Instance.metaPlayer.CopyCardSouls(GameManager.Instance.runPlayer);
        GameManager.Instance.metaPlayer.CopyCardPacks(GameManager.Instance.runPlayer);
        GameManager.Instance.metaPlayer.CopyCapsules(GameManager.Instance.runPlayer);
        GameManager.Instance.metaPlayer.CopySuperCapsules(GameManager.Instance.runPlayer);
        GameManager.Instance.metaStats.Add(GameManager.Instance.runStats);
        if (!GameManager.Instance.runPlayer.isDead)
        {
            cardPacks++;
        }

        GameManager.Instance.metaPlayer.AddCardPack(cardPacks);
        GameManager.Instance.saveManager.SaveMeta();
        GameManager.Instance.battlefield.Reset();
        GameManager.Instance.saveManager.SaveRun();

        killsMultiText.text = "x" + minionRatio;
        bossKillsMultiText.text = "x" + bossRatio;
        
        GameManager.Instance.uiStateObject.Ping("Game Over");
        
        await Task.Delay(delay*2);

        DOTween.To(() => killsText.text, x => killsText.text = x, "" + minionsKilled,
            0.5f);
        await Task.Delay(delay);
        DOTween.To(() => bossKillsText.text, x => bossKillsText.text = x,
            "" + bossesKilled, 0.5f);
        await Task.Delay(delay);
        DOTween.To(() => scoreText.text, x => scoreText.text = x, "" + score, 0.5f);
        await Task.Delay(delay);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("highScore", score);
            DOTween.To(() => highScoreText.text, x => highScoreText.text = x, "" + score, 0.5f);
            await Task.Delay(delay);
        }
        DOTween.To(() => creditsText.text, x => creditsText.text = x, "" + credits, 0.5f);
        await Task.Delay(delay);
        DOTween.To(() => creditsLostText.text, x => creditsLostText.text = x, "" + creditsLost, 0.5f);
        await Task.Delay(delay);
        DOTween.To(() => egoText.text, x => egoText.text = x, "" + ego, 0.5f);
        await Task.Delay(delay);
        DOTween.To(() => egoLostText.text, x => egoLostText.text = x, "" + egoLost, 0.5f);
        
       
    }

    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    public void ReturnToMenu()
    {
        GameManager.Instance.LoadSceneAdditive("MainMenu", "GameOver");
    }
}