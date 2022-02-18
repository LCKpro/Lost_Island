using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    private AsyncOperation async1;
    private AsyncOperation async2;

    public GameObject loadingBar;

    private float progress;

    void Start()
    {

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
    }
}
