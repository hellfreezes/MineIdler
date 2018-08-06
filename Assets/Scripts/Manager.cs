using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager {
    Product product;
    float price;
    int spriteID;
   
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

    public float Price
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

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
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

    public int SpriteID
    {
        get
        {
            return spriteID;
        }

        set
        {
            spriteID = value;
        }
    }

    public void Update(float deltaTime)
    {

    }

    void OnProductionComplete(object source, EventArgs e)
    {
        product.SellAll();
    }
}
