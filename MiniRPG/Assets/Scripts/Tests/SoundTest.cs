using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public AudioClip audioClip;
    private void OnTriggerEnter(Collider other)
    {
        Manager.Sound.Play(audioClip);
    }
}
