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
        if (myFile != null)
            myFile.validityChange -= OnValidityChange;

        myFile = f;
        myFile.validityChange += OnValidityChange;

        this.fileName.text = f.path;

        if(myFile.valid)
        {
            SlotManager.Instance.SetSlotValid(number, true);
            this.fileStatus.text = "valid command file";
            ApplyColor(valid_c);
            valid = true;
        }
        else
        {
            SlotManager.Instance.SetSlotValid(number, false);
            this.fileStatus.text = "invalid command file";
            ApplyColor(invalid_c);
            valid = false;
        }
    }

    public void EmptySlot()
    {
        myFile.validityChange -= OnValidityChange;
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
        if (myFile != null && myFile.valid)
            return myFile.commands;
        else
            return new List<string[]>();
    }

    private void OnValidityChange(bool valid)
    {
        SlotManager.Instance.SetSlotValid(number, valid);
        if (valid)
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
}
