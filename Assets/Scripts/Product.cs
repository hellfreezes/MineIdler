using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProductType
{
    PIE = 0,
    BURGER = 1,
    RESTORANT = 2,
    BUTCHER = 3
};

public class Product {

    ProductType productType;
    Sprite sprite;
    string productName;

    // Математические показатели
    float initialTime;

    Money productCost;
    Money baseCost;

    float coefficient;
    float initialProductivity;


    float progress;
    Money currentBuildingCost;
    int numberOfBuildings = 0;

    float productPriceMultiplier = 1;
    float buildingPriceMultiplier = 1;
    float timeMultiplier = 1;

    float moneyOnHand = 0; 

    bool inProgress = true;
    bool productionComplete = false;


    #region Properties
    public float InitialTime
    {
        get
        {
            return initialTime;
        }

        protected set
        {
            initialTime = value;
        }
    }
    public Money ProductCost
    {
        get
        {
            return productCost;
        }

        protected set
        {
            productCost = value;
        }
    }
    public Money BaseCost
    {
        get
        {
            return baseCost;
        }

        protected set
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

        protected set
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

        protected set
        {
            initialProductivity = value;
        }
    }

    public string ProductName
    {
        get
        {
            return productName;
        }

        protected set
        {
            productName = value;
        }
    }
    public Sprite Sprite
    {
        get
        {
            return sprite;
        }

        protected set
        {
            sprite = value;
        }
    }
    public ProductType ProductType
    {
        get
        {
            return productType;
        }

        protected set
        {
            productType = value;
        }
    }

    public float Progress
    {
        get
        {
            return progress;
        }

        protected set
        {
            progress = value;
            OnProgressChanged();
        }
    }
    public Money CurrentBuildingCost
    {
        get
        {
            return currentBuildingCost;
        }
    }
    public int NumberOfBuildings
    {
        get
        {
            return numberOfBuildings;
        }
    }

    #endregion

    public event EventHandler ProductionComplete;
    public event EventHandler ProductSold;
    public event EventHandler BuildingPurchased;
    public event EventHandler ProgressChanged;

    // Use this for initialization
    public Product (ProductPrototype proto) {
        productType = proto.productType;
        Sprite = proto.sprite;
        ProductName = proto.name;
        InitialTime = proto.initialTime;
        ProductCost = proto.productCost;
        BaseCost = proto.baseCost;
        Coefficient = proto.coefficient;
        InitialProductivity = proto.initialProductivity;

        Progress = initialTime;
        currentBuildingCost = baseCost;
        GameManager.Instance.RegisterProduct(this);
        GameManager.Instance.MoneyAmountChanged += OnMoneyAmountChanged;
    }
	
	// Update is called once per frame
	public void Update (float deltaTime) {
        UpdateProgress(deltaTime);

        if (inProgress && Progress <= 0)
        {
            ProductionCompleted();
        }
	}

    void UpdateProgress(float deltaTime)
    {
        if (inProgress && numberOfBuildings > 0)
        {
            Progress -= deltaTime;
        }
    }

    void ProductionCompleted()
    {
        inProgress = false;
        productionComplete = true;
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
            OnProductSold();
            StartProduction();
        }
    }

    public float GetProductCost()
    {
        return productCost * numberOfBuildings * productPriceMultiplier;
    }

    void AddMoneyToCashBox()
    {
        moneyOnHand += GetProductCost();
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
        Progress = initialTime * timeMultiplier;
    }

    void OnMoneyAmountChanged(object source, EventArgs e)
    {
        
    }

    protected virtual void OnProgressChanged()
    {
        if (ProgressChanged != null)
        {
            ProgressChanged(this, EventArgs.Empty);
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

    void AddBuilding(int amount)
    {
        numberOfBuildings += amount;
        currentBuildingCost = GetBuildingCost();
    }

    protected virtual void OnBuildingPurchased()
    {
        if (BuildingPurchased != null)
        {
            BuildingPurchased(this, EventArgs.Empty);
        }
    }

    public Money GetBuildingCost()
    {
        return baseCost * Mathf.Pow(coefficient, numberOfBuildings) * buildingPriceMultiplier;
    }

    public float GetWidthProgress(float width)
    {
        return width - (width * Progress / InitialTime);
    }
}
