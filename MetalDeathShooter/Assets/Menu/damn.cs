using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damn : MonoBehaviour
{
    public GameObject idiot;
    public AudioSource sound;
    public void exit()
    {
        idiot.SetActive(true);
        sound.Play();
    }
}
