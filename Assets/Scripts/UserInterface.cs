using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {
    [SerializeField]
    Text moneyAmountText;
    [SerializeField]
    GameObject managersWindow;
    [SerializeField]
    Sprite[] soundSprites;
    [SerializeField]
    Sprite[] musicSprites;
    [SerializeField]
    Image soundButton;
    [SerializeField]
    Image musicButton;

	// Use this for initialization
	void Start () {
        GameManager.Instance.MoneyAmountChanged += OnMoneyAmountChanged;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowHideManagersWindow()
    {
        managersWindow.SetActive(!managersWindow.activeSelf);
    }

    void OnMoneyAmountChanged(object source, EventArgs e)
    {
        GameManager gameManager = (GameManager)source;
        string money = gameManager.Money.ToString("#.##");
        if (gameManager.Money < 1)
        {
            money = "0" + money;
        } 
        moneyAmountText.text = money;
    }

    public void SoundOnOff()
    {
        bool sound = SoundController.Instance.PlaySound;
        SoundController.Instance.PlaySound = !sound;
        soundButton.sprite = soundSprites[!sound == true ? 0 : 1];
    }

    public void MusicOnOff()
    {
        bool music = SoundController.Instance.PlayMusic;
        SoundController.Instance.PlayMusic = !music;
        musicButton.sprite = musicSprites[!music == true ? 0 : 1];
    }
}
