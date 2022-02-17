using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public float sfxVolume = 1.0f;
    public float bgmVolume = 1.0f;

    public bool isSfxMute = false;
    public bool isBGMMute = false;

    static private int bgmCount = 0;
    
    AudioSource bgmSource;
    GameObject bgmObj;

    public AudioClip titleBGM;
    public AudioClip[] mainBGM;
    public AudioClip buttonSound_Casual;
    public AudioClip buttonSound_Classic;
    public AudioClip footStep;
    public AudioClip jumpSound;
    public AudioClip damagedSound;
    public AudioClip recallSound;
    public AudioClip getItemSound;

    public static BGMManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameBGM_Play();
    }

    private void GameBGM_Play()
    {
        string sceneName = CryptoPlayerPrefs.GetString("scene");
        int stageNumber = CryptoPlayerPrefs.GetInt("stageNum");

        switch (sceneName)
        {
            case "title":
                PlayBgm(titleBGM, 0, true);
                break;
            case "stage":
                {
                    if (stageNumber >= mainBGM.Length)
                        stageNumber = mainBGM.Length - 1;

                    ChangeBGM(mainBGM[stageNumber - 1], 0, true);
                }
                break;
            default:
                break;
        }
    }

    public void PlaySfx(Vector3 pos, AudioClip sfx, float delayed, float volm)
    {
        if (isSfxMute) return;

        StartCoroutine(PlaySfxDelayed(pos, sfx, delayed, volm));
    }

    IEnumerator PlaySfxDelayed(Vector3 pos, AudioClip sfx, float delayed, float volm)
    {
        yield return new WaitForSeconds(delayed);

        GameObject sfxObj = new GameObject("sfx");
        AudioSource aud = sfxObj.AddComponent<AudioSource>();

        aud.transform.position = pos;
        aud.clip = sfx;
        aud.minDistance = 5.0f;
        aud.maxDistance = 10.0f;
        aud.volume = volm * sfxVolume;
        aud.Play();

        Destroy(sfxObj, sfx.length);
    }

    public void BGMVolumeCtr(float volm)
    {
        GameObject bgm = GameObject.Find("bgm" + bgmCount);
        bgm.GetComponent<AudioSource>().volume = volm;
    }

    public void MuteBGM(bool isMute)
    {
        GameObject bgm = GameObject.Find("bgm" + bgmCount);
        bgm.GetComponent<AudioSource>().mute = isMute;
    }

    public void PlayBgm(AudioClip sfx, float delayed, bool isLoop)
    {
        if (isBGMMute) return;

        StartCoroutine(PlayBgmDelayed(sfx, delayed, isLoop));
    }

    IEnumerator PlayBgmDelayed(AudioClip sfx, float delayed, bool isLoop)
    {
        yield return new WaitForSeconds(delayed);

        bgmCount++;
        bgmObj = new GameObject("bgm" + bgmCount);

        if (!bgmSource) bgmSource = bgmObj.AddComponent<AudioSource>();

        bgmSource.clip = sfx;
        bgmSource.volume = bgmVolume;
        bgmSource.loop = isLoop;
        bgmSource.Play();
    }

    public void ChangeBGM(AudioClip sfx, float delayed, bool isLoop)
    {
        if (isBGMMute) return;

        StartCoroutine(ChangeBgmDelayed(sfx, delayed, isLoop));
    }

    IEnumerator ChangeBgmDelayed(AudioClip sfx, float delayed, bool isLoop)
    {
        if(bgmObj)
        {
            bgmSource.Stop();
        }
        else
        {
            bgmCount++;
            bgmObj = new GameObject("bgm" + bgmCount);
            if (!bgmSource) bgmSource = bgmObj.AddComponent<AudioSource>();
        }

        yield return new WaitForSeconds(delayed);

        bgmSource.clip = sfx;
        bgmSource.volume = bgmVolume;
        bgmSource.loop = isLoop;
        bgmSource.Play();
    }
}
