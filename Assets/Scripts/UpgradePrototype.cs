using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePrototype {
    Upgrade upgrade;

    public UpgradePrototype(int multiplier, string description, ProductType productType, Money price)
    {
        upgrade = new Upgrade(multiplier, description, productType, price);
    }
}
