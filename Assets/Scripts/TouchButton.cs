using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchButton : MonoBehaviour
{
    [SerializeField] private List<TurnOn> to = new();
    [SerializeField] private Sprite OffTexture;
    [SerializeField] private Sprite OnTexture;

    private SpriteRenderer Renderer;

    private bool turnon;



    // Start is called before the first frame update
    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
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
        
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Movable"))
        {
            for(int i = 0; i < to.Count; i++)
            {
                to[i].SetTurnOn(true);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        for (int i = 0; i < to.Count; i++)
        {
            to[i].SetTurnOn(false);
        }
    }
}
