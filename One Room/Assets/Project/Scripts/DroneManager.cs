using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManager : Singleton<DroneManager> {

    public event System.Action<List<string[]>> beginRun;

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

        foreach (string[] args in commands)
        {
            string debug = "";
            foreach (string s in args)
            {
                debug += s + " ";
            }

            Debug.Log(debug);
        }
    }

    #endregion Public Interface

}
