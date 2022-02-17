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
    public Text stageTxt;
    public Text stageTxtShadow;

    public Image[] healthImg;
    public Image gameOverUI;
    public Image gamePauseUI;

    public Sprite[] itemBuffSprite;
    public Sprite blankSprite;
    public Image[] itemBuffUI;
    private List<Sprite> itemLists;

    public int healthCnt;
    private int stageNum;
    private int maxStageClearItem;
    private int currentStageClearItem = 0;

    public Text leftNumTxt;

    public int StageNumber
    {
        get
        {
            return stageNum;
        }
    }

    private void Awake()
    {
        instance = this;
        itemLists = new List<Sprite>();
    }

    private void Start()
    {
        healthCnt = CryptoPlayerPrefs.GetInt("healthCnt");
        stageNum = CryptoPlayerPrefs.GetInt("stageNum");
        maxStageClearItem = 2 + stageNum;
        CalcHealthCnt(0);
        gameOverUI.gameObject.SetActive(false);
        stageTxt.text = "Stage " + stageNum;
        stageTxtShadow.text = stageTxt.text;
        leftNumTxt.text = "× " + maxStageClearItem;
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

    public void GetStageClearItem()
    {
        currentStageClearItem++;
        leftNumTxt.text = "× " + (maxStageClearItem - currentStageClearItem);

        if (maxStageClearItem <= currentStageClearItem)
        {
            if (stageNum >= maxStage)
            {
                Debug.Log("다음 스테이지가 없습니다.");
                currentStageClearItem = 999;
            }
            else
            {
                currentStageClearItem = 0;
                stageNum++;
            }
            PrefReset(stageNum, healthCnt);
            SceneManager.LoadSceneAsync("Loading");
        }
    }
}
