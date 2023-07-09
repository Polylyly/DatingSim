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


    private TextMeshProUGUI text;

    

    void Start()
    {
        text = transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "$" + money;
        foreach(Btn btn in buttons)
        {
            if (money < btn.cost) btn.reference.GetComponent<Button>().interactable = false; else btn.reference.GetComponent<Button>().interactable = true;
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
