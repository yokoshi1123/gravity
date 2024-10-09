using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Controller: MonoBehaviour
{
    [SerializeField] private bool isAvailable = true;

    [SerializeField] private Sprite OffTexture;
    [SerializeField] private Sprite OnTexture;
    [SerializeField] private GameObject HologramText;


    private GravityManager gravityManager;

    private SpriteRenderer Renderer;




    // Start is called before the first frame update
    void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Renderer.sprite = OffTexture;
        HologramText.SetActive(false);
        gravityManager = GameObject.Find("GravityManager").GetComponent<GravityManager>();
        gravityManager.SetIsAvailable(false);
    }

    // Update is called once per frame
    /*void Update()
    {
        turnon = to[0].GetTurnOn();

        if(turnon)
        {
            Renderer.sprite = OnTexture;
        }
        else
        {
            Renderer.sprite = OffTexture;
        }

    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isAvailable)
        {
            Renderer.sprite = OnTexture;
            HologramText.SetActive(true);
            gravityManager.SetIsAvailable(true);
            GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip, 0.2f);
        }

    }

   
}
