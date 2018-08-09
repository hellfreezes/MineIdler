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
    GameObject upgradesWindow;
    [SerializeField]
    GameObject achivmentsWindow;
    [SerializeField]
    GameObject topscoreWindow;
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
        OnMoneyAmountChanged(GameManager.Instance, EventArgs.Empty);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowHideManagersWindow()
    {
        upgradesWindow.SetActive(false);
        achivmentsWindow.SetActive(false);
        topscoreWindow.SetActive(false);
        managersWindow.SetActive(!managersWindow.activeSelf);
    }

    public void ShowHideUpgradesWindow()
    {
        managersWindow.SetActive(false);
        achivmentsWindow.SetActive(false);
        topscoreWindow.SetActive(false);
        upgradesWindow.SetActive(!upgradesWindow.activeSelf);
    }
    public void ShowHideAhivmentsWindow()
    {
        upgradesWindow.SetActive(false);
        managersWindow.SetActive(false);
        topscoreWindow.SetActive(false);
        achivmentsWindow.SetActive(!achivmentsWindow.activeSelf);
    }
    public void ShowHideTopscoreWindow()
    {
        upgradesWindow.SetActive(false);
        achivmentsWindow.SetActive(false);
        managersWindow.SetActive(false);
        topscoreWindow.SetActive(!topscoreWindow.activeSelf);
    }

    void OnMoneyAmountChanged(object source, EventArgs e)
    {
        GameManager gameManager = (GameManager)source;
        string money = gameManager.Money.ToString();// ("#.##");
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
