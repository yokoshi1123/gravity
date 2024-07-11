using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public bool respawned = false;
    [SerializeField] private GameObject PlayerAvatar;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //PlayerAvatar.SetActive(false);
        //Debug.Log("è¡Ç¶ÇÈ");
    }

    void AnimationController()
    {
        /*PlayerAvatar.SetActive(false);
        transform.position = respawnPoint;
        while (!respawned) { }
        PlayerAvator.SetActive(true);
        respawned = false;*/
    }
}
