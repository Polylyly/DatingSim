using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroopSpawnManager : MonoBehaviour
{
    //list of the possible troops to spawn
    public GameObject[] troops = new GameObject[1];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnTroop(int index)
    {
        if(index == 3)
        {
            Instantiate(troops[index], this.transform.GetChild(0));
            return;
        }
        Instantiate(troops[index], this.transform);
    }
}
