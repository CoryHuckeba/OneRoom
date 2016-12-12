using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour {

    private bool playerClose = false;
    public GameObject interactiveKey;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerClose && !ConsoleView.Instance.viewActive && !ConsoleView.Instance.editorOpen)
        {
            ConsoleView.Instance.OpenConsole();
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            interactiveKey.SetActive(true);
            playerClose = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            interactiveKey.SetActive(false);
            playerClose = false;
        }
    }

}
