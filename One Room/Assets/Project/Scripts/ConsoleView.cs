using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;
using Files;

public class ConsoleView : MonoBehaviour
{
    ConsoleController console = new ConsoleController();

    bool didShow = false;

    public GameObject viewContainer; // Container for console view, should be a child of this GameObject
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
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (inputField.isFocused)
            {
                runCommand();
            }
            else
                inputField.Select();
        }

        //Toggle visibility when tilde key pressed
        if (Input.GetKeyUp("`"))
        {
            toggleVisibility();
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

    void toggleVisibility()
    {
        setVisibility(!viewContainer.activeSelf);
    }

    void setVisibility(bool visible)
    {
        viewContainer.SetActive(visible);
    }

    void onVisibilityChanged(bool visible)
    {
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

    void updateLogStr(string[] newLog)
    {
        if (newLog == null)
        {
            logTextArea.text = "";
        }
        else
        {
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