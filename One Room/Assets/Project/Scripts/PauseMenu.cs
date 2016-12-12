using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : Singleton<PauseMenu> {

    public Text instructions;
    public Button showInstructions;
    public Button hideInstructions;
    public Button closeButton;

    public GameObject view;
    public bool showing = false;

    public void ShowInstructions(bool show)
    {
        if (show)
        {
            instructions.gameObject.SetActive(true);
            showInstructions.gameObject.SetActive(false);
            hideInstructions.gameObject.SetActive(true);
            closeButton.gameObject.SetActive(false);
        }
        else
        {
            closeButton.gameObject.SetActive(true);
            instructions.gameObject.SetActive(false);
            showInstructions.gameObject.SetActive(true);
            hideInstructions.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        
    }

    public void Close()
    {
        showing = false;
        instructions.gameObject.SetActive(false);
        showInstructions.gameObject.SetActive(true);
        hideInstructions.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(true);
        view.SetActive(false);

        PlayerController.Instance.AllowMove(true);
    }

    public void Open()
    {
        view.gameObject.SetActive(true);
        showing = true;

        PlayerController.Instance.AllowMove(false);
    }
}
