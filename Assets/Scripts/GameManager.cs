using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField]
    int startMoney = 1000;

    int buyStep = 1;

    Money money;

    Dictionary<string, Product> products;

    static GameManager instance;

    public event EventHandler MoneyAmountChanged;

    public static GameManager Instance
    {
        get { return instance;  }
    }

    public Money Money
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
        
        money = new Money(startMoney, 0);
        AddMoneyAmount(startMoney);

        //Money mo = new Money(9.125f, 1);
        //Money mo2 = new Money(9.25f, 1);
        //float res = mo.Div(mo2);
        //Debug.Log(mo.IsGreaterThen(mo2));
    }

    // Update is called once per frame
    void Update () {
		
	}

    public bool EnoughMoney(Money amount)
    {
        return (money.IsGreaterThen(amount) || money.IsEqual(amount));
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

    void AddMoneyAmount(Money amount)
    {
        money.Mult(amount);
        OnMoneyAmountChanged();
    }

    void AddMoneyAmount(float amount)
    {
        money.Mult(amount);
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
        } else
        {
            Debug.LogError("products не содержит ключа '" + name + "'");
            return null;
        }
    }
}
