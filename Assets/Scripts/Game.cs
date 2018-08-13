using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public class Game : IXmlSerializable {
    //private static Game instance;
    //public static Game Instance
    //{
    //    get { return instance; }
    //}

    //public Game()
    //{
    //    if (instance != null)
    //    {
    //        Debug.LogError("Дублирование Game. Удаляю ссылку и заменяю на ссылку на новый объект! Выяснить в чем причина.");
    //    }
    //    instance = this;
    //}

    #region IXmlSerializable
    public XmlSchema GetSchema()
    {
        throw new System.NotImplementedException();
    }

    public void ReadXml(XmlReader reader)
    {
        reader.ReadToFollowing("Funds");
        Funds.Instance.ReadXml(reader);
        reader.ReadToFollowing("Products");
        reader.ReadToDescendant("Product");
        do
        {
            ProductType productType = (ProductType)int.Parse(reader.GetAttribute("ProductType"));
            ProductsController.Instance.GetProductFromList(productType).ReadXml(reader);

        } while (reader.ReadToNextSibling("Product"));
        if (reader.ReadToFollowing("Managers"))
        {
            reader.ReadToDescendant("Manager");
            do
            {
                ProductType productType = (ProductType)int.Parse(reader.GetAttribute("ProductType"));
                ManagersController.Instance.GetManagerOfType(productType).ReadXml(reader);

            } while (reader.ReadToNextSibling("Manager"));
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        writer.WriteStartElement("Funds");
        Funds.Instance.WriteXml(writer);
        writer.WriteEndElement();
        writer.WriteStartElement("Products");
        foreach (Product product in ProductsController.Instance.GetProducts())
        {
            writer.WriteStartElement("Product");
            writer.WriteAttributeString("ProductType", ((int)product.ProductType).ToString());
            product.WriteXml(writer);
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
        writer.WriteStartElement("Managers");
        foreach (Manager manager in ManagersController.Instance.GetManagers())
        {
            writer.WriteStartElement("Manager");
            writer.WriteAttributeString("ProductType", ((int)manager.ProductType).ToString());
            manager.WriteXml(writer);
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
    }
    #endregion
}
