using Files;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroneSlot : MonoBehaviour {

    [HideInInspector]
    public bool valid = true;

    public int number;

    public Color valid_c;
    public Color invalid_c;
    public Color noFile_c;

    public Image border;
    public Text fileName;
    public Text fileStatus;

    private CommandFile myFile;

	public void AssignSlot(CommandFile f)
    {
        myFile = f;

        this.fileName.text = f.name;

        if(myFile.valid)
        {
            this.fileStatus.text = "valid command file";
            ApplyColor(valid_c);
            valid = true;
        }
        else
        {
            this.fileStatus.text = "invalid command file";
            ApplyColor(invalid_c);
            valid = false;
        }
    }

    public void EmptySlot()
    {
        this.myFile = null;

        this.fileName.text = "No File";
        this.fileStatus.text = "";
        ApplyColor(noFile_c);
        valid = true;
    }

    private void ApplyColor(Color c)
    {
        this.border.color = c;
        this.fileName.color = c;
        this.fileStatus.color = c;
    }

    public List<string[]> GetCommands()
    {
        if (myFile.valid)
            return myFile.commands;
        else
            return null;
    }
}
