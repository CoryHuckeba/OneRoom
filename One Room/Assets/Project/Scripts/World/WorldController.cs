using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldLocation
{
    public bool isPassable = true;
    public bool isInteractable = false;
    public WorldItem itemAtLocation;
    public string description = "";
}

public class Door : WorldLocation
{
    public bool isOpen = false;
    public int keyCode;
    public int keyCardLevel;
    public WorldLocation[,] nextRoom;
    public int nextRoomRow;
    public int nextRoomCol;

    //NOTE(Dylan): this likely will need to return a string - or maybe we can pass in a reference to a log object
    //for the door to write it's log entry into. 
    public string open(int keyCode, int keyCard)
    {
        if (keyCard >= this.keyCardLevel)
        {
            if (keyCode == this.keyCode)
            {
                this.isOpen = true;
                this.isPassable = true;
                //TODO(Dylan): create a log for opening the door. 
                return "Entry Authorized, opening door.";
            }
            else
            {
                if (keyCode == 0)
                {
                    //the drone didn't provide a key code.

                    //TODO(Dylan): create a log for a door open failure. 
                    return "Failed to open door: 'Error, entry not authorized. Please input a valid key code.'";
                }
                //TODO(Dylan): create a log for a door open failure. 
                return "Failed to open door: 'Error, entry not authorized. Key code incorrect.'";
            }
        }
        else
        {
            //TODO(Dylan): create a log for a door open failure
            return "Failed to open door: 'Error, entry not authorized. Level 2 Key card required.'";
        }
    }

    public Door(WorldLocation[,] nextRoom, int keyCode = 0, int keyCardLevel = 0, bool isOpen = false)
    {
        this.nextRoom = nextRoom;
        this.keyCode = keyCode;
        this.keyCardLevel = keyCardLevel;
        this.isOpen = isOpen;
        this.isPassable = this.isOpen ? true : false;
    }

    public Door()
    {
        this.nextRoom = null;
        this.keyCode = 0;
        this.keyCardLevel = 0;
        this.isOpen = false;
        this.isPassable = true;//this.isOpen ? true : false;
    }
}

public class Wall : WorldLocation
{
    public Wall()
    {
        this.isPassable = false;
        this.isInteractable = false;
    }
}

public class Obstacle : WorldLocation
{
    public Obstacle()
    {
        this.isPassable = false;
        this.isInteractable = false;
    }
}

public class Floor : WorldLocation
{
    public Floor()
    {
        this.isPassable = true;
        this.isInteractable = false;
    }
}


public class WorldController : MonoBehaviour
{

    public int[,] Room1Map = new int[,] {
        {1, 1, 2, 1, 1},
        {1, 0, 0, 0, 1},
        {1, 0, 0, 0, 1},
        {1, 0, 0, 0, 1},
        {1, 1, 2, 1, 1}
    };

    public int[,] Room2Map = new int[,] {
        {1, 1, 1, 1, 1},
        {1, 0, 0, 0, 1},
        {1, 0, 0, 0, 2},
        {1, 0, 0, 0, 1},
        {1, 1, 2, 1, 1}
    };

    public int[,] Room3Map = new int[,] {
        {1, 1, 2, 1, 1},
        {1, 0, 0, 0, 1},
        {2, 0, 0, 0, 1},
        {1, 0, 0, 0, 1},
        {1, 1, 2, 1, 1}
    };

    public int[,] Room4Map = new int[,] {
        {1, 1, 1, 1, 1},
        {1, 0, 0, 0, 1},
        {1, 0, 0, 0, 1},
        {1, 0, 0, 0, 1},
        {1, 1, 2, 1, 1}
    };

    public int[,] Room5Map = new int[,] {
        {1, 1, 2, 1, 1},
        {1, 0, 0, 0, 1},
        {1, 0, 0, 0, 2},
        {1, 0, 0, 0, 1},
        {1, 1, 1, 1, 1}
    };

    public int[,] Room6Map = new int[,] {
        {1, 1, 1, 1, 1},
        {1, 0, 0, 0, 1},
        {2, 0, 0, 0, 1},
        {1, 0, 0, 0, 1},
        {1, 1, 1, 1, 1}
    };

    public WorldLocation[,] Room1;
    public WorldLocation[,] Room2;
    public WorldLocation[,] Room3;
    public WorldLocation[,] Room4;
    public WorldLocation[,] Room5;
    public WorldLocation[,] Room6;

    public WorldLocation[,] buildRoomArray(int[,] RoomMap)
    {
        WorldLocation[,] RoomArray = new WorldLocation[RoomMap.GetLength(0), RoomMap.GetLength(1)];

        for (int i = 0; i < RoomMap.GetLength(0); i++)
        {
            for (int j = 0; j < RoomMap.GetLength(1); j++)
            {
                int thingInRoomMap = RoomMap[i, j];
                WorldLocation objectHere = new Floor();

                if (thingInRoomMap == 0)
                {
                    //objectHere = new Floor();
                }
                else if (thingInRoomMap == 1)
                {
                    objectHere = new Wall();
                }
                else if (thingInRoomMap == 2)
                {
                    objectHere = new Door();
                }
                else if (thingInRoomMap == 3)
                {
                    objectHere = new Obstacle();
                }

                RoomArray[i, j] = objectHere;
            }
        }

        return RoomArray;
    }

    public string printRoomArray(WorldLocation[,] Room)
    {
        //WorldLocation[,] RoomArray = new WorldLocation[RoomMap.GetLength(0), RoomMap.GetLength(1)];
        string roomString = "";

        for (int i = 0; i < Room.GetLength(0); i++)
        {
            for (int j = 0; j < Room.GetLength(1); j++)
            {
                WorldLocation thingInRoomMap = Room[i, j];

                WorldLocation testDoor = new Door();
                WorldLocation testWall = new Wall();
                WorldLocation testFloor = new Floor();
                WorldLocation testObstacle = new Obstacle();

                if (thingInRoomMap.GetType() == testFloor.GetType())
                {
                    roomString += "0, ";
                }
                else if (thingInRoomMap.GetType() == testWall.GetType())
                {
                    roomString += "1, ";
                }
                else if (thingInRoomMap.GetType() == testDoor.GetType())
                {
                    roomString += "2, ";
                }
                else if (thingInRoomMap.GetType() == testObstacle.GetType())
                {
                    roomString += "3, ";
                }
            }
            roomString += "\n";
        }

        return roomString;
    }

    // Use this for initialization
    void Start()
    {
        Room1 = buildRoomArray(Room1Map);
        Room2 = buildRoomArray(Room2Map);
        Room3 = buildRoomArray(Room3Map);
        Room4 = buildRoomArray(Room4Map);
        Room5 = buildRoomArray(Room5Map);
        Room6 = buildRoomArray(Room6Map);

        if (Room1[0, 2] is Door)
        {
            Door door = Room1[0, 2] as Door;
            door.nextRoom = Room2;
            door.nextRoomRow = 3;
            door.nextRoomCol = 2;
            //door.isOpen = true;
            Room1[0, 2] = door;
        }

        if (Room2[4, 2] is Door)
        {
            Door door = Room2[4, 2] as Door;
            door.nextRoom = Room1;
            Room2[4, 2] = door;
        }

        if (Room2[2, 4] is Door)
        {
            Door door = Room2[2, 4] as Door;
            door.nextRoom = Room3;
            Room2[2, 4] = door;
        }

        if (Room3[0, 2] is Door)
        {
            Door door = Room3[0, 2] as Door;
            door.nextRoom = Room2;
            Room3[0, 2] = door;
        }

        if (Room3[2, 0] is Door)
        {
            Door door = Room3[2, 0] as Door;
            door.nextRoom = Room4;
            Room3[2, 0] = door;
        }

        if (Room3[4, 2] is Door)
        {
            Door door = Room3[4, 2] as Door;
            door.nextRoom = Room5;
            Room3[4, 2] = door;
        }

        if (Room4[4, 2] is Door)
        {
            Door door = Room4[4, 2] as Door;
            door.nextRoom = Room3;
            Room4[4, 2] = door;
        }

        if (Room5[0, 2] is Door)
        {
            Door door = Room5[0, 2] as Door;
            door.nextRoom = Room4;
            Room5[0, 2] = door;
        }

        if (Room5[2, 4] is Door)
        {
            Door door = Room5[2, 4] as Door;
            door.nextRoom = Room6;
            door.keyCode = 1234;
            door.keyCardLevel = 4;
            door.open(1234, 3);
            Room5[4, 2] = door;
        }

        if (Room6[0, 2] is Door)
        {
            Door door = Room6[0, 2] as Door;
            door.nextRoom = Room5;
            Room6[0, 2] = door;
        }

        /*
        Debug.Log("Room1:");
        Debug.Log(printRoomArray(Room1));
        Debug.Log("Room2:");
        Debug.Log(printRoomArray(Room2));
        Debug.Log("Room3:");
        Debug.Log(printRoomArray(Room3));
        Debug.Log("Room4:");
        Debug.Log(printRoomArray(Room4));
        Debug.Log("Room5:");
        Debug.Log(printRoomArray(Room5));
        Debug.Log("Room6:");
        Debug.Log(printRoomArray(Room6));
        */

        if (Room5[2, 4] is Door)
        {
            Door door = Room5[2, 4] as Door;
            //Debug.Log("Door is open: " + door.isOpen);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}