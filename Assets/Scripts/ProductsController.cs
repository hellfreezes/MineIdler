using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//public class ProductArgs : EventArgs
//{
//    public Product Product { get; set; }
//}

public class ProductsController : MonoBehaviour {
    [SerializeField]
    GameObject productPanelPrefab;
    [SerializeField]
    Transform gameField;
    [SerializeField]
    Sprite[] sprites;

    List<ProductPrototype> productsPrototypes;
    Dictionary<ProductType, Product> products;

    static ProductsController instance;
    public static ProductsController Instance
    {
        get
        {
            return instance;
        }
    }

    public event EventHandler ProductCreated;
    
    // Use this for initialization
    void OnEnable () {
        if (instance != null)
        {
            Debug.LogError("Обнаружено два контроллера продуктов на сцене");
        }
        else
        {
            instance = this;
        }

        // Инициализация перенесена из этого класса в GameManager
    }

    private void Update()
    {
        foreach (ProductType p in products.Keys)
        {
            products[p].Update(Time.deltaTime);
        }
    }

    public void Init()
    {
        CreatePrototypes();
        CreateProductsAndUI();
    }

    void CreatePrototypes()
    {
        productsPrototypes = new List<ProductPrototype>();

        ProductPrototype p = new ProductPrototype();
        p.name = "Pie";
        p.productType = ProductType.PIE;
        p.sprite = sprites[0];
        p.initialTime = 0.6f;
        p.productCost = new Money(0.001f, 0);//  1;
        p.baseCost = new Money(0.004f, 0);//4;
        p.coefficient = 1.07f;
        p.initialProductivity = 1.67f;
        productsPrototypes.Add(p);

        p = new ProductPrototype();
        p.name = "Burger";
        p.productType = ProductType.BURGER;
        p.sprite = sprites[1];
        p.initialTime = 3f;
        p.productCost = new Money(0.06f, 0);//60;
        p.baseCost = new Money(0.06f, 0);//60;
        p.coefficient = 1.15f;
        p.initialProductivity = 20f;
        productsPrototypes.Add(p);

        p = new ProductPrototype();
        p.name = "Restorant";
        p.productType = ProductType.RESTORANT;
        p.sprite = sprites[2];
        p.initialTime = 6f;
        p.productCost = new Money(0.54f, 0);//540;
        p.baseCost = new Money(0.72f, 0);//720;
        p.coefficient = 1.14f;
        p.initialProductivity = 20f;
        productsPrototypes.Add(p);

        p = new ProductPrototype();
        p.name = "Butcher";
        p.productType = ProductType.BUTCHER;
        p.sprite = sprites[3];
        p.initialTime = 12f;
        p.productCost = new Money(4.32f, 0);//4320;
        p.baseCost = new Money(8.64f, 0);// 8640;
        p.coefficient = 1.13f;
        p.initialProductivity = 360f;
        productsPrototypes.Add(p);
    }

    void CreateProductsAndUI()
    {
        products = new Dictionary<ProductType, Product>();

        foreach (ProductPrototype proto in productsPrototypes)
        {
            GameObject go = Instantiate(productPanelPrefab);
            ProductPanel panel = go.GetComponent<ProductPanel>();
            Product p = new Product(proto);

            //FIXME: не тут надо подписываться
            //p.ProductSold += Funds.Instance.OnProductSold;
            //p.ProductSold += SoundController.Instance.OnProductSold;
            //p.BuildingPurchased += SoundController.Instance.OnBuy;

            go.transform.SetParent(gameField);
            go.name = "Product - " + p.ProductName;

            panel.AssignProduct(p);
            products.Add(proto.productType, p);
            OnProductCreated(p);
        }
    }

    void OnProductCreated(Product product)
    {
        if (ProductCreated != null)
        {
            ProductCreated(product, EventArgs.Empty); //new ProductArgs() { Product = product }
        }
    }

    public Sprite GetProductSprite(ProductType t)
    {
        return sprites[(int)t];
    }

    public Product GetProductFromList(ProductType type)
    {
        if (products.ContainsKey(type))
        {
            return products[type];
        }
        else
        {
            Debug.LogError("products не содержит ключа '" + type + "'");
            return null;
        }
    }

    public Product[] GetProducts()
    {
        return products.Values.ToArray();
    }
}
