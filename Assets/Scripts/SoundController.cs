using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
    [SerializeField]
    bool playMusic = true;
    [SerializeField]
    bool playSound = true;
    [SerializeField]
    AudioClip onSold;
    [SerializeField]
    AudioClip[] onBuy;

    [SerializeField]
    AudioSource musicSource;
    static SoundController instance;

    public static SoundController Instance
    {
        get
        {
            return instance;
        }
    }

    public bool PlayMusic
    {
        get
        {
            return playMusic;
        }

        set
        {
            playMusic = value;
            MuteMusic();
        }
    }

    public bool PlaySound
    {
        get
        {
            return playSound;
        }

        set
        {
            playSound = value;
        }
    }

    Vector3 universalPosition;

    private void OnEnable()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        universalPosition = Camera.main.transform.position;

        if (playMusic)
            SetBackgrounMusic();

        ProductsController.Instance.ProductCreated += OnProductCreated;
    }

    void SetBackgrounMusic()
    {
        //AudioSource.PlayClipAtPoint(music, universalPosition);
        musicSource.Play();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    protected virtual void OnProductSold (object source, EventArgs e)
    {
        if (playSound)
            AudioSource.PlayClipAtPoint(onSold, universalPosition);
    }

    protected virtual void OnBuy (object source, EventArgs e)
    {
        int i = UnityEngine.Random.Range(0, onBuy.Length - 1);
        if (playSound)
            AudioSource.PlayClipAtPoint(onBuy[i], universalPosition);
    }

    protected virtual void OnProductCreated(object source, EventArgs e)
    {
        Product p = (Product)source;
        p.ProductSold += OnProductSold;
        p.BuildingPurchased += OnBuy;
    }

    void MuteMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
}
