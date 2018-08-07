using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductsController : MonoBehaviour {
    [SerializeField]
    GameObject productPanelPrefab;
    [SerializeField]
    Transform gameField;
    [SerializeField]
    Sprite[] sprites;

    List<ProductPrototype> productsPrototypes;
    List<Product> products;

    static ProductsController instance;
    public static ProductsController Instance
    {
        get
        {
            return instance;
        }
    }

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
        foreach (Product p in products)
        {
            p.Update(Time.deltaTime);
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
        p.productCost = 1;
        p.baseCost = 4;
        p.coefficient = 1.07f;
        p.initialProductivity = 1.67f;
        productsPrototypes.Add(p);

        p = new ProductPrototype();
        p.name = "Burger";
        p.productType = ProductType.BURGER;
        p.sprite = sprites[1];
        p.initialTime = 3f;
        p.productCost = 60;
        p.baseCost = 60;
        p.coefficient = 1.15f;
        p.initialProductivity = 20f;
        productsPrototypes.Add(p);

        p = new ProductPrototype();
        p.name = "Restorant";
        p.productType = ProductType.RESTORANT;
        p.sprite = sprites[2];
        p.initialTime = 6f;
        p.productCost = 540;
        p.baseCost = 720;
        p.coefficient = 1.14f;
        p.initialProductivity = 20f;
        productsPrototypes.Add(p);

        p = new ProductPrototype();
        p.name = "Butcher";
        p.productType = ProductType.BUTCHER;
        p.sprite = sprites[3];
        p.initialTime = 12f;
        p.productCost = 4320;
        p.baseCost = 8640;
        p.coefficient = 1.13f;
        p.initialProductivity = 360f;
        productsPrototypes.Add(p);
    }

    void CreateProductsAndUI()
    {
        products = new List<Product>();

        foreach (ProductPrototype proto in productsPrototypes)
        {
            GameObject go = Instantiate(productPanelPrefab);
            ProductPanel panel = go.GetComponent<ProductPanel>();
            Product p = new Product(proto);

            go.transform.SetParent(gameField);
            go.name = "Product - " + p.ProductName;

            panel.AssignProduct(p);
            products.Add(p);
        }
    }

    public Sprite GetProductSprite(ProductType t)
    {
        return sprites[(int)t];
    }
}
