using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBrain : MonoBehaviour
{

    public WorldController WorldController;
    public bool roomsLoaded = false;

    private WorldLocation[,] currentRoom;
    private WorldItem carriedItem;
    private int currentRow;
    private int currentCol;
    private string facing;

    public List<string> logList = new List<string>();

    private List<string[]> commandList = new List<string[]>
    {
        new string[] { "move", "3"},
        new string[] { "open" },
        new string[] { "move", "2"},
        new string[] { "turn", "right"},
        new string[] { "move", "3"},
        new string[] { "drop" },
        new string[] { "turn", "around" },
        new string[] { "move", "1" },
        new string[] { "turn", "around" },
        new string[] { "move", "1" },
        new string[] { "grab" }
        //new string[] { "OpenDoor", "1435"}
    };

    // Use this for initialization
    void Start()
    {
        StartCoroutine("test");
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(1f);
        startRun(commandList);

    }

    public void setItem(WorldItem carriedItem)
    {
        this.carriedItem = carriedItem;
    }

    public void startRun(List<string[]> commandList)
    {
        facing = "up";
        currentRoom = WorldController.Room1;
        currentRow = 3;
        currentCol = 2;
        this.commandList = commandList;
        this.carriedItem = new KeyCard(2, "A level 2 security key card.");
        StartCoroutine("ProcessCommandList");
    }

    IEnumerator ProcessCommandList()
    {
        for (int i = 0; i < commandList.Count; i++)
        {
            string commandName = commandList[i][0];
            string[] commandArgs = new string[commandList[i].Length - 1];

            for (int j = 0; j < commandArgs.Length; j++)
            {
                commandArgs[j] = commandList[i][j + 1];
            }

            switch (commandName)
            {
                case "move":
                    yield return StartCoroutine("move", commandArgs);
                    break;
                case "turn":
                    yield return StartCoroutine("turn", commandArgs);
                    break;
                case "open":
                    yield return StartCoroutine("open", commandArgs);
                    break;
                case "grab":
                    yield return StartCoroutine("grab");
                    break;
                case "drop":
                    yield return StartCoroutine("drop");
                    break;
                case "use":
                    break;
                default:
                    break;
            }

            if (i == (commandList.Count - 1))
            {
                //NOTE(Dylan): We're done with the run. 
                //So send the information to the 
                //Drone manager singleton

                logList.ForEach(log => Debug.Log(log));

                //TODO(Dylan): Uncomment when drone manager exists
                //Instance.RunComplete(logList, currentItem);

            }

            yield return new WaitForSeconds(0.1f);

        }

    }

    IEnumerator move(string[] commandArgs)
    {
        int timesToMove = int.Parse(commandArgs[0]);
        for (int i = 0; i < timesToMove; i++)
        {
            if (facing == "up")
            {
                int potentialRow = currentRow - 1;
                WorldLocation moveLocation = currentRoom[potentialRow, currentCol];
                if (moveLocation.isPassable)
                {
                    if (moveLocation.GetType() == new Door().GetType())
                    {
                        Door moveLocationDoor = moveLocation as Door;
                        if (moveLocationDoor.isOpen)
                        {
                            currentRoom = moveLocationDoor.nextRoom;
                            currentRow = moveLocationDoor.nextRoomRow;
                            currentCol = moveLocationDoor.nextRoomCol;

                            logList.Add("Moved " + facing);
                        }
                        else
                        {
                            logList.Add("Couldn't move " + facing + ", We ran into a closed door!");
                        }
                    }
                    else
                    {
                        currentRow = potentialRow;
                        logList.Add("Moved " + facing);
                    }
                }
                else
                {
                    logList.Add("Couldn't move " + facing + ", ran into something impassable!");
                }
            }
            else if (facing == "down")
            {
                int potentialRow = currentRow + 1;
                WorldLocation moveLocation = currentRoom[potentialRow, currentCol];
                if (moveLocation.isPassable)
                {
                    if (moveLocation.GetType() == new Door().GetType())
                    {
                        Door moveLocationDoor = moveLocation as Door;
                        if (moveLocationDoor.isOpen)
                        {
                            currentRoom = moveLocationDoor.nextRoom;
                            currentRow = moveLocationDoor.nextRoomRow;
                            currentCol = moveLocationDoor.nextRoomCol;

                            logList.Add("Moved " + facing);
                        }
                        else
                        {
                            logList.Add("Couldn't move " + facing + ", We ran into a closed door!");
                        }
                    }
                    else
                    {
                        currentRow = potentialRow;
                        logList.Add("Moved " + facing);
                    }
                }
                else
                {
                    logList.Add("Couldn't move " + facing + ", ran into something impassable!");
                }
            }
            else if (facing == "left")
            {
                int potentialCol = currentCol - 1;
                WorldLocation moveLocation = currentRoom[currentRow, potentialCol];
                if (moveLocation.isPassable)
                {
                    if (moveLocation.GetType() == new Door().GetType())
                    {
                        Door moveLocationDoor = moveLocation as Door;
                        if (moveLocationDoor.isOpen)
                        {
                            currentRoom = moveLocationDoor.nextRoom;
                            currentRow = moveLocationDoor.nextRoomRow;
                            currentCol = moveLocationDoor.nextRoomCol;

                            logList.Add("Moved " + facing);
                        }
                        else
                        {
                            logList.Add("Couldn't move " + facing + ", We ran into a closed door!");
                        }
                    }
                    else
                    {
                        currentCol = potentialCol;
                        logList.Add("Moved " + facing);
                    }
                }
                else
                {
                    logList.Add("Couldn't move " + facing + ", ran into something impassable!");
                }
            }
            else if (facing == "right")
            {
                int potentialCol = currentCol + 1;
                WorldLocation moveLocation = currentRoom[currentRow, potentialCol];
                if (moveLocation.isPassable)
                {
                    if (moveLocation.GetType() == new Door().GetType())
                    {
                        Door moveLocationDoor = moveLocation as Door;
                        if (moveLocationDoor.isOpen)
                        {
                            currentRoom = moveLocationDoor.nextRoom;
                            currentRow = moveLocationDoor.nextRoomRow;
                            currentCol = moveLocationDoor.nextRoomCol;

                            logList.Add("Moved " + facing);
                        }
                        else
                        {
                            logList.Add("Couldn't move " + facing + ", We ran into a closed door!");
                        }
                    }
                    else
                    {
                        currentCol = potentialCol;
                        logList.Add("Moved " + facing);
                    }
                }
                else
                {
                    logList.Add("Couldn't move " + facing + ", ran into something impassable!");
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator turn(string[] commandArgs)
    {
        string turnDir = "right";
        int timesToTurn = 1;

        if (commandArgs.Length >= 1)
        {
            turnDir = commandArgs[0];
            if (turnDir == "around")
            {
                timesToTurn = 2;
                turnDir = "left";
            }
        }

        for (int i = 0; i < timesToTurn; i++)
        {

            if (facing == "up")
            {
                if (turnDir == "left")
                {
                    facing = "left";
                }
                else if (turnDir == "right")
                {
                    facing = "right";
                }
            }
            else if (facing == "down")
            {
                if (turnDir == "left")
                {
                    facing = "right";
                }
                else if (turnDir == "right")
                {
                    facing = "left";
                }
            }
            else if (facing == "left")
            {
                if (turnDir == "left")
                {
                    facing = "down";
                }
                else if (turnDir == "right")
                {
                    facing = "up";
                }
            }
            else if (facing == "right")
            {
                if (turnDir == "left")
                {
                    facing = "up";
                }
                else if (turnDir == "right")
                {
                    facing = "down";
                }
            }
            logList.Add("Turned " + turnDir);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator open(string[] commandArgs)
    {
        WorldLocation doorLocation = currentRoom[currentRow, currentCol];
        switch (facing)
        {
            case "up":
                doorLocation = currentRoom[currentRow - 1, currentCol];
                break;
            case "down":
                doorLocation = currentRoom[currentRow + 1, currentCol];
                break;
            case "left":
                doorLocation = currentRoom[currentRow, currentCol - 1];
                break;
            case "right":
                doorLocation = currentRoom[currentRow, currentCol + 1];
                break;
            default:
                break;
        }
        if (doorLocation.GetType() == new Door().GetType())
        {
            Door door = doorLocation as Door;
            int keyCode = 0;
            if (commandArgs.Length > 0)
            {
                keyCode = int.Parse(commandArgs[0]);
            }
            int keyCardLevel = 0;
            if (carriedItem.itemType == "KeyCard")
            {
                KeyCard keyCard = carriedItem as KeyCard;
                keyCardLevel = keyCard.level;
            }

            string result = door.open(keyCode, keyCardLevel);
            logList.Add(result);
        }
        else
        {
            logList.Add("Open failed. There is no door here.");
        }
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator grab()
    {
        WorldLocation currentLocation = currentRoom[currentRow, currentCol];
        WorldItem itemToGrab = currentLocation.itemAtLocation;
        if (itemToGrab != null)
        {
            if (carriedItem == null)
            {
                if (itemToGrab.carryable)
                {
                    carriedItem = itemToGrab;
                    currentLocation.itemAtLocation = null;
                    logList.Add("Grab succeeded: the drone picked up " + carriedItem.description);
                }
                else
                {
                    logList.Add("Grab failed: the item here is too heavy to pick up!");
                }
            }
            else
            {
                logList.Add("Grab failed: the drone is already holding something!");
            }
        }
        else
        {
            logList.Add("Grab failed: there is nothing here to pick up!");
        }
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator drop()
    {
        WorldLocation currentLocation = currentRoom[currentRow, currentCol];
        WorldItem itemAtCurrentLocation = currentLocation.itemAtLocation;
        if (carriedItem != null)
        {
            if (itemAtCurrentLocation == null)
            {
                currentLocation.itemAtLocation = carriedItem;
                carriedItem = null;
                logList.Add("Drop succeeded: the drone dropped " + currentLocation.itemAtLocation.description);
            }
            else
            {
                logList.Add("Drop failed: there is already something on the ground at this location!");
            }
        }
        else
        {
            logList.Add("Drop failed: the drone has nothing to drop!");
        }
        yield return new WaitForSeconds(0.1f);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
