using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PathBehavior pathBehavior;
    public GameObject bar;
    public GameObject troopSpawner;
    // Start is called before the first frame update
    void Start()
    {
        // link up the event to show the placeenemy bar when the path is confirmed
        pathBehavior.onPathConfirmed += OnPathConfirmed;

        // set up a reference to and then disable the bar
        bar.SetActive(false);

        // run onWaveComplete when the core ship is destroyed
        GameObject.Find("CoreShip").GetComponent<TowerHealth>().onDestroy += () => OnWaveComplete();

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
        // hide the troop spawning bar
        bar.SetActive(false);

        // remove all active troops
        foreach(Transform child in troopSpawner.transform)
        {
            Destroy(child.gameObject);
        }

        // reset the path
        pathBehavior.ResetPath();
    }
}
