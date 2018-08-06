using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Product : MonoBehaviour {
    RectTransform progressBarUI;
    Button buyButtonUI;
    Text numberOfBuildingsTextUI;
    Text progressTextUI;
    GameObject coinObject;



    [SerializeField]
    float initialTime;
    [SerializeField]
    float productCost;
    [SerializeField]
    float baseCost;
    [SerializeField]
    float coefficient;
    [SerializeField]
    float initialProductivity;


    float progress;
    float currentBuildingCost;

    int numberOfBuildings = 0;
    float multiplier = 1;

    // Выручка
    float moneyOnHand = 0; 

    bool inProgress = true;
    bool productionComplete = false;

    // UI
    float progressBarWidth;

    public float InitialTime
    {
        get
        {
            return initialTime;
        }

        set
        {
            initialTime = value;
        }
    }

    public float ProductCost
    {
        get
        {
            return productCost;
        }

        set
        {
            productCost = value;
        }
    }

    public float BaseCost
    {
        get
        {
            return baseCost;
        }

        set
        {
            baseCost = value;
        }
    }

    public float Coefficient
    {
        get
        {
            return coefficient;
        }

        set
        {
            coefficient = value;
        }
    }

    public float InitialProductivity
    {
        get
        {
            return initialProductivity;
        }

        set
        {
            initialProductivity = value;
        }
    }

    public event EventHandler ProductionComplete;
    public event EventHandler ProductSold;
    public event EventHandler BuildingPurchased;

    // Use this for initialization
    void Start () {
        InitUIElements();

        progressBarWidth = progressBarUI.sizeDelta.x;
        progress = initialTime;
        currentBuildingCost = baseCost;
        GameManager.Instance.RegisterProduct(this);
        GameManager.Instance.MoneyAmountChanged += OnMoneyAmountChanged;
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
	
	// Update is called once per frame
	void Update () {
        UpdateProgress();

        if (inProgress && progress <= 0)
        {
            ProductionCompleted();
        }
	}

    void UpdateProgress()
    {
        if (inProgress && numberOfBuildings > 0)
        {
            progress -= Time.deltaTime;
            UpdateProgressBarUI();
        }
    }

    void UpdateProgressBarUI()
    {
        Vector2 size = progressBarUI.sizeDelta;
        size.x = Mathf.Clamp(progressBarWidth - (progressBarWidth * progress / initialTime), 0, progressBarWidth);
        progressBarUI.sizeDelta = size;
    }

    void ProductionCompleted()
    {
        inProgress = false;
        productionComplete = true;
        coinObject.SetActive(true);
        OnProductionComplete();
    }

    protected virtual void OnProductionComplete()
    {
        if (ProductionComplete != null)
        {
            ProductionComplete(this, EventArgs.Empty);
        }
    }

    public void SellAll()
    {
        Sell();
    }

    void Sell()
    {
        if (productionComplete)
        {
            AddMoneyToCashBox();
            coinObject.SetActive(false);
            OnProductSold();
            StartProduction();
        }
    }

    float CalculateReceipt()
    {
        return productCost * numberOfBuildings * multiplier;
    }

    void AddMoneyToCashBox()
    {
        moneyOnHand += CalculateReceipt();
    }

    protected virtual void OnProductSold()
    {
        if (ProductSold != null)
        {
            ProductSold(this, EventArgs.Empty);
        }
    }

    void StartProduction()
    {
        productionComplete = false;
        inProgress = true;
        progress = initialTime;
    }

    void OnMoneyAmountChanged(object source, EventArgs e)
    {
        CheckForAbilityToBuyBuilding();
    }

    void CheckForAbilityToBuyBuilding()
    {
        if (GameManager.Instance.EnoughMoney(currentBuildingCost))
        {
            buyButtonUI.interactable = true;
        } else
        {
            buyButtonUI.interactable = false;
        }
    }

    public float GetAllMoney()
    {
        float toSend = moneyOnHand;
        moneyOnHand = 0;
        return toSend;
    }

    public void BuyBuilding()
    {
        if (GameManager.Instance.EnoughMoney(currentBuildingCost))
        {
            GameManager.Instance.SpendMoney(currentBuildingCost);
            AddBuilding(GameManager.Instance.BuyStep);
            OnBuildingPurchased();
        }
    }

    void OnBuildingPurchased()
    {
        if (BuildingPurchased != null)
        {
            BuildingPurchased(this, EventArgs.Empty);
        }
    }

    void AddBuilding(int amount)
    {
        numberOfBuildings += amount;
        currentBuildingCost = CalculateCost();
        UpdateNumberOfBuildingsUI();
        UpdateButtonUI();
        UpdateProgressTextUI();
    }

    void UpdateButtonUI()
    {
        Text t = buyButtonUI.GetComponentInChildren<Text>();
        t.text = "Купить " + GameManager.Instance.BuyStep.ToString() + " за $" + (currentBuildingCost * GameManager.Instance.BuyStep).ToString("#.##");
    }

    void UpdateProgressTextUI()
    {
        progressTextUI.text = CalculateReceipt().ToString("#.##");
    }

    void UpdateNumberOfBuildingsUI()
    {
        numberOfBuildingsTextUI.text = numberOfBuildings.ToString();
    }

    float CalculateCost()
    {
        return baseCost * Mathf.Pow(coefficient, numberOfBuildings);
    }
}
