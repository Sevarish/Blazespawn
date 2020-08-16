using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotFire : MonoBehaviour
{
    float durationTimer, duration, damageTimer, damageInterval;
    int damage;
    HealthSystem hpSys;

    public void InitiatalSetup(float dt, int dmg, float interval)
    {
        duration = dt;
        damage = dmg;
        damageInterval = interval;
        damageTimer = interval;
        hpSys = GetComponent<HealthSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        durationTimer += Time.deltaTime;
        if (durationTimer < duration)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer > damageInterval)
            {
                hpSys.TakeDamage(damage, "Fire");
                damageTimer = 0;
            }
        }
        else
        {
            Destroy(this);
        }
    }
}
