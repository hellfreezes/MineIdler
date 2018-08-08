using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField]
    int startMoney = 1000;

    int buyStep = 1;

    float money = 0;

    Dictionary<string, Product> products;

    static GameManager instance;

    public event EventHandler MoneyAmountChanged;

    public static GameManager Instance
    {
        get { return instance;  }
    }

    public float Money
    {
        get
        {
            return money;
        }
    }

    public int BuyStep
    {
        get
        {
            return buyStep;
        }

        set
        {
            buyStep = value;
        }
    }

    // Use this for initialization
    void OnEnable () {
        if (instance != null)
        {
            Debug.LogError("Менеджер игры не один на сцене!");
            Destroy(gameObject);
        }
        instance = this;

        products = new Dictionary<string, Product>();
    }

    private void Start()
    {
        ProductsController.Instance.Init();
        ManagersController.Instance.Init();
        AddMoneyAmount(startMoney);

        Money mo = new Money(1.00010f, 1);
        Money mo2 = new Money(1f, 1);
        mo2.Div(2);
        Debug.Log(mo2.GetValue());
    }

    // Update is called once per frame
    void Update () {
		
	}

    public bool EnoughMoney(float amount)
    {
        if (money >= amount)
        {
            return true;
        }

        return false;
    }

    public void RegisterProduct(Product newProduct)
    {
        if (newProduct != null)
        {
            //Debug.Log("Добавлен продукт: " + newProduct.ProductName);
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

    void AddMoneyAmount(float amount)
    {
        money += amount;
        OnMoneyAmountChanged();
    }

    public void AddMoney(float amount)
    {
        AddMoneyAmount(amount);
    }

    public void SpendMoney(float amount)
    {
        if (money >= amount)
        {
            money -= amount;
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
        } else
        {
            Debug.LogError("products не содержит ключа '" + name + "'");
            return null;
        }
    }
}
