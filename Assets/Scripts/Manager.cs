using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public class Manager : IXmlSerializable {
    GameObject panel;
    Product product;
    ProductType productType;
    Money price;
   
    string productName;
    string managerName;
    string description;

    Sprite sprite;

    bool isActive = false;


    #region Properties
    public Product Product
    {
        get
        {
            return product;
        }

        set
        {
            product = value;
        }
    }

    public Money Price
    {
        get
        {
            return price;
        }

        set
        {
            price = value;
        }
    }

    public string ProductName
    {
        get
        {
            return productName;
        }

        set
        {
            productName = value;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }

        set
        {
            description = value;
        }
    }

    public Sprite Sprite
    {
        get
        {
            return sprite;
        }

        set
        {
            sprite = value;
        }
    }

    public string ManagerName
    {
        get
        {
            return managerName;
        }

        set
        {
            managerName = value;
        }
    }

    public ProductType ProductType
    {
        get
        {
            return productType;
        }

        set
        {
            productType = value;
        }
    }

    public GameObject Panel
    {
        get
        {
            return panel;
        }

        set
        {
            panel = value;
        }
    }
    #endregion

    public event EventHandler Hired;

    public void Update(float deltaTime)
    {
        if (isActive)
        {

        }
    }

    private void OnProductionComplete(object source, EventArgs e)
    {
        product.SellAll();
    }

    public Manager (ManagerPrototype prototype)
    {
        sprite = prototype.sprite;
        managerName = prototype.managerName;
        description = prototype.description;
        productName = prototype.productName;
        productType = prototype.productType;
        price = prototype.price;
    }

    public void Hire()
    {
        //Debug.Log(managerName + " нанят");
        product = ProductsController.Instance.GetProductFromList(productType);
        Funds.Instance.SpendMoney(price);
        isActive = true;
        product.ProductionComplete += OnProductionComplete;
        product.SellAll(); // запускаем первый раз на случай если есть что продать
        OnHired();
    }

    void OnHired()
    {
        if (Hired != null)
        {
            Hired(this, EventArgs.Empty);
        }
    }

    #region IXmlSerializable
    public XmlSchema GetSchema()
    {
        throw new NotImplementedException();
    }

    public void ReadXml(XmlReader reader)
    {
        isActive = bool.Parse(reader.GetAttribute("IsActive"));

        product = ProductsController.Instance.GetProductFromList(productType);
        product.ProductionComplete += OnProductionComplete;
        product.SellAll(); // запускаем первый раз на случай если есть что продать
        OnHired();
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("IsActive", isActive.ToString());
    }
    #endregion
}
