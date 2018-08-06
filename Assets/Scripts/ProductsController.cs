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

    List<ProductPrototype> products;

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

    public void Init()
    {
        CreatePrototypes();
        CreatePanels();
    }

    void CreatePrototypes()
    {
        products = new List<ProductPrototype>();

        ProductPrototype p = new ProductPrototype();
        p.name = "Pie";
        p.productType = ProductType.PIE;
        p.sprite = 0;
        p.initialTime = 0.6f;
        p.productCost = 1;
        p.baseCost = 4;
        p.coefficient = 1.07f;
        p.initialProductivity = 1.67f;
        products.Add(p);

        p = new ProductPrototype();
        p.name = "Burger";
        p.productType = ProductType.BURGER;
        p.sprite = 1;
        p.initialTime = 3f;
        p.productCost = 60;
        p.baseCost = 60;
        p.coefficient = 1.15f;
        p.initialProductivity = 20f;
        products.Add(p);

        p = new ProductPrototype();
        p.name = "Restorant";
        p.productType = ProductType.RESTORANT;
        p.sprite = 2;
        p.initialTime = 6f;
        p.productCost = 540;
        p.baseCost = 720;
        p.coefficient = 1.14f;
        p.initialProductivity = 20f;
        products.Add(p);

        p = new ProductPrototype();
        p.name = "Butcher";
        p.productType = ProductType.BUTCHER;
        p.sprite = 3;
        p.initialTime = 12f;
        p.productCost = 4320;
        p.baseCost = 8640;
        p.coefficient = 1.13f;
        p.initialProductivity = 360f;
        products.Add(p);
    }

    void CreatePanels()
    {
        foreach (ProductPrototype proto in products)
        {
            GameObject go = Instantiate(productPanelPrefab);
            go.transform.SetParent(gameField);
            go.name = "Product - " + proto.name;

            Product p = go.GetComponent<Product>();
            p.ProductName = proto.name;
            p.InitialProductivity = proto.initialProductivity;
            p.InitialTime = proto.initialTime;
            p.ProductCost = proto.productCost;
            p.BaseCost = proto.baseCost;
            p.Coefficient = proto.coefficient;
            p.InitialProductivity = proto.initialProductivity;
            p.Sprite = sprites[proto.sprite];

            p.gameObject.transform.Find("BackGround").transform.Find("Button").GetComponentInChildren<Image>().sprite = p.Sprite;
        }
    }

    public Sprite GetProsuctSprite(ProductType t)
    {
        return sprites[(int)t];
    }
}
