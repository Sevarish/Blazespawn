using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOneBehaviour : MonoBehaviour
{
    Animator anim;
    Rigidbody rigB;
    float timer, movementTimer, attackTimer, movementSpeed = 10;
    bool attackActive = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        movementTimer += Time.deltaTime;
        if (movementTimer < 5)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
            if (movementTimer > 10)
            {
                movementTimer = 0;
            }
        }
        transform.Translate(0, 0, movementSpeed * Time.deltaTime);

        timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.P) && timer > 1.6f)
        {
            anim.Play("Attack");
            attackActive = true;
            timer = 0;
        }
        else
        {
            if (attackTimer > 1.6f)
            {
                anim.Play("Walk");
            }
        }

    }

    private void Gravity()
    {
        //rigB.AddForce(0, 20, 0);
    }
}
