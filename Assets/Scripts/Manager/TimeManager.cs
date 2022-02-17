using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public Text timeTxt;
    public Text timeTxt_Shadow;
    public Light dirLight;
    public Light spotLight;

    private float sec_Current = 0;
    private float min_Current = 0;
    private float time_Current;
    private string numTxt;
    private float maxTime = 10.0f;
    private bool isEnded = false;
    private bool isDay = true;

    private void Start()
    {
        TimerReset();
    }
    private void Update()
    {
        if(isEnded)
        {
            TimerEnd();
        }
        TimerCheck();
    }
    private void TimerReset()
    {
        time_Current = maxTime;
        if(time_Current >= 60)
        {
            min_Current = time_Current / 60;
            sec_Current = time_Current % 60;
            //Debug.Log($"min : {min_Current} sec : {sec_Current}");
        }
        else
        {
            min_Current = 0;
            sec_Current = time_Current;
        }
        numTxt = ((int)min_Current).ToString() + " : " + ((int)sec_Current).ToString("D2");
        timeTxt.text = numTxt;
        timeTxt_Shadow.text = numTxt;
        isEnded = false;
    }
    private void TimerCheck()
    {
        if (0 <= sec_Current)
        {
            sec_Current -= Time.deltaTime;
            numTxt = ((int)min_Current).ToString() + " : " + ((int)sec_Current).ToString("D2");
            timeTxt.text = numTxt;
            timeTxt_Shadow.text = numTxt;
        }
        else if(0 > sec_Current)
        {
            if (min_Current == 0)
            {
                TimerEnd();
                return;
            }

            sec_Current = 60.0f;
            min_Current -= 1;
        }
        
    }
    private void TimerEnd()
    {
        sec_Current = maxTime;
        numTxt = ((int)min_Current).ToString() + " : " + ((int)sec_Current).ToString("D2");
        timeTxt.text = numTxt;
        timeTxt_Shadow.text = numTxt;
        isEnded = true;

        if(isDay)
        {
            dirLight.intensity = 0;
            spotLight.intensity = 10;
            isDay = false;
        }
        else
        {
            dirLight.intensity = 1;
            spotLight.intensity = 0;
            isDay = true;
        }
        TimerReset();
    }
}
