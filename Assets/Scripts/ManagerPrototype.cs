using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerPrototype {

    public Money price;
    public Sprite sprite;

    public string productName;
    public string managerName;
    public string description;

    public ProductType productType;

    public ManagerPrototype(string managerName, string description, string productName, Money price, Sprite sprite, ProductType productType)
    {
        this.managerName = managerName;
        this.description = description;
        this.productName = productName;
        this.price = price;
        this.sprite = sprite;
        this.productType = productType;
    }
}
