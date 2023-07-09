using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class butt : MonoBehaviour
{
    public GameObject anim;
    public AudioSource sound;

    [Space]
    public GameObject settingsCanvas;
    public GameObject normalCanvas;

    public void PLay()
    {
        anim.SetActive(true);
        sound.Play();
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MainScene");
    }

    public void Settings()
    {
        settingsCanvas.SetActive(true);
        normalCanvas.SetActive(false);
    }
}
