using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCreator : MonoBehaviour
{
    private bool startAlgoritmh = false;
    private bool backtrack = false;
    private bool needsReset = true;
    private bool algorithmInitiated = false;
    private int currentVisitedIndex = 0;
    private List<GameObject> wallContainer;
    private Vector3 startPosition, currentPosition1;
    private List<Vector3> possibleObjects = new List<Vector3>();
    private List<Vector3> visited = new List<Vector3>();
    private LayerMask layerMask = 1 << 9;
    private GameObject CheckerObject;
    public GameObject finish;
    [SerializeField] GameObject Player;
    private CheckColliders checkRef;

    public void Setup(List<GameObject> wallList, Vector3 StartPos)
    {
        wallContainer = wallList;
        startPosition = StartPos;
        CheckerObject = GameObject.Find("MazeChecker(Clone)");
        checkRef = CheckerObject.GetComponent<CheckColliders>();
        checkRef.AssignColliders();
        Invoke("StartAlgorithm", 0.5f);
    }

    void Update()
    {
        if (algorithmInitiated)
        {
            if (startAlgoritmh) 
            {
                CheckSurround();
            }
            if (backtrack)
            {
                BackTrack();
            }
        }
    }

    //Starts algorithm after setup.
    private void StartAlgorithm()
    {
        CheckSurround();
        algorithmInitiated = true;
        startAlgoritmh = true;
    }

    //nextPos asks for an object in possibleObjects. That object will be the next position for the CheckerObject. If there were no objects available in possibleObjects, it starts backtracking.
    private void CheckSurround()
    {
        var nextPos = checkRef.Next();
        Debug.Log(checkRef.Next());
        if (nextPos == null)
        {
            backtrack = true;
            startAlgoritmh = false;
        }
        else
        {   
            startAlgoritmh = true;
            backtrack = false;
            needsReset = true;
            NextPosition(nextPos);
        }
    }

    //Updates CheckerObject's position, runs DeleteWall with the to-be deleted wall as argument. Also clears the possibleObjects list.
    private void NextPosition(GameObject obj)
    {
        CheckerObject.transform.position = new Vector3(obj.gameObject.transform.position.x, 5, obj.gameObject.transform.position.z);
        visited.Add(obj.gameObject.transform.position);
        DeleteWall(obj);
        checkRef.possibleObjects.Clear();
    }

    //This uses recursive backtracking. When it has no possible spaces to go to, currentVisitedIndex will be equal to the length of visited (list).
    //If it doesn't need a reset, every frame it will subtract 1 from currentVisitedIndex. It will move 1 space back in the list, checks around the CheckerObject in the Update and
    //repeats this process until no spots in the maze is left. Then it runs FinishedMaze().
    private void BackTrack()
    {
        if (needsReset)
        {
             currentVisitedIndex = visited.Count;
             needsReset = false;
        }
        else
        {
            currentVisitedIndex -= 1;
        }

        if (currentVisitedIndex > -1)
        {
            CheckerObject.transform.position = new Vector3(visited[currentVisitedIndex].x, 2, visited[currentVisitedIndex].z);
        }
        else
        {
            FinishedMaze();
        }
        CheckSurround();
    }

    //Deletes the wall on the position CheckerObject needs to go.
    private void DeleteWall(GameObject wall)
    {
        Destroy(wall);
    }

    public void EmptyList()
    {
        visited.Clear();
    }

    //This is run when the maze is finished.
    private void FinishedMaze()
    {
        //Have it delete random blocks

        startAlgoritmh = false;
        backtrack = false;
        algorithmInitiated = false;

        Destroy(CheckerObject);

        Instantiate(Player, new Vector3(startPosition.x, 2, startPosition.z), Quaternion.identity);
    }
}
