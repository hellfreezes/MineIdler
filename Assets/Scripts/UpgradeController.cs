using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour {
    [SerializeField]
    Transform upgradesListPanel;
    [SerializeField]
    GameObject upgradePanelPrefab;

    List<UpgradePrototype> prototypes;
    List<Upgrade> upgrades;

    static UpgradeController instance;
    public static UpgradeController Instance
    {
        get
        {
            return instance;
        }
    }

    // Use this for initialization
    void Start() {
        if (instance != null)
        {
            Debug.LogError("Обнаружено два контроллера апгрейдов на сцене");
        }
        else
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void Init()
    {
        Create();
        InstallUI();
    }

    private void Create()
    {
        upgrades = new List<Upgrade>();

        Upgrade newUpgrade = new Upgrade(3, "Пристройка к пирожковой", ProductType.PIE, new Money(0.1f, 0));
        upgrades.Add(newUpgrade);
        newUpgrade = new Upgrade(3, "Пристройка к бургерной", ProductType.BURGER, new Money(1f, 0));
        upgrades.Add(newUpgrade);
    }

    private void InstallUI()
    {
        foreach (Upgrade upgrade in upgrades)
        {
            GameObject panel = Instantiate(upgradePanelPrefab);
            panel.transform.SetParent(upgradesListPanel);
            upgrade.Panel = panel;
            panel.GetComponent<UpgradePanel>().Assign(upgrade);
        }
    }


}
