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
        


        lights = new GameObject[3];  // lights �z���������
        lightSprites = new SpriteRenderer[3][];  // lightSprites �z���������

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
                Color color = lightSprite.color;  // color ���擾
                color.a = 0f;                     // �A���t�@�l�i�����x�j��ύX
                lightSprite.color = color;        // �ύX��� color ���Đݒ�
            }
        }
        StartCoroutine(FadeCycle());
        //StartCoroutine(Fade(0, 1));
    }

    // Update is called once per frame
    void Update()
    {
        //�������[�v�Ńt���[�Y���܂�!!�댯!!
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
        // �t�F�[�h�C���i�����x0 -> 1�j
        yield return StartCoroutine(Fade(lightnum, 1));

        //�ҋ@
        yield return new WaitForSecondsRealtime(ONduration);

        // �t�F�[�h�A�E�g�i�����x1 -> 0�j
        yield return StartCoroutine(Fade(lightnum, 0));

        //�ҋ@
        yield return new WaitForSecondsRealtime(OFFduration);
    }

    IEnumerator FadeCycle()
    {
        int count = 0;

        // �������[�v�Ńt�F�[�h�������J��Ԃ�
        while (true)
        {
            int lightnum = count % 3;

            // �t�F�[�h�C���E�t�F�[�h�A�E�g�̏��������s
            yield return StartCoroutine(FadeInAndOut(lightnum));

            count++;
        }
    }

}
