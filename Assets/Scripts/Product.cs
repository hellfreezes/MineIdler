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

    // --- xml
    float progress;
    Money currentBuildingCost;
    Money currentProductCost;
    int numberOfBuildings = 0;

    float productPriceMultiplier = 1;
    float buildingPriceMultiplier = 1;
    float timeMultiplier = 1;

    Money moneyOnHand = new Money(0,0); 

    bool inProgress = true;
    bool productionComplete = false;

    // --- xml

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
        currentBuildingCost = (new Money(0,0)).Add(baseCost);
        currentProductCost = (new Money(0, 0)).Add(productCost);
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

    public Money GetProductCost()
    {
        return currentProductCost;
    }

    public Money CalculateProductCost()
    {
        //return productCost * numberOfBuildings * productPriceMultiplier;
        Money m = productCost.Clone(); // чтобы не изменять базовое значение
        return m.Mult(numberOfBuildings).Mult(productPriceMultiplier);
    }

    void AddMoneyToCashBox()
    {
        moneyOnHand = GetProductCost().Add(moneyOnHand);
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

    public Money GetAllMoney()
    {
        Money toSend = moneyOnHand;
        moneyOnHand = new Money(0,0);
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
        currentProductCost = CalculateProductCost();
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
        Money m = baseCost.Clone(); // чтобы не изменять базовое значение
        return m.Mult(Mathf.Pow(coefficient, numberOfBuildings)).Mult(buildingPriceMultiplier);
    }

    public float GetWidthProgress(float width)
    {
        return width - (width * Progress / InitialTime);
    }
}
