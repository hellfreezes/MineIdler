using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagersController : MonoBehaviour {
    List<Manager> managers;

	// Use this for initialization
	void Start () {
		
	}

    void CreateManagers()
    {
        Manager m = new Manager("Скрудж", "Профессиональный продавец пирогов. Найми его и жизнь станет проще.", "Pie", 2000f);
        managers.Add(m);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
