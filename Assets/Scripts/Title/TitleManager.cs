using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    private float moveX;

    private void Awake()
    {
        //CryptoPlayerPrefs.DeleteAll();
    }

    void Start()
    {
        CryptoPlayerPrefs_HasKeyStringFind("Tutorial", "on");
        CryptoPlayerPrefs_HasKeyStringFind("GoTitle", "off");
        CryptoPlayerPrefs_HasKeyStringFind("scene", "title");
        CryptoPlayerPrefs_HasKeyIntFind("healthCnt", 3);

        SceneManager.LoadScene("Setting", LoadSceneMode.Additive);
        moveX = 0.003f;
        StartCoroutine(ReverseX());
    }

    void Update()
    {
        Camera.main.transform.Translate(new Vector3(moveX, 0, 0));
    }

    IEnumerator ReverseX()
    {
        yield return new WaitForSeconds(15.0f);
        moveX = -moveX;
        StartCoroutine(ReverseX());
    }

    public void GameStart()
    {
        CryptoPlayerPrefs.SetString("GoTitle", "off");
        CryptoPlayerPrefs.SetString("scene", "stage");
        BGMManager.instance.PlaySfx(transform.position, BGMManager.instance.buttonSound_Casual, 0, 1);
        GameLoading();
    }

    private void GameLoading()
    {
        SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("Setting", LoadSceneMode.Additive);

        Debug.Log("씬호출완료");
    }

    public void GameSetting()
    {
        BGMManager.instance.PlaySfx(transform.position, BGMManager.instance.buttonSound_Casual, 0, 1);
        SettingManager.instance.settingFrame.SetActive(true);
    }
    
    public void GameQuit()
    {
        Application.Quit();
    }

    //CryptoPlayerPrefs의 해쉬키를 찾는 함수
    // 해당 키가 있으면 불러오고 없으면 새로 생성 ( 실수값 )
    public void CryptoPlayerPrefs_HasKeyIntFind(string key, int value)
    {
        if (!CryptoPlayerPrefs.HasKey(key))
        {
            CryptoPlayerPrefs.SetInt(key, value);
        }
        else
        {
            CryptoPlayerPrefs.GetInt(key, 0);
        }
    }

    public void CryptoPlayerPrefs_HasKeyFloatFind(string key, float value)
    {
        if (!CryptoPlayerPrefs.HasKey(key))
        {
            CryptoPlayerPrefs.SetFloat(key, value);
        }
        else
        {
            CryptoPlayerPrefs.GetFloat(key, 0);
        }
    }


    public void CryptoPlayerPrefs_HasKeyStringFind(string key, string value)
    {
        if (!CryptoPlayerPrefs.HasKey(key))
        {
            CryptoPlayerPrefs.SetString(key, value);
        }
        else
        {
            CryptoPlayerPrefs.GetString(key, "0");
        }
    }

    
}
