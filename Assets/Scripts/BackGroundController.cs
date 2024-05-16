using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    // ”wŒi‚Ì–‡”
    private int spriteCount = 2;
    // ”wŒi‚ª‰ñ‚è‚İ
    private float rightOffset = 1.5f;
    private float leftOffset = -0.5f;

    private Transform bgTfm;
    private SpriteRenderer mySpriteRndr;
    [SerializeField] private Transform player;
    float width;

    void Start()
    {
        bgTfm = transform;
        mySpriteRndr = GetComponent<SpriteRenderer>();
        width = mySpriteRndr.bounds.size.x;
        transform.position = new Vector2(transform.position.x, player.position.y + 1.5f);
    }


    void Update()
    {
        transform.position = new Vector2(transform.position.x, player.position.y + 1.5f);
        // À•W•ÏŠ·
        Vector3 myViewport = Camera.main.WorldToViewportPoint(bgTfm.position);
        // Debug.Log(name + ":" + myViewport);

        // ”wŒi‚Ì‰ñ‚è‚İ(ƒJƒƒ‰‚ªX²ƒvƒ‰ƒX•ûŒü‚ÉˆÚ“®)
        if (myViewport.x < leftOffset)
        {
            bgTfm.position += Vector3.right * (width * spriteCount);
        }
        // ”wŒi‚Ì‰ñ‚è‚İ(ƒJƒƒ‰‚ªX²ƒ}ƒCƒiƒX•ûŒü‚ÉˆÚ“®)
        else if (myViewport.x > rightOffset)
        {
            bgTfm.position -= Vector3.right * (width * spriteCount);
        }
    }
}