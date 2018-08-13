using System.Collections;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class Funds : IXmlSerializable
{
    static Funds instance;
    public static Funds Instance
    {
        get { return instance; }
    }

    int buyStep = 1;

    Money money;

    public event EventHandler MoneyAmountChanged;

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

    public Funds()
    {
        if (instance != null)
        {
            Debug.LogError("Попытка создать еще второй экземпляр Game на сцене");
            instance = null;
        }
        instance = this;

        money = new Money(0, 0);

        ProductsController.Instance.ProductCreated += OnProductCreated;
    }

    public void Reset()
    {
        buyStep = 1;
        money = new Money(0, 0);
    }

    public bool EnoughMoney(Money amount)
    {
        return (money.IsGreaterThen(amount) || money.IsEqual(amount));
    }

    protected virtual void OnProductCreated(object source, EventArgs e)
    {
        Product product = (Product)source;
        product.ProductSold += OnProductSold;
    }

    protected virtual void OnProductSold(object source, EventArgs e)
    {
        Product product = (Product)source;
        AddMoneyAmount(product.GetAllMoney());

        OnMoneyAmountChanged();
    }

    public void AddMoneyAmount(Money amount)
    {
        money.Add(amount);
        OnMoneyAmountChanged();
    }

    public void AddMoneyAmount(float amount)
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

    #region IXmlSerializable
    public XmlSchema GetSchema()
    {
        throw new NotImplementedException();
    }

    public void ReadXml(XmlReader reader)
    {
        buyStep = int.Parse(reader.GetAttribute("BuyStep"));
        money.ReadXml(reader);
        OnMoneyAmountChanged();
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("BuyStep", buyStep.ToString());
        money.WriteXml(writer);
    }
    #endregion
}
