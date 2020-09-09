using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindArtileryBehaviour : MonoBehaviour
{
    public int damage;
    private float speed = 30;
    private float hitBoxX, hitboxZ, hitboxY, randomX, randomY;
    public GameObject target;
    void Start()
    {
        hitBoxX = target.transform.localScale.x;

        randomX = Random.Range(-500, 500);
        randomY = Random.Range(-500, 500);
        GetComponent<Rigidbody>().AddForce(0, randomY, randomX);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Move();
            Check();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Move()
    {
        Vector3 targetDirection = Vector3.RotateTowards(transform.forward, (target.transform.position - this.transform.position), 1, 0);
        this.transform.rotation = Quaternion.LookRotation(targetDirection);
        this.transform.Translate(0, 0, speed * Time.deltaTime);
    }

    private void Check()
    {
        float X = target.transform.position.x - transform.position.x,
              Y = target.transform.position.y - transform.position.y,
              Z = target.transform.position.z - transform.position.z;
        if (X < hitBoxX && Y < hitboxY && Z < hitboxZ)
        {
            target.GetComponent<HealthSystem>().TakeDamage(damage, "Psychic");
            Destroy(this.gameObject);
        }
    }
}
