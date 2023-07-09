using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PathBehavior pathBehavior;
    public GameObject bar;
    public GameObject troopSpawner;
    public MoneyManager moneyManager;
    public Image waveCompleteScreen;
    // Start is called before the first frame update
    void Start()
    {
        // link up the event to show the placeenemy bar when the path is confirmed
        pathBehavior.onPathConfirmed += OnPathConfirmed;

        // set up a reference to and then disable the bar
        bar.SetActive(false);

        // run onWaveComplete when the core ship is destroyed
        GameObject.Find("CoreShip").GetComponent<TowerHealth>().onDestroy += () => OnWaveComplete();


        // start wave is called when the scene is loaded

        StartWave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartWave()
    {
        // hide the troop spawning bar
        bar.SetActive(false);

        // remove all active troops
        foreach (Transform child in troopSpawner.transform)
        {
            if (child.name.Equals("SubSpawner")) continue;
            Destroy(child.gameObject);
        }

        // reset the path
        pathBehavior.ResetPath();

        // give initial money
        moneyManager.AddMoney(1000);
    }
    void OnPathConfirmed()
    {
        bar.SetActive(true);
    }

    void OnWaveComplete()
    {
        //everything that happens when a wave is completed should be done here

        // hide the troop spawning bar
        bar.SetActive(false);

        // show the UI screen that allows resetting the wave
        waveCompleteScreen.gameObject.SetActive(true);

        
    }
}
