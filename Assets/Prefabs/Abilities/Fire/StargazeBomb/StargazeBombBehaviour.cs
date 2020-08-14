using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StargazeBombBehaviour : MonoBehaviour
{
    [SerializeField] GameObject Explosion;
    float movementSpeed = 60, rotSpeed = 90, timer;
    readonly int damage = 35;
    bool timerStart = false;
    public string targetTeam = "";
    private List<GameObject> EnteredInTrigger = new List<GameObject>();
    Camera cam;
    BoxCollider boxC;
    Rigidbody rigB;
    void Start()
    {
        boxC = GetComponent<BoxCollider>();
        rigB = GetComponent<Rigidbody>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        boxC.size = new Vector3(0.1f, 0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, movementSpeed * Time.deltaTime);
        rigB.AddForce(new Vector3(0, -50, 0));
        if (timerStart)
        {
            timer += Time.deltaTime;
            if (timer > 0.1f)
            {
                for (int i = 0; i < EnteredInTrigger.Count; i++)
                {
                    if (EnteredInTrigger[i])
                    {
                        if (EnteredInTrigger[i].tag == "Enemy")
                        {
                            EnteredInTrigger[i].GetComponent<HealthSystem>().TakeDamage(damage);
                        }
                    }
                }
                timerStart = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!EnteredInTrigger.Contains(other.gameObject))
        {
        EnteredInTrigger.Add(other.gameObject);
        }

        if (other.tag != "PlayerChar")
        {
            var exp = Instantiate(Explosion, this.transform.position, Quaternion.identity);
            Destroy(exp, 0.5f);
            timerStart = true;
            Destroy(this.gameObject, 0.3f);
            movementSpeed = 0;

            boxC.size = new Vector3(15f, 15f, 15f);
        }

        other = null;
    }
}
