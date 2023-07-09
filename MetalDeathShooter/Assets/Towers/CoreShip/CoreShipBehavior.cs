using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreShipBehavior : MonoBehaviour
{
     
    void Start()
    {
        // link the on destroy event to the one emitted by this GO's towerhealth script
        GetComponent<TowerHealth>().onDestroy += () => OnTowerDestroy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // will run when this tower is destroyed
    private void OnTowerDestroy()
    {
        //the wave is now completed
    }
}
