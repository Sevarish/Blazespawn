using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockthriveBehaviour : MonoBehaviour
{
    float timer;
    public int damage;
    private List<GameObject> EnteredInTrigger = new List<GameObject>();
    bool dealtDamage = false;
    BoxCollider boxC;
    Rigidbody rigB;
    void Start()
    {
        boxC = GetComponent<BoxCollider>();
        boxC.size = new Vector3(15f, 15f, 15f);
    }

    // Update is called once per frame
    void Update()
    {
         timer += Time.deltaTime;
         if (timer > 0.2f && !dealtDamage)
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
            dealtDamage = true;
         }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!EnteredInTrigger.Contains(other.gameObject))
        {
            EnteredInTrigger.Add(other.gameObject);
        }
        other = null;
    }
}
