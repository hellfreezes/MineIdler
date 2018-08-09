using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Game {
    static Game instance;
    public static Game Instance
    {
        get { return instance; }
    }

    int buyStep = 1;

    Money money;

    Dictionary<string, Product> products;

    public event EventHandler MoneyAmountChanged;

    public Game()
    {
        if (instance != null)
        {
            Debug.LogError("Попытка создать еще второй экземпляр Game на сцене");
            instance = null;
        }
        instance = this;

        money = new Money(0, 0);
        products = new Dictionary<string, Product>();
    }

    public bool EnoughMoney(Money amount)
    {
        return (money.IsGreaterThen(amount) || money.IsEqual(amount));
    }

    public void RegisterProduct(Product newProduct)
    {
        if (newProduct != null)
        {
            products.Add(newProduct.ProductName, newProduct);
            newProduct.ProductSold += OnProductSold;
            newProduct.ProductSold += SoundController.Instance.OnProductSold;
            newProduct.BuildingPurchased += SoundController.Instance.OnBuy;
        }
    }

    void OnProductSold(object source, EventArgs e)
    {
        Product product = (Product)source;
        AddMoneyAmount(product.GetAllMoney());

        OnMoneyAmountChanged();
    }

    void AddMoneyAmount(Money amount)
    {
        money.Add(amount);
        OnMoneyAmountChanged();
    }

    void AddMoneyAmount(float amount)
    {
        money.Add(amount);
        OnMoneyAmountChanged();
    }

    public void AddMoney(Money amount)
    {
        AddMoneyAmount(amount);
    }

    public void SpendMoney(Money amount)
    {
        if (money.IsGreaterThen(amount) || money.IsEqual(amount))
        {
            money.Subt(amount);
            OnMoneyAmountChanged();
        }
    }

    protected virtual void OnMoneyAmountChanged()
    {
        if (MoneyAmountChanged != null)
        {
            MoneyAmountChanged(this, EventArgs.Empty);
        }
    }

    public Product GetProductFromList(string name)
    {
        if (products.ContainsKey(name))
        {
            return products[name];
        }
        else
        {
            Debug.LogError("products не содержит ключа '" + name + "'");
            return null;
        }
    }
}
