using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupArgs : EventArgs
{
    public Sprite image;
    public string header;
    public string line1;
    public string line2;
}

public class PopupController : MonoBehaviour {

    //UI
    [SerializeField]
    GameObject popupWindow;

    Image spriteUI;
    Text headerUI;
    Text line1UI;
    Text line2UI;

    RectTransform window;

	// Use this for initialization
	void Start () {
        window = popupWindow.GetComponent<RectTransform>();

        spriteUI = popupWindow.transform.Find("Image").GetComponent<Image>();
        headerUI = popupWindow.transform.Find("Header").GetComponent<Text>();
        line1UI = popupWindow.transform.Find("Line2").GetComponent<Text>();
        line2UI = popupWindow.transform.Find("Line3").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		if (popupWindow.activeSelf)
        {
            window.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        }
	}

    void OnNote(object source, PopupArgs e)
    {
        popupWindow.SetActive(true);
        spriteUI.sprite = e.image;
        headerUI.text = e.header;
        line1UI.text = e.line1;
        line2UI.text = e.line2;
    }
}
