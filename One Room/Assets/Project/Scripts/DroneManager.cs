using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : Singleton<DroneManager> {

    public event System.Action<List<string[]>> beginRun;

    public List<string> logs = new List<string>();

    public bool runActive = false;
    
	// Use this for initialization
	void Start ()
    {
	    	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    #region Public Interface

    public void SetCommands(List<string[]> commands)
    {
        if (!runActive)
        {
            if (beginRun != null)
            {
                runActive = true;
                beginRun(commands);
            } 
        }
    }

    public void AddLog(List<string> log)
    {
        string newLog = "";
        foreach (string line in log)
        {
            newLog += line + "\n";
        }

        if (newLog == "")
            newLog = "Empty Log. Did you forget to inpuit commands before running?\n";

        newLog += "\n---End of Log---";
        logs.Insert(logs.Count, newLog);
    }

    #endregion Public Interface

}
