using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    [System.Serializable]
    public struct Btn
    {
        public GameObject reference;
        public int cost;
    }

    public int money;
    public Btn[] buttons;
    public bool bankrupt = false;
    public GameManager gm;

    private TextMeshProUGUI text;

    

    void Start()
    {
        text = transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "$" + money;
        bankrupt = true;
        // will remain true only if the player cannot buy any more units
        foreach(Btn btn in buttons)
        {
            if (money < btn.cost)
            {
                btn.reference.GetComponent<Button>().interactable = false;
            }
            else
            {
                btn.reference.GetComponent<Button>().interactable = true;
                bankrupt = false;
            }
        }
        if (gm.waveData[gm.waveCount].allowBombers == false) buttons[1].reference.GetComponent<Button>().interactable = false;
        if (gm.waveData[gm.waveCount].allowDestroyers == false) buttons[2].reference.GetComponent<Button>().interactable = false;
        if (gm.waveData[gm.waveCount].allowSubs == false) buttons[3].reference.GetComponent<Button>().interactable = false;
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public void SpendMoney(int amount)
    {
        money -= amount;
    }
}
