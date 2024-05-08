using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    // ”wŒi‚Ì–‡”
    int spriteCount = 2;
    // ”wŒi‚ª‰ñ‚è‚İ
    float rightOffset = 1.5f;
    float leftOffset = -0.5f;

    Transform bgTfm;
    SpriteRenderer mySpriteRndr;
    float width;

    void Start()
    {
        bgTfm = transform;
        mySpriteRndr = GetComponent<SpriteRenderer>();
        width = mySpriteRndr.bounds.size.x;
    }


    void Update()
    {
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