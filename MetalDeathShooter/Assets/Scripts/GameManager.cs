using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private PathBehavior pathBehavior;
    private GameObject bar;
    // Start is called before the first frame update
    void Start()
    {
        // link up the event to show the placeenemy bar when the path is confirmed
        pathBehavior = GameObject.FindWithTag("PathManager").GetComponent<PathBehavior>();
        pathBehavior.onPathConfirmed += OnPathConfirmed;

        // set up a reference to and then disable the bar
        bar = GameObject.Find("Bar");
        bar.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPathConfirmed()
    {
        bar.SetActive(true);
    }

    void OnWaveComplete()
    {
        bar.SetActive(false);
        pathBehavior.ResetPath();
    }
}
