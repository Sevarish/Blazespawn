using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCheck : MonoBehaviour
{
    private GameObject touchObject;

    private void OnTriggerEnter(Collider obj)
    {
        touchObject = obj.gameObject;
    }

    public GameObject GetTouchedObject()
    {
        return touchObject;
    }

    public void SetNull()
    {
        touchObject = null;
    }
}
