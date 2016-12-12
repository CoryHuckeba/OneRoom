using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogViewer : MonoBehaviour {
    
    private int index = 0;

    public Button nextButton;
    public Button previousButton;
    public Text logField;

    void Start()
    {
        DroneManager.Instance.newLogs += OnNewLogs;
    }

    void OnEnable()
    {
        index = DroneManager.Instance.logs.Count - 1;

        if (DroneManager.Instance.logs.Count > 0)
            logField.text = DroneManager.Instance.logs[index];
        else
            logField.text = "No Logs Found";

        if (index <= 0)
        {
            Debug.Log("Index: " + index);
            previousButton.interactable = false;
        }
        else
            previousButton.interactable = true;

        nextButton.interactable = false;
    }

    public void NextLog()
    {
        if (index < DroneManager.Instance.logs.Count - 1)
        {
            index++;
            logField.text = DroneManager.Instance.logs[index];

            if (index == DroneManager.Instance.logs.Count - 1)
                nextButton.interactable = false;

            if (DroneManager.Instance.logs.Count > 1)
                previousButton.interactable = true;
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

            nextButton.interactable = true;
        }
    }

    public void OnNewLogs()
    {
        if (DroneManager.Instance.logs.Count == 1)
        {
            index = 0;
            logField.text = DroneManager.Instance.logs[0];
        }
        else
            nextButton.interactable = true;
    }
}
