using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    //// 背景の枚数
    //private int spriteCount = 2;
    //// 背景が回り込み
    //private float rightOffset = 1.5f;
    //private float leftOffset = -0.5f;

    //private Transform bgTfm;
    //private SpriteRenderer mySpriteRndr;
    [SerializeField] private Transform player;
    //float width;

    void Start()
    {
        //bgTfm = transform;
        //mySpriteRndr = GetComponent<SpriteRenderer>();
        //width = mySpriteRndr.bounds.size.x;
        //transform.position = new Vector2(transform.position.x, player.position.y + 1.5f);
        transform.position = new Vector2(player.position.x, player.position.y + 1.0f);
    }


    void Update()
    {
        transform.position = new Vector2(player.position.x, player.position.y + 1.0f);
        //transform.position = new Vector2(transform.position.x, player.position.y + 1.5f);
        //// 座標変換
        //Vector3 myViewport = Camera.main.WorldToViewportPoint(bgTfm.position);
        //// Debug.Log(name + ":" + myViewport);

        //// 背景の回り込み(カメラがX軸プラス方向に移動時)
        //if (myViewport.x < leftOffset)
        //{
        //    bgTfm.position += Vector3.right * (width * spriteCount);
        //}
        //// 背景の回り込み(カメラがX軸マイナス方向に移動時)
        //else if (myViewport.x > rightOffset)
        //{
        //    bgTfm.position -= Vector3.right * (width * spriteCount);
        //}
    }
}