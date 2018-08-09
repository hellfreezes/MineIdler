﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagersController : MonoBehaviour {
    [SerializeField]
    Transform managersList;
    [SerializeField]
    GameObject managerPanelPrefab;
    [SerializeField]
    Sprite[] managerSprites;

    List<ManagerPrototype> prototypes;
    List<Manager> managers;

    static ManagersController instance;
    public static ManagersController Instance
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
            Debug.LogError("Обнаружено два контроллера менеджеров на сцене");
        } else
        {
            instance = this;
        }

        // Инициализация перенесена из этого класса в GameManager
    }

    public void Init()
    {
        managers = new List<Manager>();
        CreateManagersPrototypes();
        CreateManagersAndUI();
    }


    void CreateManagersPrototypes()
    {
        prototypes = new List<ManagerPrototype>();

        ManagerPrototype m = new ManagerPrototype("Скрудж", "Профессиональный продавец пирогов. Найми его и жизнь станет проще.", 
                                                    "Pie", new Money(2,0), managerSprites[0], ProductType.PIE);
        prototypes.Add(m);
        m = new ManagerPrototype("Джулес Винфилд", "Этот мужик знает толк в бургерах!",
                                                    "Burger", new Money(15, 0), managerSprites[1], ProductType.BURGER);
        prototypes.Add(m);
        m = new ManagerPrototype("Джейми Оливер", "Ресторан Джейми может приготовить вам ужин за 30 минут!",
                                                    "Restorant", new Money(100, 0), managerSprites[2], ProductType.RESTORANT);
        prototypes.Add(m);
        m = new ManagerPrototype("Декстер Морган", "Профессиональный мясник!",
                                                    "Butcher", new Money(500, 0), managerSprites[3], ProductType.BUTCHER);
        prototypes.Add(m);
    }

    void CreateManagersAndUI()
    {
        foreach(ManagerPrototype p in prototypes)
        {
            Manager m = new Manager(p);
            managers.Add(m);


            GameObject panel = Instantiate(managerPanelPrefab);
            panel.transform.SetParent(managersList);
            m.Panel = panel;
            panel.GetComponent<ManagerPanel>().AssignManager(m);
        }
    }
	
	// Update is called once per frame
	void Update () {
		foreach(Manager m in managers)
        {
            m.Update(Time.deltaTime);
        }
	}
}
