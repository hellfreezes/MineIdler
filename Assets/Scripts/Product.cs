using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public enum ProductType
{
    PIE = 0,
    BURGER = 1,
    RESTORANT = 2,
    BUTCHER = 3
};

public class Product : IXmlSerializable {

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
        moneyOnHand.Add(GetProductCost());
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
        if (Funds.Instance.EnoughMoney(currentBuildingCost))
        {
            Funds.Instance.SpendMoney(currentBuildingCost);
            AddBuilding(Funds.Instance.BuyStep);
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

    public void Reset()
    {
        Progress = initialTime;
        productPriceMultiplier = 1;
        buildingPriceMultiplier = 1;
        timeMultiplier = 1;
        numberOfBuildings = 0;
        inProgress = true;
        productionComplete = false;
        currentBuildingCost = BaseCost;
        currentProductCost = ProductCost;

        OnBuildingPurchased();
        OnProductSold();
    }

    #region IXmlSerializable
    public XmlSchema GetSchema()
    {
        throw new NotImplementedException();
    }

    public void ReadXml(XmlReader reader)
    {
        Progress = float.Parse(reader.GetAttribute("Progress"));
        numberOfBuildings = int.Parse(reader.GetAttribute("NumberOfBuildings"));
        productPriceMultiplier = float.Parse(reader.GetAttribute("ProductPriceMultiplier"));
        buildingPriceMultiplier = float.Parse(reader.GetAttribute("BuildingPriceMultiplier"));
        timeMultiplier = float.Parse(reader.GetAttribute("TimeMultiplier"));
        inProgress = bool.Parse(reader.GetAttribute("InProgress"));
        productionComplete = bool.Parse(reader.GetAttribute("ProductionComplete"));

        if (productionComplete)
            OnProductionComplete();

        XmlReader localReader = XmlReader.Create(new StringReader(reader.ReadInnerXml()), reader.Settings);

        localReader.ReadToDescendant("MoneyOnHand");
        moneyOnHand.ReadXml(localReader);
        localReader.Close();

        currentBuildingCost = GetBuildingCost();
        currentProductCost = CalculateProductCost();
        OnBuildingPurchased();
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("Progress", progress.ToString());
        writer.WriteAttributeString("NumberOfBuildings", numberOfBuildings.ToString());
        writer.WriteAttributeString("ProductPriceMultiplier", productPriceMultiplier.ToString());
        writer.WriteAttributeString("BuildingPriceMultiplier", buildingPriceMultiplier.ToString());
        writer.WriteAttributeString("TimeMultiplier", timeMultiplier.ToString());
        writer.WriteAttributeString("InProgress", inProgress.ToString());
        writer.WriteAttributeString("ProductionComplete", productionComplete.ToString());

        writer.WriteStartElement("MoneyOnHand");
        moneyOnHand.WriteXml(writer);
        writer.WriteEndElement();
    }
    #endregion
}
