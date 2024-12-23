using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GravityFieldTexture : MonoBehaviour
{
    [SerializeField] private bool isFixed = false;
    /*[SerializeField]*/ private GravityManager gravityManager;
    /*[SerializeField]*/ private SpriteRenderer gFieldTexture;

    [Serializable] private struct TextureSet
    {
        public Sprite[] textures;
    }

    [SerializeField] TextureSet[] textureSets;

    //private bool isChanging = false;

    [SerializeField] private int gPattern = 3;
    private int num = 0;


    
    // Start is called before the first frame update
    void Awake()
    {
        //gravityManager = GetComponent<GravityManager>();
        try
        { 
            gravityManager = GameObject.Find("GravityManager").GetComponent<GravityManager>();
            isFixed = !gravityManager.GetIsAvailable();
        }
        catch { }
        gFieldTexture = GetComponent<SpriteRenderer>();
        StartCoroutine(ChangeTexture());

        if (isFixed)
        {
            GetComponent<BoxCollider2D>().size = GetComponent<SpriteRenderer>().size;
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    // Update is called once per frame

    void Update()
    {
        if (!isFixed)
        {
            gPattern = gravityManager.GetGScale();
        }

        //try
        //{
        //    gFieldTexture = GameObject.FindWithTag("GravityField").GetComponent<SpriteRenderer>();
        //}
        //catch
        //{
        //    gFieldTexture = null;
        //}
        //if (gFieldTexture != null && !isChanging)
        //{
        //    StartCoroutine(ChangeTexture());
        //}
    }

    public int GetGPattern()
    {
        return gPattern;
    }

    private IEnumerator ChangeTexture()
    {
        //isChanging = true;
        while (true) 
        {
            num = (num + 16001) % 16;
            //Debug.Log(gPattern + "," + num);
            gFieldTexture.sprite = textureSets[gPattern].textures[num];
            yield return new WaitForSecondsRealtime(0.1f);
            //isChanging = false;
        }

    }

    public void SetIsFixed(bool value)
    {
        isFixed = value;
    }

    public void SetGPattern(int pattern)
    {
        if (pattern >= 0 && pattern <= 3) gPattern = pattern;
    }
}
