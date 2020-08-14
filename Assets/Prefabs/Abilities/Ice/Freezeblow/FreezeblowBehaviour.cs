using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeblowBehaviour : MonoBehaviour
{

    public int damage;
    private float projectileSpeed = 130;
    void Start()
    {
        Destroy(this.gameObject, 1.5f);
    }

    void Update()
    {
        transform.Translate(0, 0, projectileSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage);
            projectileSpeed = 0;
            Destroy(this.gameObject, 0.2f);
        }

        collision = null;
    }
}
