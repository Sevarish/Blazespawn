using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSystem : MonoBehaviour
{
    float maxDistance = 40, targetLockTimer;
    GameObject currentTarget, previousTarget;
    LayerMask layerMask = 1 << 11;
    [SerializeField] Image Targetting;
    Image tg;
    Camera cam;
    void Start()
    {
        layerMask = ~layerMask;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        //{
        //   hit.point;
        //}

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, maxDistance, layerMask))
        {
            if (hit.transform.tag == "Enemy")
            {

                if (tg == null)
                {
                    currentTarget = hit.transform.gameObject;
                    tg = Instantiate(Targetting, cam.WorldToScreenPoint(currentTarget.transform.position), Quaternion.identity);
                    tg.transform.SetParent(GameObject.Find("Canvas").transform);
                }
                previousTarget = currentTarget;
            }
            else
            {
                 currentTarget = null;
                 if (tg != null)
                 {
                     Destroy(tg.gameObject);
                 }
            }
        }
        else
        {
            currentTarget = null;
            if (tg != null)
            {
                Destroy(tg);
            }
            targetLockTimer = 0;
        }
        if (tg != null && currentTarget != null)
        {
            FollowUI();
            RotateImage();
        }
    }

    private void FollowUI()
    {
            tg.transform.position = cam.WorldToScreenPoint(currentTarget.transform.position);
    }

    private void RotateImage()
    {
        tg.transform.Rotate(0, 0, 5);
    }

    public GameObject GetTarget()
    {
        return currentTarget;
    }
}
