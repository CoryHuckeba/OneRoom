using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogViewer : MonoBehaviour {
    
    private int index = 0;

    public Button nextButton;
    public Button previousButton;
    public Text logField;

    void OnEnable()
    {
        index = DroneManager.Instance.logs.Count - 1;
        logField.text = DroneManager.Instance.logs[index];
    }

    public void NextLog()
    {
        if (index < DroneManager.Instance.logs.Count - 1)
        {
            index++;
            logField.text = DroneManager.Instance.logs[index];

            if (index == DroneManager.Instance.logs.Count - 1)
                nextButton.interactable = false;
        }
    }

    public void PreviousLog()
    {
        if (index > 0)
        {
            index--;
            logField.text = DroneManager.Instance.logs[index];

            if (index == 0)
                previousButton.interactable = false;
        }
    }
}
