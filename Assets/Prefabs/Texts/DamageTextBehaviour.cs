using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextBehaviour : MonoBehaviour
{
    Camera cam;
    public GameObject Target;
    Text textComp;
    float awayX, awayY, awaySpeedX, awaySpeedY;
    int textSize = 20;
    RectTransform rectT;
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        textComp = GetComponent<Text>();
        rectT = textComp.GetComponent<RectTransform>();
        awaySpeedY = Random.Range(-4f, 4f);
        awaySpeedX = Random.Range(-4f, 4f);
    }

    // Update is called once per frame
    void Update()
    {
        awayX += awaySpeedX;  
        awayY += awaySpeedY;
        if (Target != null)
        {
            this.transform.position = cam.WorldToScreenPoint(new Vector3(Target.transform.position.x, Target.transform.position.y, Target.transform.position.z));
        }
        rectT.position = new Vector3(rectT.position.x + awayX, rectT.position.y + awayY + awayY, rectT.position.z);
        

        textSize += 2;
        textComp.fontSize = textSize;
    }
}
