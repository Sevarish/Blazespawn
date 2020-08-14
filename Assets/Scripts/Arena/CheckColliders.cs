using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckColliders : MonoBehaviour
{
    private TouchCheck right1, right2, left1, left2, up1, up2, down1, down2, rightdown, rightup, leftdown, leftup;
    public List<GameObject> possibleObjects = new List<GameObject>();
    private List<TouchCheck> touchCheckColliders = new List<TouchCheck>();

    //Assigns all Box Colliders to variables.
    public void AssignColliders()
    {
        right1 = this.transform.GetChild(0).GetComponent<TouchCheck>(); touchCheckColliders.Add(right1);
        right2 = this.transform.GetChild(1).GetComponent<TouchCheck>(); touchCheckColliders.Add(right2);
        left1 = this.transform.GetChild(2).GetComponent<TouchCheck>(); touchCheckColliders.Add(left1);
        left2 = this.transform.GetChild(3).GetComponent<TouchCheck>(); touchCheckColliders.Add(left2);
        up1 = this.transform.GetChild(4).GetComponent<TouchCheck>(); touchCheckColliders.Add(up1);
        up2 = this.transform.GetChild(5).GetComponent<TouchCheck>(); touchCheckColliders.Add(up2);
        down1 = this.transform.GetChild(6).GetComponent<TouchCheck>(); touchCheckColliders.Add(down1);
        down2 = this.transform.GetChild(7).GetComponent<TouchCheck>(); touchCheckColliders.Add(down2);
        rightdown = this.transform.GetChild(8).GetComponent<TouchCheck>(); touchCheckColliders.Add(rightdown);
        rightup = this.transform.GetChild(9).GetComponent<TouchCheck>(); touchCheckColliders.Add(rightup);
        leftdown = this.transform.GetChild(10).GetComponent<TouchCheck>(); touchCheckColliders.Add(leftdown);
        leftup = this.transform.GetChild(11).GetComponent<TouchCheck>(); touchCheckColliders.Add(leftup);
    }

    //Checks every side with 4 colliders ( 1 position away, 2 positions away, and both corners), checks which spot is possible to move to,
    //randomly chooses one of the spots and returns the GameObject on that spot.
    public GameObject Next()
    {
        if (right2.GetTouchedObject() != null && rightdown.GetTouchedObject() != null && rightup.GetTouchedObject() != null)
        {
            if (right1.GetTouchedObject() != null && right1.GetTouchedObject().tag == "Wall")
            {
                possibleObjects.Add(right1.GetTouchedObject());
            }
        }
        if (left2.GetTouchedObject() != null && leftdown.GetTouchedObject() != null && leftup.GetTouchedObject() != null)
        {
            if (left1.GetTouchedObject() != null && left1.GetTouchedObject().tag == "Wall")
            {
                possibleObjects.Add(left1.GetTouchedObject());
            }
        }
        if (up2.GetTouchedObject() != null && rightup.GetTouchedObject() != null && leftup.GetTouchedObject() != null)
        {
            if (up1.GetTouchedObject() != null && up1.GetTouchedObject().tag == "Wall")
            {
                possibleObjects.Add(up1.GetTouchedObject());
            }
        }
        if (down2.GetTouchedObject() != null && rightdown.GetTouchedObject() != null && leftdown.GetTouchedObject() != null)
        {
            if (down1.GetTouchedObject() != null && down1.GetTouchedObject().tag == "Wall")
            {
                possibleObjects.Add(down1.GetTouchedObject());
            }
        }
        //Sets all touchedObject's on all TouchCheck scripts to null.
        SetNullAllColliders();

        if (possibleObjects.Count == 0)
        {
            return null;
        } 
        else
        {
            return possibleObjects[Random.Range(0, possibleObjects.Count)];
        }
    }

    //Sets all touchedObject's on all TouchCheck scripts to null.
    private void SetNullAllColliders()
    {
        for (int i = 0; i < touchCheckColliders.Count; i++)
        {
            touchCheckColliders[i].SetNull();
        }
    }


}
