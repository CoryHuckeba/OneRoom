using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WorldLocation
{
    public bool isPassable;
    public bool isPushable;
    public WorldItem itemAtLocation;
    public string description;
}

public class Door : WorldLocation
{
    public bool isOpen = false;
    public int keyCode;
    public string keyCardColor;
    public WorldLocation[,] nextRoom;
    public int nextRoomRow;
    public int nextRoomCol;

    // NOTE(Dylan): this likely will need to return a string - or maybe we can pass in a reference to a log object
    // for the door to write it's log entry into. 
    public string open(int keyCode, string keyCardColor)
    {
        if (keyCardColor == this.keyCardColor || this.keyCardColor == "none")
        {
            if (keyCode == this.keyCode || this.keyCode == 0)
            {
                this.isOpen = true;
                this.keyCardColor = "none";
                return "Entry Authorized, opening door.";
            }
            else
            {
                if (keyCode == 0)
                {
                    return "Failed to open door: 'Error, entry not authorized. Please input a valid key code.'";
                }
                return "Failed to open door: 'Error, entry not authorized. Key code incorrect.'";
            }
        }
        else
        {
            return "Failed to open door: 'Error, entry not authorized." + this.keyCardColor + "required.";
        }
    }

    public Door(WorldLocation[,] nextRoom, int keyCode = 0, string keyCardColor = "none", bool isOpen = false)
    {
        this.nextRoom = nextRoom;
        this.keyCode = keyCode;
        this.keyCardColor = keyCardColor;
        this.isOpen = isOpen;
        this.isPassable = this.isOpen ? true : false;
    }

    public Door()
    {
        this.nextRoom = null;
        this.keyCode = 0;
        this.keyCardColor = "none";
        this.isOpen = false;
        this.isPassable = true;// NOTE(Dylan): doors are always considered passable, even if closed, unless they are collapsed or otherwise blocked. 
    }
}

public class Wall : WorldLocation
{
    public Wall()
    {
        this.isPassable = false;
        this.isPushable = false;
        this.description = "wall";
    }
}

public class Obstacle : WorldLocation
{
    public Obstacle(string description)
    {
        this.isPassable = false;
        this.isPushable = false;
        this.description = description;
    }
}

public class Boulder : WorldLocation
{
    public Boulder()
    {
        this.isPassable = false;
        this.isPushable = true;
        this.description = "a Boulder (The drone should be able to push this).";
    }
}

public class Hole : WorldLocation
{
    public bool isFilled;

    public Hole()
    {
        this.isPassable = true;
        this.isFilled = false;
        this.isPushable = false;
        this.description = "a deep fissure in the floor (The drone may be able to cross if it can be filled in).";
    }
}

public class Floor : WorldLocation
{
    public Floor()
    {
        this.isPassable = true;
        this.isPushable = false;
    }
}


public class WorldController : MonoBehaviour
{

    public int[,] Room1Map = new int[,] {
        {1, 1, 2, 1, 1},
        {1, 0, 0, 0, 1},
        {1, 0, 0, 0, 1},
        {1, 0, 0, 0, 1},
        {1, 1, 1, 1, 1}
    };

    public int[,] Room2Map = new int[,] {
        {1, 1, 1, 2, 1, 1},
        {1, 0, 0, 0, 0, 1},
        {1, 3, 0, 0, 0, 2},
        {1, 3, 3, 0, 0, 1},
        {1, 3, 3, 3, 0, 1},
        {1, 3, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 1},
        {1, 2, 1, 1, 1, 1}
    };

    public int[,] Room3Map = new int[,] {
        {1, 1, 1, 2, 1, 1, 1},
        {1, 0, 0, 4, 0, 0, 1},
        {1, 0, 0, 3, 0, 0, 2},
        {1, 0, 0, 4, 0, 0, 1},
        {1, 2, 1, 1, 1, 1, 1}
    };

    public int[,] Room4Map = new int[,] {
        {1, 1, 1, 1, 1},
        {1, 0, 0, 0, 1},
        {1, 0, 0, 0, 1},
        {1, 0, 0, 0, 1},
        {1, 1, 2, 1, 1}
    };

    public int[,] Room5Map = new int[,] {
        {1, 1, 1, 2, 1, 1, 1},
        {1, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 2},
        {1, 0, 0, 0, 0, 0, 1},
        {1, 5, 5, 5, 5, 5, 1},
        {1, 0, 0, 0, 4, 0, 1},
        {1, 0, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 1},
        {1, 1, 1, 1, 1, 1, 1}
    };

    public int[,] Room6Map = new int[,] {
        {1, 1, 1, 1},
        {1, 0, 0, 1},
        {2, 0, 0, 1},
        {1, 1, 1, 1}
    };

    public int[,] Room7Map = new int[,] {
        {0}
    };

    public WorldLocation[,] Room1;
    public WorldLocation[,] Room2;
    public WorldLocation[,] Room3;
    public WorldLocation[,] Room4;
    public WorldLocation[,] Room5;
    public WorldLocation[,] Room6;
    public WorldLocation[,] Room7;

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
                    string obsDesc = "";
                    switch (Random.Range(0, 3))
                    {
                        case (0):
                            obsDesc = "a collapsed wall";
                            break;
                        case (1):
                            obsDesc = "a pile of rubble";
                            break;
                        case (2):
                            obsDesc = "a toppled piece of heavy machinery";
                            break;
                        case (3):
                            obsDesc = "a collapsed section of concrete ceiling";
                            break;
                        default:
                            break;
                    }

                    objectHere = new Obstacle(obsDesc);
                }
                else if (thingInRoomMap == 4)
                {
                    objectHere = new Boulder();
                }
                else if (thingInRoomMap == 5)
                {
                    objectHere = new Hole();
                }

                RoomArray[i, j] = objectHere;
            }
        }

        return RoomArray;
    }

    public string printRoomArray(WorldLocation[,] Room)
    {
        string roomString = "";

        for (int i = 0; i < Room.GetLength(0); i++)
        {
            for (int j = 0; j < Room.GetLength(1); j++)
            {
                WorldLocation thingInRoomMap = Room[i, j];

                WorldLocation testDoor = new Door();
                WorldLocation testWall = new Wall();
                WorldLocation testFloor = new Floor();
                WorldLocation testObstacle = new Obstacle("derp");

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
        Room7 = buildRoomArray(Room7Map);

        // --- Room 1 ------------------------------
        if (Room1[0, 2] is Door)
        {
            Door door = Room1[0, 2] as Door;
            door.nextRoom = Room2;
            door.nextRoomRow = 6;
            door.nextRoomCol = 1;
            door.keyCardColor = "blue";
            Room1[0, 2] = door;
        }

        Room1[2, 3].itemAtLocation = new KeyCard("blue", "Blue Keycard", "A Blue Keycard assigned to Dr. Place.");

        // --- Room 2 ------------------------------

        if (Room2[7, 1] is Door)
        {
            Door door = Room2[7, 1] as Door;
            door.nextRoom = Room1;
            door.nextRoomRow = 1;
            door.nextRoomCol = 2;
            Room2[7, 1] = door;
        }

        if (Room2[0, 3] is Door)
        {
            Door door = Room2[0, 3] as Door;
            door.nextRoom = Room3;
            door.nextRoomRow = 3;
            door.nextRoomCol = 1;
            Room2[0, 3] = door;
        }

        // --- Room 3 ------------------------------

        if (Room3[4, 1] is Door)
        {
            Door door = Room3[4, 1] as Door;
            door.nextRoom = Room2;
            door.nextRoomRow = 1;
            door.nextRoomCol = 3;
            Room3[4, 1] = door;
        }

        if (Room3[0, 3] is Door)
        {
            Door door = Room3[0, 3] as Door;
            door.nextRoom = Room4;
            door.nextRoomRow = 3;
            door.nextRoomCol = 2;
            Room3[0, 3] = door;
        }

        if (Room3[2, 6] is Door)
        {
            Door door = Room3[2, 6] as Door;
            door.nextRoom = Room5;
            door.nextRoomRow = 6;
            door.nextRoomCol = 1;
            door.keyCardColor = "red";
            Room3[2, 6] = door;
        }

        // --- Room 4 ------------------------------

        if (Room4[4, 2] is Door)
        {
            Door door = Room4[4, 2] as Door;
            door.nextRoom = Room3;
            door.nextRoomRow = 1;
            door.nextRoomCol = 3;
            Room4[4, 2] = door;
        }

        Room4[1, 1].itemAtLocation = new KeyCard("red", "Red Keycard", "A Red Keycard belonging to Security Officer Reuland.");

        // --- Room 5 ------------------------------

        if (Room5[0, 3] is Door)
        {
            Door door = Room5[0, 3] as Door;
            door.nextRoom = Room7;
            door.nextRoomRow = 0;
            door.nextRoomCol = 0;
            door.keyCardColor = "gold";
            Room5[0, 3] = door;
        }

        if (Room5[6, 0] is Door)
        {
            Door door = Room5[6, 0] as Door;
            door.nextRoom = Room3;
            door.nextRoomRow = 2;
            door.nextRoomCol = 5;
            Room5[6, 0] = door;
        }

        if (Room5[2, 6] is Door)
        {
            Door door = Room5[2, 6] as Door;
            door.nextRoom = Room6;
            door.nextRoomRow = 2;
            door.nextRoomCol = 1;
            door.keyCardColor = "blue";
            Room5[2, 6] = door;
        }

        // --- Room 6 ------------------------------

        if (Room6[2, 0] is Door)
        {
            Door door = Room6[2, 0] as Door;
            door.nextRoom = Room5;
            door.nextRoomCol = 5;
            door.nextRoomRow = 2;
            Room6[2, 0] = door;
        }

        Room6[2, 2].itemAtLocation = new KeyCard("gold", "Gold Keycard", "A Gold Keycard belonging to Research Director Tavery.");

        // --- Room 7 ------------------------------


        /*
        if (Room2[1, 1] is Floor)
        {
            Room2[1, 1].itemAtLocation = new Gore();
        }
        if (Room2[3, 3] is Floor)
        {
            Room2[3, 3].itemAtLocation = new Gore();
        }
        */

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