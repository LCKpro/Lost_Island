using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public GameObject settingFrame;
    public GameObject[] muteCheck;

    public Scrollbar bgmScroll;
    public Scrollbar sfxScroll;

    public static SettingManager instance;
    private void Awake()
    {
        instance = this;
    }
    public void GameSettingExit()
    {
        Time.timeScale = 1;
        settingFrame.SetActive(false);
        BGMManager.instance.PlaySfx(transform.position, BGMManager.instance.buttonSound_Classic, 0, 1);
    }

    public void MuteCheck(int number)
    {
        bool check = muteCheck[number].activeSelf;
        muteCheck[number].SetActive(!check);

        switch (number)
        {
            case 0:
                BGMManager.instance.isBGMMute = !check;
                BGMManager.instance.MuteBGM(!check);
                break;
            case 1:
                BGMManager.instance.isSfxMute = !check;
                break;
            default:
                break;
        }

        BGMManager.instance.PlaySfx(transform.position, BGMManager.instance.buttonSound_Classic, 0, 1);
    }

    public void ChangeBGMValue()
    {
        BGMManager.instance.bgmVolume = bgmScroll.value;
        BGMManager.instance.BGMVolumeCtr(bgmScroll.value);
        //Debug.Log("BGM : " + bgmScroll.value);
    }

    public void ChangeSFXValue()
    {
        BGMManager.instance.sfxVolume = sfxScroll.value;
        //Debug.Log("sfx : " + bgmScroll.value);
    }

}
