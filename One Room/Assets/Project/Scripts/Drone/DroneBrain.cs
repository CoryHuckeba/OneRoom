using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private List<string[]> commandList = new List<string[]>();
    /*{
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
    };*/

    // Use this for initialization
    void Start()
    {
        DroneManager.Instance.beginRun += startRun;
    }


    public void setItem(WorldItem carriedItem)
    {
        this.carriedItem = carriedItem;
    }

    public void startRun(List<string[]> commandList)
    {
        facing = "north";
        currentRoom = WorldController.Room1;
        currentRow = 3;
        currentCol = 2;
        this.commandList = commandList;
        /*this.carriedItem = new KeyCard(2, "Rick Sanchez's Level 2 Key Card", "A level 2 security key card. \n"
                                        + "It bears the name and image of Dr. Rick Sanchez. \n"
                                        + "It is partially covered with a flaky black substance.");*/
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
                    move(commandArgs);
                    break;
                case "turn":
                    turn(commandArgs);
                    break;
                case "open":
                    open(commandArgs);
                    break;
                case "grab":
                    grab();
                    break;
                case "drop":
                    drop();
                    break;
                case "scan":
                    scan();
                    break;
                case "push":
                    push();
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(.1f);

        }

        //NOTE(Dylan): We're done with the run. 
        //So send the information to the 
        //Drone manager singleton
        logList.ForEach(log => Debug.Log(log));
        DroneManager.Instance.endRun(logList, carriedItem);
        logList.Clear();

    }

    void move(string[] commandArgs)
    {
        int timesToMove = int.Parse(commandArgs[0]);
        for (int i = 0; i < timesToMove; i++)
        {
            if (facing == "north")
            {
                int potentialRow = currentRow - 1;
                WorldLocation moveLocation = currentRoom[potentialRow, currentCol];
                if (moveLocation.isPassable)
                {
                    Debug.Log(moveLocation.GetType());
                    if (moveLocation.GetType() == new Door().GetType())
                    {
                        Door moveLocationDoor = moveLocation as Door;
                        Debug.Log("MoveLocation: " + potentialRow + ", " + currentCol);
                        Debug.Log("NextRoom: " + moveLocationDoor.nextRoomName);
                        Debug.Log("Door's nextroom coords: " + moveLocationDoor.nextRoomRow + ", " + moveLocationDoor.nextRoomCol);
                        Debug.Log("Door's nextroom: \n" + WorldController.printRoomArray(moveLocationDoor.nextRoom));
                        if (moveLocationDoor.isOpen)
                        {
                            currentRoom = moveLocationDoor.nextRoom;
                            currentRow = moveLocationDoor.nextRoomRow;
                            currentCol = moveLocationDoor.nextRoomCol;

                            // END THE FUCKING GAME
                            if (currentRoom.Length == 11)
                                SceneManager.LoadScene(1);

                            logList.Add("Moved forward 1 meter.");
                        }
                        else
                        {
                            if(moveLocationDoor.keyCardColor != "none")
                            {
                                logList.Add("Failed to move forward. A closed " + moveLocationDoor.keyCardColor + " door is blocking the way.");
                            } else
                            {
                                logList.Add("Failed to move forward. A closed door is blocking the way.");
                            }
                        }
                    }
                    else
                    {
                        currentRow = potentialRow;
                        logList.Add("Moved forward 1 meter.");
                    }
                }
                else
                {
                    logList.Add("Failed to move forward. " + moveLocation.description + " is blocking the way.");
                }
            }
            else if (facing == "south")
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

                            logList.Add("Moved forward 1 meter.");
                        }
                        else
                        {
                            if (moveLocationDoor.keyCardColor != "none")
                            {
                                logList.Add("Failed to move forward. A closed " + moveLocationDoor.keyCardColor + " door is blocking the way.");
                            }
                            else
                            {
                                logList.Add("Failed to move forward. A closed door is blocking the way.");
                            }
                        }
                    }
                    else
                    {
                        currentRow = potentialRow;
                        logList.Add("Moved forward 1 meter.");
                    }
                }
                else
                {
                    logList.Add("Failed to move forward. " + moveLocation.description + " is blocking the way");
                }
            }
            else if (facing == "west")
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

                            logList.Add("Moved forward 1 meter.");
                        }
                        else
                        {
                            if (moveLocationDoor.keyCardColor != "none")
                            {
                                logList.Add("Failed to move forward. A closed " + moveLocationDoor.keyCardColor + " door is blocking the way.");
                            }
                            else
                            {
                                logList.Add("Failed to move forward. A closed door is blocking the way.");
                            }
                        }
                    }
                    else
                    {
                        currentCol = potentialCol;
                        logList.Add("Moved forward 1 meter.");
                    }
                }
                else
                {
                    logList.Add("Failed to move forward. " + moveLocation.description + " is blocking the way");
                }
            }
            else if (facing == "east")
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

                            logList.Add("Moved forward 1 meter.");
                        }
                        else
                        {
                            if(moveLocationDoor.keyCardColor != "none")
                            {
                                logList.Add("Failed to move forward. A closed " + moveLocationDoor.keyCardColor + " door is blocking the way.");
                            } else
                            {
                                logList.Add("Failed to move forward. A closed door is blocking the way.");
                            }
                        }
                    }
                    else
                    {
                        currentCol = potentialCol;
                        logList.Add("Moved forward 1 meter.");
                    }
                }
                else
                {
                    logList.Add("Failed to move forward. " + moveLocation.description + " is blocking the way");
                }
            }
        }
    }

    void turn(string[] commandArgs)
    {
        string turnDir = "left";
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

            if (facing == "north")
            {
                if (turnDir == "left")
                {
                    facing = "west";
                }
                else if (turnDir == "right")
                {
                    facing = "east";
                }
            }
            else if (facing == "south")
            {
                if (turnDir == "left")
                {
                    facing = "east";
                }
                else if (turnDir == "right")
                {
                    facing = "west";
                }
            }
            else if (facing == "west")
            {
                if (turnDir == "left")
                {
                    facing = "south";
                }
                else if (turnDir == "right")
                {
                    facing = "north";
                }
            }
            else if (facing == "east")
            {
                if (turnDir == "left")
                {
                    facing = "north";
                }
                else if (turnDir == "right")
                {
                    facing = "south";
                }
            }
            logList.Add("Turned " + turnDir);
        }
    }

    void open(string[] commandArgs)
    {
        WorldLocation doorLocation = currentRoom[currentRow, currentCol];
        switch (facing)
        {
            case "north":
                doorLocation = currentRoom[currentRow - 1, currentCol];
                break;
            case "south":
                doorLocation = currentRoom[currentRow + 1, currentCol];
                break;
            case "west":
                doorLocation = currentRoom[currentRow, currentCol - 1];
                break;
            case "east":
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
            string keyCardColor = "none";
            if (carriedItem != null)
            {
                if (carriedItem.itemType == "KeyCard")
                {
                    KeyCard keyCard = carriedItem as KeyCard;
                    keyCardColor = keyCard.color;
                }
            }

            string result = door.open(keyCode, keyCardColor);
            logList.Add(result);
        }
        else
        {
            logList.Add("Open failed. There is no door here.");
        }
    }

    void grab()
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
                    logList.Add("Grab succeeded: the drone picked up " + carriedItem.name);
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
    }

    void drop()
    {
        WorldLocation currentLocation = currentRoom[currentRow, currentCol];
        WorldItem itemAtCurrentLocation = currentLocation.itemAtLocation;
        if (carriedItem != null)
        {
            if (itemAtCurrentLocation == null)
            {
                currentLocation.itemAtLocation = carriedItem;
                carriedItem = null;
                logList.Add("Drop succeeded: the drone dropped " + currentLocation.itemAtLocation.name);
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
    }

    void scan()
    {


        Debug.Log(currentRoom == null);
        Debug.Log(currentRoom.GetLength(0));
        Debug.Log(currentRoom.GetLength(1));

        Debug.Log(WorldController.printRoomArray(currentRoom));
        
        logList.Add("Initiate room scan");
        for (int row = 0; row < currentRoom.GetLength(0); row++)
        {
            for (int col = 0; col < currentRoom.GetLength(1); col++)
            {
                WorldLocation locInRoom = currentRoom[row, col];
                if (locInRoom.itemAtLocation != null || locInRoom is Door || locInRoom is Boulder || locInRoom is Hole)
                {
                    if (row == currentRow && col == currentCol)
                    {
                        //the item is east on top of us
                        //use more detailed description
                        logList.Add("The scan detects a " + locInRoom.itemAtLocation.name + " in the immediate vicinity of the drone.");
                    }
                    else if (row < currentRow && col == currentCol)
                    {
                        //the item is directly north
                        if(locInRoom.itemAtLocation != null)
                        {
                            logList.Add("The scan detects a " + locInRoom.itemAtLocation.size + " " + locInRoom.itemAtLocation.material + " object to the North");
                        }
                        if (locInRoom is Door)
                        {
                            Door door = locInRoom as Door;
                            if(door.keyCardColor != "none")
                            {
                                logList.Add("The scan detects a " + door.keyCardColor + " door to the North");
                            }
                            
                        }
                        else if (locInRoom is Boulder || locInRoom is Hole)
                        {
                            logList.Add("The scan detects " + locInRoom.description + " to the North");
                        }
                    }
                    else if (row > currentRow && col == currentCol)
                    {
                        //the item is directly south
                        if (locInRoom.itemAtLocation != null)
                        {
                            logList.Add("The scan detects a " + locInRoom.itemAtLocation.size + " " + locInRoom.itemAtLocation.material + " object to the South");
                        }
                        if (locInRoom is Door)
                        {
                            Door door = locInRoom as Door;
                            if (door.keyCardColor != "none")
                            {
                                logList.Add("The scan detects a " + door.keyCardColor + " door to the South");
                            }

                        }
                        else if (locInRoom is Boulder || locInRoom is Hole)
                        {
                            logList.Add("The scan detects " + locInRoom.description + " to the South");
                        }
                    }
                    else if (row == currentRow && col < currentCol)
                    {
                        //the item is directly west
                        if (locInRoom.itemAtLocation != null)
                        {
                            logList.Add("The scan detects a " + locInRoom.itemAtLocation.size + " " + locInRoom.itemAtLocation.material + " object to the West");
                        }
                        if (locInRoom is Door)
                        {
                            Door door = locInRoom as Door;
                            if (door.keyCardColor != "none")
                            {
                                logList.Add("The scan detects a " + door.keyCardColor + " door to the West");
                            }

                        }
                        else if (locInRoom is Boulder || locInRoom is Hole)
                        {
                            logList.Add("The scan detects " + locInRoom.description + " to the West");
                        }
                    }
                    else if (row == currentRow && col > currentCol)
                    {
                        //the item is directly east
                        if (locInRoom.itemAtLocation != null)
                        {
                            logList.Add("The scan detects a " + locInRoom.itemAtLocation.size + " " + locInRoom.itemAtLocation.material + " object to the East");
                        }
                        if (locInRoom is Door)
                        {
                            Door door = locInRoom as Door;
                            if (door.keyCardColor != "none")
                            {
                                logList.Add("The scan detects a " + door.keyCardColor + " door to the East");
                            }

                        }
                        else if (locInRoom is Boulder || locInRoom is Hole)
                        {
                            logList.Add("The scan detects " + locInRoom.description + " to the East");
                        }
                    }
                    else if (row < currentRow && col < currentCol)
                    {
                        //the item is to the north west
                        if (locInRoom.itemAtLocation != null)
                        {
                            logList.Add("The scan detects a " + locInRoom.itemAtLocation.size + " " + locInRoom.itemAtLocation.material + " object to the North-West");
                        }
                        if (locInRoom is Door)
                        {
                            Door door = locInRoom as Door;
                            if (door.keyCardColor != "none")
                            {
                                logList.Add("The scan detects a " + door.keyCardColor + " door to the North-West");
                            }

                        }
                        else if (locInRoom is Boulder || locInRoom is Hole)
                        {
                            logList.Add("The scan detects " + locInRoom.description + " to the North-West");
                        }
                    }
                    else if (row < currentRow && col > currentCol)
                    {
                        //the item is to the north east
                        if (locInRoom.itemAtLocation != null)
                        {
                            logList.Add("The scan detects a " + locInRoom.itemAtLocation.size + " " + locInRoom.itemAtLocation.material + " object to the North-East");
                        }
                        if (locInRoom is Door)
                        {
                            Door door = locInRoom as Door;
                            if (door.keyCardColor != "none")
                            {
                                logList.Add("The scan detects a " + door.keyCardColor + " door to the North-East");
                            }

                        }
                        else if (locInRoom is Boulder || locInRoom is Hole)
                        {
                            logList.Add("The scan detects " + locInRoom.description + " to the North-East");
                        }
                    }
                    else if (row > currentRow && col < currentCol)
                    {
                        //the item is to the south west
                        if (locInRoom.itemAtLocation != null)
                        {
                            logList.Add("The scan detects a " + locInRoom.itemAtLocation.size + " " + locInRoom.itemAtLocation.material + " object to the South-West");
                        }
                        if (locInRoom is Door)
                        {
                            Door door = locInRoom as Door;
                            if (door.keyCardColor != "none")
                            {
                                logList.Add("The scan detects a " + door.keyCardColor + " door to the South-West");
                            }

                        }
                        else if (locInRoom is Boulder || locInRoom is Hole)
                        {
                            logList.Add("The scan detects " + locInRoom.description + " to the South-West");
                        }
                    }
                    else if (row > currentRow && col > currentCol)
                    {
                        //the item is to the south east
                        if (locInRoom.itemAtLocation != null)
                        {
                            logList.Add("The scan detects a " + locInRoom.itemAtLocation.size + " " + locInRoom.itemAtLocation.material + " object to the South-East");
                        }
                        if (locInRoom is Door)
                        {
                            Door door = locInRoom as Door;
                            if (door.keyCardColor != "none")
                            {
                                logList.Add("The scan detects a " + door.keyCardColor + " door to the South-East");
                            }                        
                        }
                        else if (locInRoom is Boulder || locInRoom is Hole)
                        {
                            logList.Add("The scan detects " + locInRoom.description + " to the South-East");
                        }
                    }

                }
            }
        }
        logList.Add("Room scan complete");
    }

    void push()
    {
        WorldLocation locToPush = null;
        WorldLocation pushDest = null;

        Vector2 pushCoords = Vector2.zero;
        Vector2 pushEndCoords = Vector2.zero;

        switch (facing)
        {
            case ("north"):
                locToPush = currentRoom[currentRow - 1, currentCol];
                pushCoords = new Vector2(currentRow - 1, currentCol);
                if(currentRow - 2 >= 0)
                {
                    pushDest = currentRoom[currentRow - 2, currentCol];
                    pushEndCoords = new Vector2(currentRow - 2, currentCol);
                }
                break;
            case ("south"):
                locToPush = currentRoom[currentRow + 1, currentCol];
                pushCoords = new Vector2(currentRow + 1, currentCol);
                if (currentRow + 2 <= currentRoom.GetLength(0))
                {
                    pushDest = currentRoom[currentRow + 2, currentCol];
                    pushEndCoords = new Vector2(currentRow + 2, currentCol);
                }
                break;
            case ("east"):
                locToPush = currentRoom[currentRow, currentCol + 1];
                pushCoords = new Vector2(currentRow, currentCol + 1);
                if (currentCol + 2 <= currentRoom.GetLength(1))
                {
                    pushDest = currentRoom[currentRow, currentCol + 2];
                    pushEndCoords = new Vector2(currentRow, currentCol + 2);
                }
                break;
            case ("west"):
                locToPush = currentRoom[currentRow, currentCol - 1];
                pushCoords = new Vector2(currentRow, currentCol - 1);
                if (currentCol - 2 >= 0)
                {
                    pushDest = currentRoom[currentRow, currentCol - 2];
                    pushEndCoords = new Vector2(currentRow, currentCol - 2);
                }
                break;
        }

        if(locToPush.isPushable == true)
        {
            if (pushDest.GetType() == new Floor().GetType())
            {
                currentRoom[(int)pushCoords.x, (int)pushCoords.y] = new Floor();
                currentRoom[(int)pushEndCoords.x, (int)pushEndCoords.y] = new Boulder();
                //locToPush = new Floor();
                //pushDest = new Boulder();
                logList.Add("Push success: The drone pushes the boulder forward one meter");
                
            }
            else if(pushDest.GetType() == new Hole().GetType())
            {
                currentRoom[(int)pushCoords.x, (int)pushCoords.y] = new Floor();
                currentRoom[(int)pushEndCoords.x, (int)pushEndCoords.y] = new Floor();
                //locToPush = new Floor();
                //pushDest = new Floor();
                logList.Add("Push success: The drone pushes the boulder forward one meter.");
                logList.Add("The boulder falls into a hole, filling it.");
            }
            else
            {
                logList.Add("Push failure: The boulder doesn't budge, something is blocking it on the other side");
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(locToPush.description))
                logList.Add("Push failure: The drone pushes against the " + locToPush.description + " but it does not move.");
            else
                logList.Add("Push failure: There is nothing for the drone to push here.");
        }
    }

    void toss()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
