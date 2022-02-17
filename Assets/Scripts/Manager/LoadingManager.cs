using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    AsyncOperation async1;               // 비동기화
    AsyncOperation async2;               // 비동기화

    public GameObject loadingBar;

    private float progress;

    void Start()
    {
        /*async1 = SceneManager.LoadSceneAsync("Stage", LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("Setting", LoadSceneMode.Additive);*/

        if (CryptoPlayerPrefs.GetString("GoTitle") == "on")
        {
            async1 = SceneManager.LoadSceneAsync("Title", LoadSceneMode.Single);
        }
        else
        {
            async1 = SceneManager.LoadSceneAsync("Stage", LoadSceneMode.Single);

            DontDestroyOnLoad(this.gameObject);
        }
        async2 = SceneManager.LoadSceneAsync("Setting", LoadSceneMode.Additive);
    }

    void Update()
    {
        /*if (CryptoPlayerPrefs.GetString("GoTitle") == "Off")*/
        if (!async1.isDone)       // 현재 있는 작업상황이 끝났는지??
        {
            progress = (async1.progress + async2.progress) * 100;
            loadingBar.GetComponent<Image>().fillAmount = (int)progress;
            //Debug.Log("Progress : " + progress);
        }

        if (async1.isDone && async2.isDone)
        {
            Destroy(this.gameObject);
        }
        /*else
        if (!async1.isDone)       // 현재 있는 작업상황이 끝났는지??
        {
            progress = async1.progress * 100;
            loadingBar.GetComponent<Image>().fillAmount = (int)progress;
            Debug.Log("Progress : " + progress);
        }*/
    }
}
