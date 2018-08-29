using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField]
    float startMoney = 0.004f;
    [SerializeField]
    float saveIntervale = 30f;

    public Funds funds;

    private Game game;
    private float timer = 0;

    static GameManager instance;

    public static GameManager Instance
    {
        get { return instance;  }
    }

    // Use this for initialization
    void OnEnable () {
        if (instance != null)
        {
            Debug.LogError("Менеджер игры не один на сцене!");
            Destroy(gameObject);
        }
        instance = this;
    }

    private void Start()
    {
        // Выполнить сохранение при обноружении выхода из приложения
        Application.quitting += SaveGame;

        timer = saveIntervale;
        game = new Game();
        funds = new Funds();

        ProductsController.Instance.Init();
        ManagersController.Instance.Init();
        UpgradeController.Instance.Init();

        funds.AddMoneyAmount(startMoney);
        //Autoload();
    }

    private void Update()
    {
        Autosave();
    }

    private void Autoload()
    {
        LoadGame();
    }

    public void ResetGame()
    {
        funds.Reset();
        ProductsController.Instance.ResetAll();
        ManagersController.Instance.ResetAll();

        funds.AddMoneyAmount(startMoney);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Autosave()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SaveGame();
            timer = saveIntervale;
        }
    }

    public void SaveGame()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Game));
        TextWriter writer = new StringWriter();
        serializer.Serialize(writer, game);
        Debug.Log("Игра сохранена");
        Debug.Log(writer.ToString());
        writer.Close();
        PlayerPrefs.SetString("save01", writer.ToString());
    }

    public void LoadGame()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Game));
        TextReader reader = new StringReader(PlayerPrefs.GetString("save01"));
        game = (Game)serializer.Deserialize(reader);
        Debug.Log("Игра загружена");
        reader.Close();
    }
}
