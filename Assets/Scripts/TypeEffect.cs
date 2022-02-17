using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    // 글자 재생 속도 변수 (초당 몇 글자)
    public int charPerSeconds;
    public GameObject endCursor;

    string targetMsg;
    Text msgText;
    AudioSource audioSource;
    int index;
    float interval;
    public bool isAnim = false;

    private void Start()
    {
        msgText = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetMsg(string msg)
    {
        if (isAnim)
        {
            msgText.text = targetMsg;
            CancelInvoke("Effecting");
            EffectEnd();
        }
        else
        {
            targetMsg = msg;
            EffectStart();
        }
    }

    void EffectStart()
    {
        msgText.text = "";
        index = 0;
        endCursor.SetActive(false);

        //애니메이션 시작
        interval = 1.0f / charPerSeconds;
        isAnim = true;
        Invoke("Effecting", interval);
    }

    void Effecting()
    {
        if (msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }
        msgText.text += targetMsg[index];
        // 텍스트 사운드
        if (targetMsg[index] != ' ' || targetMsg[index] != '.')
            audioSource.Play();

        index++;
        // 끝날 때 까지 다시 호출
        Invoke("Effecting", interval);
    }

    void EffectEnd()
    {
        isAnim = false;
        endCursor.SetActive(true);
    }
}
