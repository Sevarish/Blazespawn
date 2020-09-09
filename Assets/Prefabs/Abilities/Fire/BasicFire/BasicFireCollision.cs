using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFireCollision : MonoBehaviour
{
    //public string teamTarget = "";
    public float speed = 60;
    public int damage;
    private void Start()
    {
        transform.parent = GameObject.Find("ColliderContainer").GetComponent<Transform>();
        Destroy(this.gameObject, 0.5f);
    }

    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage, "Fire");
            Destroy(this.gameObject);
        }

        collision = null;
    }
}
