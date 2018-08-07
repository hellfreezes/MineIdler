using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade {
    int multiplier;
    float price;
    string description;
    ProductType productType;

    public Upgrade(int multiplier, string description, ProductType productType, float price)
    {
        this.price = price;
        this.multiplier = multiplier;
        this.description = description;
        this.productType = productType;
    }
}
