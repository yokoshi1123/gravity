using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointUpdater : MonoBehaviour
{
    [SerializeField] private SpriteRenderer texture;
    public Sprite before;
    public Sprite after;

    void Awake()
    {
        gameObject.SetActive(true);
        texture.sprite = before;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().respawnPoint = transform.position;
            gameObject.SetActive(false);
            texture.sprite = after;
        }
    }
}
