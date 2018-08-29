using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Upgrade {
    GameObject panel;

    int multiplier;
    Money price;
    
    string description;
    ProductType productType;

    bool active;

    public GameObject Panel
    {
        //16 80 195
        //кордиант 205 75 16
        get; set;

    }

    public Money Price
    {
        get { return price; }
    }

    public string Description
    {
        get
        {
            return description;
        }
    }

    public ProductType ProductType
    {
        get
        {
            return productType;
        }
    }

    public Upgrade(int multiplier, string description, ProductType productType, Money price)
    {
        this.price = price;
        this.multiplier = multiplier;
        this.description = description;
        this.productType = productType;

        this.active = false;
    }

    public void Apply()
    {

    }
}
