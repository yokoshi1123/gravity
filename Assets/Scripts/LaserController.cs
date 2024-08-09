using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField] private float OFFSET = 3f;
    [SerializeField] private float ONDURATION = 3f;
    [SerializeField] private float OFFDURATION = 6f;

    [SerializeField] private float ONTEXTURE = 14;

    private const int TRANSITION = 17;

    private RaycastHit2D hit;
    private const float RAYDISTANCE = 50f;

    private Vector3 basePos;
    private Vector3 offsetPos;

    private BoxCollider2D beamCollider;

    [SerializeField] private GameObject machine;

    private SpriteRenderer machineRenderer;
    private SpriteRenderer beamRenderer;

    [SerializeField] private List<Sprite> machineTextures = new List<Sprite>();
    [SerializeField] private List <Sprite> beamTextures = new List<Sprite>();

    void Awake()
    {       
        basePos = transform.position;
        beamCollider = GetComponent<BoxCollider2D>();
        
        offsetPos = machine.transform.position;
        machineRenderer = machine.GetComponent<SpriteRenderer>();
        //machineRenderer.sprite = machineTextures[4];
        
        beamRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        StartCoroutine(LaserCycle());
    }

    void Update()
    {
        //Debug.DrawRay(basePos, Vector2.left * RAYDISTANCE, Color.green, 0.015f);
        beamCollider.enabled = false;
        hit = Physics2D.Raycast(basePos, Vector2.left * machine.transform.localScale.x, RAYDISTANCE);
        //while (hit.collider != null && hit.collider.name == this.name)
        //{
        //    hit = Physics2D.Raycast(hit.collider.transform.position, Vector2.left, RAYDISTANCE);
        //}
        if (hit.collider != null && !hit.collider.CompareTag("Player"))
        {
            Transform hitObj = hit.collider.transform;
            //Debug.Log(hitObj.position);
            transform.position = new Vector2((basePos.x + hitObj.position.x + hitObj.localScale.x / 2) / 2, basePos.y);
            transform.localScale = new Vector2(Mathf.Abs(basePos.x - hitObj.position.x) - hitObj.localScale.x / 2 - 0.05f, transform.localScale.y); //, beamTransform.localScale.z);
        }
        if (beamRenderer.sprite != null)
        {
            beamCollider.enabled = true;
        }
    }

    private IEnumerator LaserCycle()
    {
        yield return new WaitForSeconds(OFFSET);
        while (true)
        {
            for (int i = 0; i < ONTEXTURE; i++)
            {
                if (i == ONTEXTURE - 1)
                {
                    beamRenderer.sprite = beamTextures[6];
                }
                yield return new WaitForSeconds(ONDURATION / ONTEXTURE);
                machineRenderer.sprite = machineTextures[i];
            }
            beamRenderer.sprite = beamTextures[5];
            yield return new WaitForSeconds(ONDURATION);
            //beamRenderer.sprite = beamTextures[4];
            for (int i = (int)ONTEXTURE; i < TRANSITION; i++)
            {
                yield return new WaitForSeconds(ONDURATION / ONTEXTURE);
                machineRenderer.sprite = machineTextures[i];
                beamRenderer.sprite = beamTextures[TRANSITION - i];
            }
            beamRenderer.sprite = null;
            yield return new WaitForSeconds(OFFDURATION);
        }
    }
}
