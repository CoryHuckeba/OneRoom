using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBox : MonoBehaviour {

    public Image sr;
    public Sprite[] sprites;

    int index = 0;

    public void ChangeSquare()
    {
        Debug.Log("TEST");
        if (index == sprites.Length - 1)
            index = 0;
        else
            index++;

        sr.sprite = sprites[index];
    }
}
