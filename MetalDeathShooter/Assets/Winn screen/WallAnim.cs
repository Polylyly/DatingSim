using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAnim : MonoBehaviour
{
    
    public GameObject thing;

    public void Start()
    {
       
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(1);
        thing.SetActive(false);
    }
}
