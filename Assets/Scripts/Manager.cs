using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager {
    Product product;
    float price;
   
    string productName;
    string name;
    string description;

    public Manager(string name, string description, string productName, float price)
    {
        this.name = name;
        this.description = description;
        this.productName = productName;
        this.price = price;
    }

    public void Update(float deltaTime)
    {

    }

    void OnProductionComplete(object source, EventArgs e)
    {
        product.SellAll();
    }
}
