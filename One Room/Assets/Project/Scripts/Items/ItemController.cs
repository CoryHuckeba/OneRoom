using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldItem
{
    public string itemType;
    public string sprite;
    public string size;
    public string material;
    public string name;
    public string description;
    public bool carryable;
    //bool draggable;
    //bool pushable;
    //bool interactive;
}

public class KeyCard : WorldItem
{
    public string color;

    public KeyCard(string color, string name, string description)
    {
        this.color = color;
        itemType = "KeyCard";
        size = "small";
        material = "plastic";
        this.name = name;
        this.description = description;
        carryable = true;
    }
}

public class Gore : WorldItem
{
    public Gore()
    {
        string name = "";
        string description = "";
        switch (Random.Range(0, 3))
        {
            case (0):
                name = "human head";
                description = "human head";
                break;
            case (1):
                name = "human arm";
                description = "human arm";
                break;
            case (2):
                name = "human leg";
                description = "human leg";
                break;
            case (3):
                name = "unidentified soft tissue";
                description = "unidentified soft tissue";
                break;
        }
        this.name = name;
        this.description = description; 
        this.material = "organic";
        this.size = "medium";
        this.carryable = true;
    }
}

public class ItemController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}