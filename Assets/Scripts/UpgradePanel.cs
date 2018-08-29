using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour {
    Upgrade upgrade;


    //UI
    Text upgradeNameUI;
    Text descriptionUI;
    Text priceValueUI;
    Button buttomApplyUI;
    Image productImageUI;

    private void OnEnable()
    {
        InitUIElements();
        Funds.Instance.MoneyAmountChanged += OnMoneyAmountChanged;
    }

    private void InitUIElements()
    {
        upgradeNameUI = transform.Find("Name").GetComponent<Text>();
        descriptionUI = transform.Find("Description").GetComponent<Text>();
        priceValueUI = transform.Find("PriceValue").GetComponent<Text>();
        buttomApplyUI = transform.Find("Button - Buy").GetComponent<Button>();
        productImageUI = transform.Find("ProductSprite").GetComponent<Image>();

        buttomApplyUI.onClick.AddListener(delegate { upgrade.Apply(); });
    }

    public void Assign(Upgrade u)
    {
        upgrade = u;
        //u.Hired += OnHired;
        //upgradeNameUI.text = u.Name;
        descriptionUI.text = u.Description;
        priceValueUI.text = u.Price.ToString();

        productImageUI.sprite = ProductsController.Instance.GetProductSprite(u.ProductType);
    }


    private void OnMoneyAmountChanged(object source, EventArgs e)
    {
        CheckApplyAbility();
    }

    private void CheckApplyAbility()
    {
        if (Funds.Instance.EnoughMoney(upgrade.Price))
        {
            buttomApplyUI.interactable = true;
        }
        else
        {
            buttomApplyUI.interactable = false;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
