using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField]
    int startMoney = 1000;

    int buyStep = 1;

    float money = 0;

    List<Product> products;

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

        products = new List<Product>();
    }

    private void Start()
    {
        AddMoneyAmount(startMoney);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void RegisterProduct(Product newProduct)
    {
        if (newProduct != null)
        {
            products.Add(newProduct);
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
}
