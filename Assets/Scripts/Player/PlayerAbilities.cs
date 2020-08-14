using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{

    private List<System.Action> Rotation1 = new List<System.Action>(),
                                Rotation2 = new List<System.Action>(),
                                Rotation3 = new List<System.Action>();
    private AbilitiesList abilityRef; 
    void Start()
    {
        abilityRef = GetComponent<AbilitiesList>();
        Rotation1.Add(abilityRef.Firestream);
        Rotation1.Add(abilityRef.BlazeBurst);
        Rotation1.Add(abilityRef.StargazeBomb);
        Rotation1.Add(abilityRef.Recovery);

        Rotation2.Add(abilityRef.Freezeblow);
        Rotation2.Add(abilityRef.Shockthrive);

        Rotation3.Add(abilityRef.Bloodsurge);
        abilityRef.SetAbilityList(Rotation1, Rotation2, Rotation3);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
