using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagersController : MonoBehaviour {
    [SerializeField]
    Transform managersList;
    [SerializeField]
    GameObject managerPanelPrefab;


    List<Manager> managers;

	// Use this for initialization
	void Start () {
        managers = new List<Manager>();

        CreateManagers();
        CreateManagersList();
    }

    void CreateManagers()
    {
        Manager m = new Manager("Скрудж", "Профессиональный продавец пирогов. Найми его и жизнь станет проще.", "Pie", 2000f);
        managers.Add(m);
    }

    void CreateManagersList()
    {
        foreach(Manager m in managers)
        {
            GameObject panel = Instantiate(managerPanelPrefab);
            panel.transform.SetParent(managersList);
            panel.transform.Find("Name").GetComponent<Text>().text = m.Name;
            panel.transform.Find("Description").GetComponent<Text>().text = m.Description;
            panel.transform.Find("PriceValue").GetComponent<Text>().text = m.Price.ToString();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
