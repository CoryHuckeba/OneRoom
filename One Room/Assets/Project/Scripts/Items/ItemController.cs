using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldItem
{
    public string itemType;
    public string sprite;
    public string description;
    public bool carryable;
    //bool draggable;
    //bool pushable;
    //bool interactive;
}

public class KeyCard : WorldItem
{
    public int level;

    public KeyCard(int level, string description)
    {
        this.level = level;
        itemType = "KeyCard";
        this.description = description;
        carryable = true;
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