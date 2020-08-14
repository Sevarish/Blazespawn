using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodsurgeParticle : MonoBehaviour
{
    private float rotationX = 90, initialSpeed = 15, speed = 85, timer;
    private Rigidbody rigB;
    private Transform playerTransform;
    private PlayerHealthSystem playerHp;
    void Start()
    {
        rigB = GetComponent<Rigidbody>();
        float startRotation = Random.Range(0, 360);
        this.transform.rotation = Quaternion.Euler(0, 0, 90);
        this.transform.rotation = Quaternion.Euler(0, startRotation, 0);
        playerTransform = GameObject.FindGameObjectWithTag("PlayerChar").transform;
        playerHp = playerTransform.GetComponent<PlayerHealthSystem>();
    }

    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < 0.3f)
        {
            this.transform.Translate(0, 0, 5 * Time.deltaTime);
            this.transform.Translate(initialSpeed * Time.deltaTime, 0, 0);
        }
        else
        {
            Vector3 targetDirection = Vector3.RotateTowards(transform.forward, (playerTransform.position - this.transform.position), 1, 0);
            this.transform.rotation = Quaternion.LookRotation(targetDirection);
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }

        float x = playerTransform.position.x - this.transform.position.x,
              y = playerTransform.position.y - this.transform.position.y,
              z = playerTransform.position.z - this.transform.position.z;
        if (x < 0)
        {
            x = -x;
        }
        if (y < 0)
        {
            y = -y;
        }
        if (z < 0)
        {
            z = -z;
        }
        if (x < 1 && y < 1 && z < 1)
        {
            playerHp.Heal(1);
            Destroy(this.gameObject);
        }
    }
}
