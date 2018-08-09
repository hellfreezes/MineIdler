using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductPanel : MonoBehaviour {
    Product product;

    RectTransform progressBarUI;
    Button buyButtonUI;
    Text numberOfBuildingsTextUI;
    Text progressTextUI;
    GameObject coinObject;

    // UI
    float progressBarWidth;


    public event EventHandler Sold;

    // Use this for initialization
    void OnEnable () {
        InitUIElements();
        progressBarWidth = progressBarUI.sizeDelta.x;

    }
	
	// Update is called once per frame
	void Update () {
        UpdateProgressBarUI();
        UpdateNumberOfBuildingsUI();
        CheckForAbilityToBuyBuilding();
        UpdateButtonUI();
    }

    void InitUIElements()
    {
        progressBarUI = transform.Find("Bar").transform.Find("Progress").GetComponent<RectTransform>();
        buyButtonUI = transform.Find("Button - Buy").GetComponent<Button>();
        numberOfBuildingsTextUI = transform.Find("Level").GetComponentInChildren<Text>();
        progressTextUI = transform.Find("Bar").transform.Find("Progress").GetComponentInChildren<Text>();
        coinObject = transform.Find("BackGround").transform.Find("Image").gameObject;
    }

    public void AssignProduct(Product p)
    {
        product = p;

        GameManager.Instance.MoneyAmountChanged += OnMoneyAmountChanged;
        p.BuildingPurchased += OnBuildingPurchased;
        p.ProgressChanged += OnProgressChanged;
        p.ProductionComplete += OnProductionComplete;
        p.ProductSold += OnProductSold;

        transform.Find("BackGround").transform.Find("Button").GetComponentInChildren<Image>().sprite = p.Sprite;
        UpdateProgressBarUI();
        UpdateButtonUI();
        UpdateProgressTextUI();
        UpdateNumberOfBuildingsUI();
        CheckForAbilityToBuyBuilding();
    }

    public void BuyNewBuilding()
    {
        product.BuyBuilding();
    }

    public void SellAll()
    {
        product.SellAll();
    }

    void UpdateProgressBarUI()
    {
        Vector2 size = progressBarUI.sizeDelta;
        size.x = Mathf.Clamp(product.GetWidthProgress(progressBarWidth), 0, progressBarWidth);
        progressBarUI.sizeDelta = size;
    }

    void UpdateButtonUI()
    {
        Text t = buyButtonUI.GetComponentInChildren<Text>();
        t.text = "Купить " + GameManager.Instance.BuyStep.ToString() + " за $" + (product.CurrentBuildingCost.ToString());// ("#.##"); Mult(GameManager.Instance.BuyStep))
    }

    void UpdateProgressTextUI()
    {
        progressTextUI.text = product.GetProductCost().ToString();// ("#.##");
    }

    void UpdateNumberOfBuildingsUI()
    {
        numberOfBuildingsTextUI.text =product.NumberOfBuildings.ToString();
    }

    protected virtual void OnSold()
    {
        if (Sold != null)
        {
            Sold(this, EventArgs.Empty);
        }
    }

    void OnMoneyAmountChanged(object source, EventArgs e)
    {
        CheckForAbilityToBuyBuilding();
    }

    void CheckForAbilityToBuyBuilding()
    {
        if (GameManager.Instance.EnoughMoney(product.CurrentBuildingCost))
        {
            buyButtonUI.interactable = true;
        }
        else
        {
            buyButtonUI.interactable = false;
        }
    }

    void OnBuildingPurchased(object source, EventArgs e)
    {
        UpdateNumberOfBuildingsUI();
        UpdateButtonUI();
        UpdateProgressTextUI();
    }

    void OnProgressChanged(object source, EventArgs e)
    {
        UpdateProgressBarUI();
    }

    void OnProductionComplete(object source, EventArgs e)
    {
        coinObject.SetActive(true);
    }

    void OnProductSold(object source, EventArgs e)
    {
        coinObject.SetActive(false);
    }
}
