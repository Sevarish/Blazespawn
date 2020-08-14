using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Setup : MonoBehaviour
{
    [SerializeField] private GameObject Grid;
    private SpawnWallField SpawnWallRef;
    void Start()
    {
        ResetGame();
    }

    public void ResetGame()
    {
        //References SpawnWallRef to the SpawnWallField script that is on this Game Object.
        SpawnWallRef = this.GetComponent<SpawnWallField>();

        //If objects in the wall list exist, Destroy them all and clear the list. Also Destroys the Player object and Finish object.
        if (SpawnWallRef.GetList().Count > 0)
        {
            Destroy(GameObject.Find("Player(Clone)").gameObject, 0.01f);
            Destroy(GameObject.Find("SetupManager").GetComponent<MazeCreator>().finish);
            GameObject.Find("SetupManager").GetComponent<MazeCreator>().EmptyList();

            List<GameObject> walls = SpawnWallRef.GetList();
            for (int i = 0; i < walls.Count; i++) 
            {
                Destroy(walls[i]);
            }
            SpawnWallRef.EmptyList();
        }

        //Grid.transform.localScale = new Vector3(250, 1, 250);

        //This will spawn the inital "field" of walls.
        SpawnWallRef.SpawnField(Grid.transform.localScale.x, Grid.transform.localScale.z);

        //Generates a start position for the maze, retrievable with GetStartPosition().
        SpawnWallRef.GenerateStartPosition(Grid.transform.localScale.x, Grid.transform.localScale.z);

        //Starts the setup for the MazeCreator script that is on this Game Object.
        this.GetComponent<MazeCreator>().Setup(SpawnWallRef.GetList(), SpawnWallRef.GetStartPosition());
    }
}
