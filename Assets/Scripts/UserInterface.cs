using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {
    [SerializeField]
    Text moneyAmountText;

	// Use this for initialization
	void Start () {
        GameManager.Instance.MoneyAmountChanged += OnMoneyAmountChanged;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMoneyAmountChanged(object source, EventArgs e)
    {
        GameManager gameManager = (GameManager)source;
        string money = gameManager.Money.ToString("#.##");
        if (gameManager.Money < 1)
        {
            money = "0" + money;
        } 
        moneyAmountText.text = money;
    }
}
