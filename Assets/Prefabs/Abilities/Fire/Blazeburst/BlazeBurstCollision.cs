using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlazeBurstCollision : MonoBehaviour
{
    public string teamTarget = "";
    public float speed = 100, x = 1, y = 1, growthTimer;
    public int damage;
    BoxCollider boxCol;
    private void Start()
    {
        transform.parent = GameObject.Find("ColliderContainer").GetComponent<Transform>();
        boxCol = GetComponent<BoxCollider>();
        Destroy(this.gameObject, 0.3f);
    }

    void Update()
    {
        growthTimer += Time.deltaTime;
        transform.Translate(0, 0, speed * Time.deltaTime);
        x = growthTimer * 100;
        y = growthTimer * 30;
        boxCol.size = new Vector3(x, y, 1);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == teamTarget)
        {
            collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage);
        }

        collision = null;
    }
}
