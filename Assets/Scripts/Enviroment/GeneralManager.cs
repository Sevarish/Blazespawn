using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Physics.gravity = new Vector3(0, -50, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
