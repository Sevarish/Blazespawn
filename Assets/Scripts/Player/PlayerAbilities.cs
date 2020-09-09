using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public delegate void Ability(string Input);
    public List<Ability> Rotation1 = new List<Ability>(),
                         Rotation2 = new List<Ability>(),
                         Rotation3 = new List<Ability>();
    private AbilitiesList abilityRef;
    private string[] inputList = {};
    void Start()
    {
        abilityRef = GetComponent<AbilitiesList>();
    }
}
