using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int maxStage = 4;
    public FadeTxt fadeTxt;

    public Image[] healthImg;
    public Image gameOverUI;
    public Image gamePauseUI;

    public Sprite[] itemBuffSprite;
    public Sprite blankSprite;
    public Image[] itemBuffUI;
    private List<Sprite> itemLists;

    public int healthCnt;

    public Image expGainImg;
    public Text playerLvText;
    private float expPoint_Current = 5;
    private int playerLevel = 1;

    public int PlayerLevel
    {
        get
        {
            return playerLevel;
        }
    }

    private void Awake()
    {
        instance = this;
        itemLists = new List<Sprite>();
    }

    private void Start()
    {
        playerLevel = 1;
        healthCnt = CryptoPlayerPrefs.GetInt("healthCnt");
        CalcHealthCnt(0);
        gameOverUI.gameObject.SetActive(false);
        //expGainImg.fillAmount = expPoint_Current == 0 ? 0 : expPoint_Current / (playerLevel * 100);
        expPoint_Current = 0;
        GainExpFunc(0);
    }

    

    public void CalcHealthCnt(int cnt)
    {
        healthCnt += (cnt);
        if (healthCnt > 6)
            healthCnt = 6;
        
        for (int i = 0; i < healthImg.Length; i++)
        {
            if(i < healthCnt)
                healthImg[i].color = new Color(1, 1, 1, 1);
            else
                healthImg[i].color = new Color(1, 1, 1, 0.35f);
        }

        if (healthCnt <= 0)
        {
            GameOver();
            return;
        }
    }

    private void PrefReset(int stageNum, int healthCnt)
    {
        CryptoPlayerPrefs.SetInt("stageNum", stageNum);
        CryptoPlayerPrefs.SetInt("healthCnt", healthCnt);
    }

    private void GameOver()
    {
        Debug.Log("게임 오버!");
        PrefReset(1, 3);
        gameOverUI.gameObject.SetActive(true);
    }
    public void GameSetting()
    {
        Time.timeScale = 0;
        BGMManager.instance.PlaySfx(transform.position, BGMManager.instance.buttonSound_Casual, 0, 1);
        SettingManager.instance.settingFrame.SetActive(true);
    }

    public void GamePause()
    {
        Time.timeScale = 0;
        gamePauseUI.gameObject.SetActive(true);
    }
    public void GamePauseExit()
    {
        Time.timeScale = 1;
        gamePauseUI.gameObject.SetActive(false);
    }

    public void ReStart()
    {
        Time.timeScale = 1;
        PrefReset(1, 3);
        SceneManager.LoadSceneAsync("Loading");
    }

    public void GoTitle()
    {
        Time.timeScale = 1;
        PrefReset(1, 3);
        CryptoPlayerPrefs.SetString("scene", "title");
        CryptoPlayerPrefs.SetString("GoTitle", "on");
        SceneManager.LoadSceneAsync("Loading");
    }

    public void BuffUI_On(int num)
    {
        itemLists.Add(itemBuffSprite[num]);
        BuffUI_Start();
        Invoke("BuffUI_Off", 10.0f);
    }
    public void BuffUI_Start()
    {
        if (itemLists.Count <= 0)
            itemLists.Add(blankSprite);
        else
            itemLists.Remove(blankSprite);

        for (int i = 0; i < itemLists.Count; i++)
        {
            if (itemLists[i] != null)
                itemBuffUI[i].sprite = itemLists[i];
            else
                itemBuffUI[i].sprite = blankSprite;
        }
    }

    private void BuffUI_Off()
    {
        if(itemLists.Count > 0)
            itemBuffUI[itemLists.Count - 1].sprite = blankSprite;
        itemLists.RemoveAt(0);
        BuffUI_Start();
    }

    public void GainExpFunc(int value)
    {
        expPoint_Current += value;
        expGainImg.fillAmount = expPoint_Current == 0 ? 0 : expPoint_Current / (playerLevel * 100);

        if(expPoint_Current >= playerLevel * 100)
        {
            playerLevel++;
            playerLvText.text = "Lv." + playerLevel;
            expPoint_Current -= ((playerLevel - 1) * 100);
            GainExpFunc(0);
        }
    }

    public void GetStageClearItem()
    {



    }
}
