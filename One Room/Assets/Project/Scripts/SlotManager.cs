using Files;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : Singleton<SlotManager> {

    public DroneSlot[] slots;

    public bool allValid = true;

    private Dictionary<int, DroneSlot> slotDictionary;

	void Start () {
        // Configure array into table
        slotDictionary = new Dictionary<int, DroneSlot>();
        foreach (DroneSlot s in slots)
        {
            slotDictionary.Add(s.number, s);
        }
	}
	
	public void SetFile(int slot, CommandFile file)
    {
        slotDictionary[slot].AssignSlot(file);
        ValidateSlots();
    }

    public void clearFile(int slot=-1)
    {
        // Clear all slots
        if (slot == -1)
        {
            foreach (DroneSlot s in slots)
                s.EmptySlot();
        }
        // Clear specified slot
        else
        {
            slotDictionary[slot].EmptySlot();
        }
        ValidateSlots();
    }

    public List<string[]> CompileSlots()
    {
        List<string[]> commands = new List<string[]>();
        foreach (DroneSlot s in slots)
        {
            List<string[]> tempCommands = s.GetCommands();
            if (tempCommands != null)
                commands.AddRange(tempCommands);
        }

        return commands;
    }

    private void ValidateSlots()
    {
        bool v = true;
        foreach (DroneSlot s in slots)
        {
            if (!s.valid)
            {
                allValid = false;
                return;
            }
        }

        allValid = true;
    }
}
