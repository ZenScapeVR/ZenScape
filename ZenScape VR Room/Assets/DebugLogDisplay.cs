using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugLogDisplay : MonoBehaviour
{
    public TextMeshPro logText;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logText, string stackTrace, LogType type)
    {
        this.logText.text += logText + "\n";
    }
}
