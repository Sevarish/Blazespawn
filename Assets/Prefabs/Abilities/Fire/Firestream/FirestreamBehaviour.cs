using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirestreamBehaviour : MonoBehaviour
{

    public int damage;
    private float projectileSpeed = 45;
    void Start()
    {
        Destroy(this.gameObject, 0.6f);
    }

    void Update()
    {
        transform.Translate(0, 0, projectileSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage, "Fire");
            projectileSpeed = 0;
            Destroy(this.gameObject, 0.1f);
        }

        collision = null;
    }
}
