using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{
    public enum MapType
    {
        Seeded,
        Random,
        MapOfTheDay
    }

    public MapType mapType = MapType.Random;

    public int mapSeed;

    public int rows;

    public int columns;

    private float roomWidth = 50f;

    private float roomHeight = 50f;

    public GameObject[] gridPrefabs;

    public Room[,] grid;
    // Start is called before the first frame update
    void Start()
    {
        switch (mapType)
        {
            case MapType.MapOfTheDay:
                mapSeed = DateToInt(DateTime.Now.Date);
                break;
            case MapType.Random:
                mapSeed = DateToInt(DateTime.Now);
                break;
            case MapType.Seeded:
                break;
            default:
                Debug.LogError("[MapGenerator] Type not Implemented");
                break;
        }
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject RandomRoom()
    {
        return gridPrefabs[UnityEngine.Random.Range(0, gridPrefabs.Length)];
    }
    public int DateToInt(DateTime dateToUse)
    {
        // Add our date up and return it
        return dateToUse.Year + dateToUse.Month + 
               dateToUse.Day + dateToUse.Hour + 
               dateToUse.Minute + dateToUse.Second + 
               dateToUse.Millisecond;
    }
    public void GenerateGrid()
    {
        UnityEngine.Random.seed = mapSeed;
        grid = new Room[columns,rows];
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                float xPos = roomWidth * column;
                float zPos = roomHeight * row;
                Vector3 newPos = new Vector3(xPos, 0, zPos);
                GameObject tempObject = Instantiate(RandomRoom(), newPos, Quaternion.identity) as GameObject;
                tempObject.transform.parent = this.transform;
                tempObject.name = "Room_" + row + "," + column;
                Room tempRoom = tempObject.GetComponent<Room>();


                if (row == 0)
                {
                    tempRoom.doorNorth.SetActive(false);
                }
                else if (row == rows - 1)
                {
                    tempRoom.doorSouth.SetActive(false);
                }
                else
                {
                    tempRoom.doorNorth.SetActive(false);
                    tempRoom.doorSouth.SetActive(false);
                }


                if (column == 0)
                {
                    tempRoom.doorEast.SetActive(false);
                }
                else if (column == columns - 1)
                {
                    tempRoom.doorWest.SetActive(false);
                }
                else
                {
                    tempRoom.doorEast.SetActive(false);
                    tempRoom.doorWest.SetActive(false);
                }
                grid[column, row] = tempRoom;
            }
        }
    }
}
