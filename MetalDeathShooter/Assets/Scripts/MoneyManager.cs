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
