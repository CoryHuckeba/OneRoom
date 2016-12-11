using Files;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandEditor : MonoBehaviour {

    #region Properties & Variables

    // Reference to the console view
    public ConsoleView consoleView;

    public GameObject view;
    public InputField file;
    public Text feedback;

    private bool control = false;
    private CommandFile activeFile;

    #endregion Properties & Variables


    #region MonoBehaviour Implementation

    // Use this for initialization
    void Start ()
    {
        file.lineType = InputField.LineType.MultiLineNewline;
        consoleView.console.openEditor += OnOpen;
	}
	
	// Update is called once per frame
	void Update ()
    {
		// Prevent typing while ctrl is held down
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            file.DeactivateInputField();
            control = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            control = false;
            file.ActivateInputField();
            file.MoveTextEnd(false);
        }
        
        if (control && Input.GetKeyDown(KeyCode.X))
        {
            ParseResults p = CommandParser.Instance.ParseFile(file.text);
            if (p.success == true)
            {
                feedback.text = "";
                activeFile.commands = p.commands;
                activeFile.valid = true;
            }
            else
            {
                activeFile.valid = false;
                feedback.text = p.error;
            }
        }

        if (control && Input.GetKeyDown(KeyCode.C))
        {
            Close();
        }
    }

    #endregion MonoBehaviour Implementation


    #region Event Handlers

    private void OnOpen(bool open, CommandFile openedFile=null)
    {
        if (open)
        {
            // Set up file environment
            this.activeFile = openedFile;
            this.file.text = commandsToString(openedFile.commands);

            // Enable editor
            view.SetActive(true);
            file.GetComponent<ConsoleInput>().enabled = true;
            file.ActivateInputField();            
        }
    }

    private void Close()
    {
        activeFile = null;
        file.GetComponent<ConsoleInput>().enabled = false;
        file.DeactivateInputField();
        view.SetActive(false);
    }


    #endregion Event Handlers


    #region File Processing

    private string commandsToString(List<string[]> commands)
    {
        string s = "";

        foreach (string[] com in commands)
        {
            foreach (string arg in com)
            {
                s += arg + " ";
            }

            s += "\n";
        }

        return s;
    }

    #endregion File Processing
}
