using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCard : MonoBehaviour {

    public void FadedOut()
    {
        PlayerController.Instance.AllowMove(true);
        this.gameObject.SetActive(false);
    }
}
