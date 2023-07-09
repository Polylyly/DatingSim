using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public PathBehavior pathBehavior;
    public GameObject bar;
    public GameObject troopSpawner;
    public MoneyManager moneyManager;
    public Image waveCompleteScreen;
    public TextMeshProUGUI text;
    public Image lossScreen;
    public TextMeshProUGUI lossText;
    public Image startScreen;
    public TextMeshProUGUI startText;
    public GameObject coreShip;
    public Vector3 coreShipPos;
    public int waveCount = 0;
    public float timer = -1f;
    public TowerGen towerGen;

    public WaveData[] waveData;
    // Start is called before the first frame update
    void Start()
    {
        // link up the event to show the placeenemy bar when the path is confirmed
        pathBehavior.onPathConfirmed += OnPathConfirmed;

        // set up a reference to and then disable the bar
        bar.SetActive(false);

        


        // start wave is called when the scene is loaded

        StartWave();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer!= -1f) timer -= Time.deltaTime;
        
        //check if the player has lost the game
        //we include a timer here to check for if the player loses at the exact same time as winning - win trumps a loss
        if (moneyManager.bankrupt && troopSpawner.transform.childCount == 1 && timer == -1f && coreShip != null && waveCompleteScreen.gameObject.activeSelf == false && bar.activeSelf == true)
        {
            timer = 2f;
            
        }
        if(timer <= 1f && timer != -1f)
        {
            if (moneyManager.bankrupt && troopSpawner.transform.childCount == 1 && coreShip != null && waveCompleteScreen.gameObject.activeSelf == false && bar.activeSelf == true)
            {
                bar.SetActive(false);
                lossScreen.gameObject.SetActive(true);
                lossText.text = "You reached wave " + waveCount;
                timer = -1f;
            }
            else
            {
                timer = -1f;
            }
        }
    }

    public void StartWave()
    {
        timer = -1f;
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

        // respawn the core ship
        GameObject cs = Instantiate(coreShip, coreShipPos, Quaternion.identity);
        // run onWaveComplete when the core ship is destroyed
        cs.GetComponent<TowerHealth>().onDestroy += () => OnWaveComplete();

        // load the wave data
        WaveData data = waveData[waveCount];
        towerGen.maxScrap = data.genNumber;
        towerGen.wallThreshold = data.wallThreshold;
        towerGen.mineThreshold = data.mineThreshold;
        towerGen.arrowThreshold = data.arrowThreshold;
        towerGen.laserThreshold = data.laserThreshold;
        towerGen.sniperThreshold = data.sniperThreshold;
        towerGen.rerollChance = data.rerollChance;
        moneyManager.AddMoney(data.startMoney);

        // show the start text screen
        startScreen.gameObject.SetActive(true);
        startText.text = data.levelStartText;

        

        // spawn in initial towers
        if (data.spawnInitial)
        {
            towerGen.Generate();
        }

        // increment waveCount

        waveCount++;
        text.text = "Wave " + waveCount + " Complete!";
        
        

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

    public void ResetGame()
    {
        waveCount = 0;
        Destroy(coreShip);
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        foreach(GameObject tower in towers)
        {
            Destroy(tower);
        }
        StartWave();
    }
}
