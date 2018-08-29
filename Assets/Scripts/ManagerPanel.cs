using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerPanel : MonoBehaviour {

    Manager manager;

    //UI
    Image portraitUI;
    Text managerNameUI;
    Text descriptionUI;
    Text priceValueUI;
    Button buttomHireUI;
    Image productImageUI;

    private void OnEnable()
    {
        InitUIElements();
        Funds.Instance.MoneyAmountChanged += OnMoneyAmountChanged;
    }

    private void InitUIElements()
    {
        portraitUI = transform.Find("Portrait").GetComponent<Image>();
        managerNameUI = transform.Find("Name").GetComponent<Text>();
        descriptionUI = transform.Find("Description").GetComponent<Text>();
        priceValueUI = transform.Find("PriceValue").GetComponent<Text>();
        buttomHireUI = transform.Find("Button - Buy").GetComponent<Button>();
        productImageUI = transform.Find("ProductSprite").GetComponent<Image>();

        buttomHireUI.onClick.AddListener(delegate { manager.Hire(); });
    }

    private void OnMoneyAmountChanged(object source, EventArgs e)
    {
        CheckHireAbility();
    }

    public void Assign(Manager m)
    {
        manager = m;
        m.Hired += OnHired;

        portraitUI.sprite = m.Sprite;
        managerNameUI.text = m.ManagerName;
        descriptionUI.text = m.Description;
        priceValueUI.text = m.Price.ToString();

        productImageUI.sprite = ProductsController.Instance.GetProductSprite(m.ProductType);
    }

    private void CheckHireAbility()
    {
        if (Funds.Instance.EnoughMoney(manager.Price))
        {
            buttomHireUI.interactable = true;
        }
        else
        {
            buttomHireUI.interactable = false;
        }
    }

    void OnHired(object source, EventArgs e)
    {
        buttomHireUI.interactable = false;
        gameObject.SetActive(false);
    }
}
