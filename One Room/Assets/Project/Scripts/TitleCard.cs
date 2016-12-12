using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCard : MonoBehaviour {

    public void FadedOut()
    {
        this.gameObject.SetActive(false);
    }

    public void PlayerMove()
    {
        PlayerController.Instance.AllowMove(true);
    }
}
