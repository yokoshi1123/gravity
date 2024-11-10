using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSpritesController : MonoBehaviour
{
    [SerializeField] private float duration = 1.0f;
    [SerializeField] private float ONduration = 5.0f;
    [SerializeField] private float OFFduration = 1.0f;
    private GameObject[] lights;
    private SpriteRenderer[][] lightSprites; 


    // Start is called before the first frame update
    void Start()
    {
        


        lights = new GameObject[3];  // lights 配列を初期化
        lightSprites = new SpriteRenderer[3][];  // lightSprites 配列を初期化

        lights[0] = transform.Find("light_B").gameObject;
        lights[1] = transform.Find("light_R").gameObject;
        lights[2] = transform.Find("light_P").gameObject;

        for ( int i = 0; i < 3; i++)
        {
            lightSprites[i] = lights[i].GetComponentsInChildren < SpriteRenderer > ();

            //Debug.Log($"light_{i} has {lightSprites[i].Length} SpriteRenderers.");

            if (lightSprites[i].Length == 0)
            {
                Debug.LogError($"No SpriteRenderers found for light_{i}.");
            }
        }

        for (int i = 0; i < 3; i++)
        {
            foreach (SpriteRenderer lightSprite in lightSprites[i])
            {
                Color color = lightSprite.color;  // color を取得
                color.a = 0f;                     // アルファ値（透明度）を変更
                lightSprite.color = color;        // 変更後の color を再設定
            }
        }
        StartCoroutine(FadeCycle());
        //StartCoroutine(Fade(0, 1));
    }

    // Update is called once per frame
    void Update()
    {
        //無限ループでフリーズします!!危険!!
        /*int count = 0;
        while(true)
        {
            int lightnum = count % 3;

            StartCoroutine(FadeInAndOut(lightnum));

            count++;
        }*/


    }

    IEnumerator Fade(int lightnum, float targetAlpha)
    {
        Color spriteColor = lightSprites[lightnum][0].color;

        while (!Mathf.Approximately(spriteColor.a, targetAlpha))
        {
            float changePerFrame = Time.deltaTime / duration;
            spriteColor.a = Mathf.MoveTowards(spriteColor.a, targetAlpha, changePerFrame);
            
            foreach (SpriteRenderer lightSprite in lightSprites[lightnum])
            lightSprite.color = spriteColor;
            yield return null;

            //Debug.Log($"lightnum {lightnum}, Alpha: {spriteColor.a}");
        }
    }

    IEnumerator FadeInAndOut(int lightnum)
    {
        // フェードイン（透明度0 -> 1）
        yield return StartCoroutine(Fade(lightnum, 1));

        //待機
        yield return new WaitForSecondsRealtime(ONduration);

        // フェードアウト（透明度1 -> 0）
        yield return StartCoroutine(Fade(lightnum, 0));

        //待機
        yield return new WaitForSecondsRealtime(OFFduration);
    }

    IEnumerator FadeCycle()
    {
        int count = 0;

        // 無限ループでフェード処理を繰り返す
        while (true)
        {
            int lightnum = count % 3;

            // フェードイン・フェードアウトの処理を実行
            yield return StartCoroutine(FadeInAndOut(lightnum));

            count++;
        }
    }

}
