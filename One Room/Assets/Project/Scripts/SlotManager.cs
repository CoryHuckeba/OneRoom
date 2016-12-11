using Files;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : Singleton<SlotManager> {

    public DroneSlot[] slots;

    private Dictionary<int, DroneSlot> slotDictionary;
    private Dictionary<int, bool> slotsValid;        

	void Start () {
        // Configure array into table
        slotDictionary = new Dictionary<int, DroneSlot>();
        slotsValid = new Dictionary<int, bool>();
        foreach (DroneSlot s in slots)
        {
            slotDictionary.Add(s.number, s);
            slotsValid.Add(s.number, true);
        }
	}
	
	public void SetFile(int slot, CommandFile file)
    {
        slotDictionary[slot].AssignSlot(file);
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

    public bool AllValid()
    {
        foreach (bool b in slotsValid.Values)
        {
            if (!b)
                return false;
        }

        return true;
    }

    public void SetSlotValid(int slot, bool valid)
    {
        if (slot <= 7 && slot > 0)
            slotsValid[slot] = valid;
    }
}
