using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;
using Files;

public class ConsoleView : Singleton<ConsoleView>
{
    public ConsoleController console = new ConsoleController();

    bool didShow = false;
    public bool viewActive = false;
    public bool editorOpen = false;

    public GameObject viewContainer; // Container for console view, should be a child of this GameObject
    public GameObject droneUI;
    public Text logTextArea;
    public Text logCommandArea;
    public Text PathName;
    public InputField inputField;

    void Start()
    {
        if (console != null)
        {
            // Set up console initial path
            console.currentDirectory = new Directory("Home", "Home/");
            PathName.text = "Home/";           

            // Subscribe to console events
            console.visibilityChanged += onVisibilityChanged;
            console.logChanged += onLogChanged;
            console.commandLogChanged += onCommandLogChanged;
            console.workingPathChanged += onPathChanged;
        }

        updateLogStr(console.log);
    }

    ~ConsoleView()
    {
        console.visibilityChanged -= onVisibilityChanged;
        console.logChanged -= onLogChanged;
    }

    void Update()
    {
        // Toggle text input on enter key
        if (!editorOpen && viewActive && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            if (inputField.isFocused)
            {
                runCommand();
            }
            else
                inputField.Select();
        }

        //Toggle visibility when 5 fingers touch.
        if (Input.touches.Length == 5)
        {
            if (!didShow)
            {
                toggleVisibility();
                didShow = true;
            }
        }
        else
        {
            didShow = false;
        }
    }

    public void ExitConsole()
    {
        PlayerController.Instance.AllowMove(true);
        this.viewActive = false;
        this.viewContainer.SetActive(false);
        this.droneUI.SetActive(false);
        AudioManager.Instance.playSound(SoundType.ComputerOff);
    }

    public void OpenConsole()
    {
        this.viewActive = true;
        this.viewContainer.SetActive(true);
        this.droneUI.SetActive(true);
        this.inputField.ActivateInputField();
    }

    void toggleVisibility()
    {
        setVisibility(!viewContainer.activeSelf);
        setVisibility(!droneUI.activeSelf);
    }

    void setVisibility(bool visible)
    {
        viewContainer.SetActive(visible);
        droneUI.SetActive(visible);
    }

    void onVisibilityChanged(bool visible)
    {
        if (visible)
            viewActive = true;
        else
            viewActive = false;
        setVisibility(visible);
    }

    void onLogChanged(string[] newLog)
    {
        updateLogStr(newLog);
    }

    void onCommandLogChanged(string[] newLog)
    {
        updateCommandLogStr(newLog);
    }

    void onPathChanged(string newPath)
    {
        this.PathName.text = newPath;
    }

    public void EditorOpen(bool open)
    {
        editorOpen = true;
        viewActive = false;
        viewContainer.SetActive(false);
    }

    public void EditorClose()
    {
        editorOpen = false;
        viewActive = true;
        viewContainer.SetActive(true);
        this.inputField.ActivateInputField();
    }

    void updateLogStr(string[] newLog)
    {
        if (newLog == null)
        {
            // Show instructions
            logTextArea.text = "Welcome! type help for a list of commands";
        }
        else
        {
            if (logTextArea.text == "Welcome! type help for a list of commands")
                logTextArea.text = "";
            logTextArea.text = string.Join("\n", newLog);
        }
    }

    void updateCommandLogStr(string[] newLog)
    {
        if (newLog == null)
        {
            logCommandArea.text = "";
        }
        else
        {
            logCommandArea.text = string.Join("\n", newLog);
        }
    }

    /// <summary>
    /// Event that should be called by anything wanting to submit the current input to the console.
    /// </summary>
    public void runCommand()
    {
        console.runCommandString(inputField.text);
        inputField.text = "";
    }

}