using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    AudioSource audioSrc;
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBasicAttack()
    {
        audioSrc.Play();
    }

    public void StopBasicAttack()
    {
        audioSrc.Stop();
    }
}
