using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    private bool playerClose = false;
    public GameObject mapView;
    public GameObject interactiveKey;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerClose && !ConsoleView.Instance.viewActive)
        {
            mapView.SetActive(true);
            PlayerController.Instance.AllowMove(false);
        }


        //Toggle visibility when escape key pressed
        if (Input.GetKeyUp(KeyCode.Escape) && mapView.activeInHierarchy)
        {
            mapView.SetActive(false);
            PlayerController.Instance.AllowMove(true);
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
