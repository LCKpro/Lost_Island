using UnityEngine;
using UnityEngine.UI;

public class ExceptionLogger : MonoBehaviour
{
    public Text errorLogTxt;

    private void Awake()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Exception || type == LogType.Error)
        {
            errorLogTxt.text = "Logged at : " + System.DateTime.Now.ToString() +
                " - Log : " + logString +
                " - Trace : " + stackTrace +
                " - Type : " + type.ToString();
        }
    }
}
