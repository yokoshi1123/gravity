using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GravityFieldTexture : MonoBehaviour
{
    private GravityManager gravityManager;
    private SpriteRenderer gFieldTexture;

    [Serializable] private struct TextureSet
    {
        public Sprite[] textures;
    }


    [SerializeField] TextureSet[] textureSets;

    private bool isChanging = false;

    private int gPattern;
    private int num = 0;
    
    // Start is called before the first frame update
    void Awake()
    {
        gravityManager = GetComponent<GravityManager>();
        //StartCoroutine(ChangeTexture());
    }

    // Update is called once per frame

    void Update()
    {
        gPattern = gravityManager.GetGScale();
        try
        {
            gFieldTexture = GameObject.FindWithTag("GravityField").GetComponent<SpriteRenderer>();
        }
        catch
        {
            gFieldTexture = null;
        }
        if (gFieldTexture != null && !isChanging)
        {
            StartCoroutine(ChangeTexture());
        }
    }

    private IEnumerator ChangeTexture()
    {
        isChanging = true;
        num = (num + 16001) % 16;
        //Debug.Log(gPattern + "," + num);
        gFieldTexture.sprite = textureSets[gPattern].textures[num];
        yield return new WaitForSecondsRealtime(0.1f);
        isChanging = false;
    }
}
