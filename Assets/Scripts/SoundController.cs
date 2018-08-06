using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
    [SerializeField]
    bool playMusic = true;
    [SerializeField]
    AudioClip music;
    [SerializeField]
    AudioClip onSold;
    [SerializeField]
    AudioClip[] onBuy;

    static SoundController instance;

    public static SoundController Instance
    {
        get
        {
            return instance;
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
    }

    void SetBackgrounMusic()
    {
        AudioSource.PlayClipAtPoint(music, universalPosition);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnProductSold (object source, EventArgs e)
    {
        AudioSource.PlayClipAtPoint(onSold, universalPosition);
    }

    public void OnBuy (object source, EventArgs e)
    {
        int i = UnityEngine.Random.Range(0, onBuy.Length - 1);
        AudioSource.PlayClipAtPoint(onBuy[i], universalPosition);
    }
}
