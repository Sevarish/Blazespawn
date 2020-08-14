using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWallField : MonoBehaviour
{
    [SerializeField] private GameObject WallObject, WallStorage, CheckerObject;
    private List<GameObject> wallContainer = new List<GameObject>();
    private Vector3 startPosition;
    private GameObject startWall;
    public void SpawnField(float gridX, float gridZ)
    {
        //Start Positions, and current positions for the current spawn position for the wall grid.
        float startPointX = -gridX / 2 + 5,
              startPointZ = -gridZ / 2 + 5,
              currentPosX = startPointX,
              currentPosZ = startPointZ;

        //Spawns a column of walls, then resets to start position of the column and adds a row. Repeat till size is met.
        for (int i = 0; i < gridZ / 10; i++)
        {
            for (int j = 0; j < gridX / 10; j++)
            {
                var wall = Instantiate(WallObject, new Vector3(currentPosX, 5.5f, currentPosZ), Quaternion.identity);
                wallContainer.Add(wall);
                wall.transform.parent = WallStorage.transform;
                currentPosX += 10;
            }
            currentPosX = startPointX;
            currentPosZ += 10;
        }

    }

    //Returns the wallContainer, containing all the walls in the grid.
    public List<GameObject> GetList()
    {
        return wallContainer;
    }

    public void EmptyList()
    {
        wallContainer.Clear();
    }

    //Generates a start position along the Z axis and instantiates the object that is responsible for checking walls/paths in the maze.
    public void GenerateStartPosition(float gridScaleX, float gridScaleZ)
    {
        //Picks for X a position 1 block away from the edge, and for Z a position at least 1 block away from the edge (either side).
        float startPointX = (gridScaleX / 2) - 15,
              startPointZ = Random.Range((-gridScaleZ / 2) + 10, (gridScaleZ / 2) - 10);

        RaycastHit hit;
        //Casts a Raycast and checks if and which object tagged "wall" it hits. If it somehow fails, defaults startPoint Z to 0.
        if (Physics.Raycast(new Vector3(startPointX, 1, startPointZ), transform.forward * 2f, out hit, 10, 1) && hit.transform.tag == "Wall")
        {
            startPointZ = hit.transform.position.z;
            startWall = hit.transform.gameObject;
            Destroy(hit.transform.gameObject);
        }
        else
        {
            startPointZ = 0;
        }

        startPosition = new Vector3(startPointX, 5, startPointZ);
        Debug.Log(startPosition);
        Instantiate(CheckerObject, startPosition, Quaternion.identity);
    }

    public Vector3 GetStartPosition()
    {
        return startPosition;
    }
    
    public GameObject GetCheckObject()
    {
        return CheckerObject;
    }
}
