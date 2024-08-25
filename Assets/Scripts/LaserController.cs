using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField] private bool isVertical = false;
    
    [SerializeField] private float OFFSET = 1f;
    [SerializeField] private float ONDURATION = 2f;
    [SerializeField] private float OFFDURATION = 3f;

    [SerializeField] private float ONTEXTURE = 14;

    private const int TRANSITION = 17;

    private RaycastHit2D hit;
    //private Transform hitObj;
    private const float RAYDISTANCE = 50f;

    private Vector3 basePos;

    //private BoxCollider2D beamCollider;
    //[SerializeField] private BoxCollider2D beamCollider2;

    [SerializeField] private GameObject machine;

    private SpriteRenderer machineRenderer;
    private SpriteRenderer beamRenderer;

    [SerializeField] private List<Sprite> machineTextures = new();
    [SerializeField] private List <Sprite> beamTextures = new();

    //private IEnumerator cycleEra;

    void Awake()
    {       
        basePos = transform.position;
        //Debug.Log(basePos);
        //beamCollider = GetComponent<BoxCollider2D>();

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
        //beamCollider.enabled = false;
        //beamCollider2.enabled = false;
        if (beamRenderer.sprite != null)
        {        
            if (isVertical)
            {
                //Debug.DrawRay(basePos, Vector2.down * machine.transform.localScale.y * RAYDISTANCE, Color.green, 0.015f);
                int layerMask = ~(1 << 2 | 1 << 7 | 1 << 8);
                //Debug.Log(layerMask);
                hit = Physics2D.Raycast(basePos, Vector2.down * machine.transform.localScale.y, RAYDISTANCE, layerMask);
                if (hit.collider != null)
                {
                    Vector2 hitPos = hit.collider.ClosestPoint(basePos);
                    //Debug.Log(hit.collider.gameObject.name + " : " + hitPos + " / " + hit.collider.transform.position);
                    transform.position = new Vector2(basePos.x, (basePos.y + hitPos.y) * 0.5f);
                    //transform.localScale = new Vector2(Mathf.Abs(basePos.y - hitPos.y), transform.localScale.y);
                    beamRenderer.size = new Vector2(Mathf.Abs(basePos.y - hitPos.y), beamRenderer.size.y);
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        //Debug.Log(":(");
                        StartCoroutine(hit.collider.gameObject.GetComponent<PlayerController>().Respawn());
                    }

                    //if (hit.collider.gameObject.CompareTag("Player"))
                    //{
                    //    Debug.Log(":(");
                    //    transform.position = new Vector2(basePos.x, (basePos.y + hitPos.y) * 0.5f);
                    //    transform.localScale = new Vector2(Mathf.Abs(basePos.y - hitPos.y) + 0.01f, transform.localScale.y);
                    //}
                    //else
                    //{
                    //    //Debug.Log(":)");
                    //    transform.position = new Vector2(basePos.x, (basePos.y + hitPos.y) * 0.5f + 0.01f * transform.localScale.y);
                    //    transform.localScale = new Vector2(Mathf.Abs(basePos.y - hitPos.y) - 0.05f, transform.localScale.y);
                    //}
                }
            }
            else
            {
                //Debug.DrawRay(basePos, Vector2.left * RAYDISTANCE, Color.green, 0.015f);
                int layerMask = ~(1 << 2 | 1 << 6 | 1 << 8);
                hit = Physics2D.Raycast(basePos, Vector2.left * machine.transform.localScale.x, RAYDISTANCE, layerMask);
                if (hit.collider != null)
                {
                    Vector2 hitPos = hit.collider.ClosestPoint(basePos);
                    //Debug.Log(hit.collider.name + " : " + hitPos);
                    transform.position = new Vector2((basePos.x + hitPos.x) * 0.5f, basePos.y);
                    //transform.localScale = new Vector2(Mathf.Abs(basePos.x - hitPos.x), transform.localScale.y);
                    beamRenderer.size = new Vector2(Mathf.Abs(basePos.x - hitPos.x), beamRenderer.size.y);
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        StartCoroutine(hit.collider.gameObject.GetComponent<PlayerController>().Respawn());
                    }

                    //transform.position = new Vector2((basePos.x + hitPos.x) * 0.5f + 0.01f * transform.localScale.y, basePos.y);
                    //transform.localScale = new Vector2(Mathf.Abs(basePos.x - hitPos.x) - 0.05f, transform.localScale.y);
                    //if (hit.collider.gameObject.CompareTag("Player"))
                    //{
                    //    transform.position = new Vector2((basePos.x + hitPos.x) * 0.5f, basePos.y);
                    //    transform.localScale = new Vector2(Mathf.Abs(basePos.x - hitPos.x), transform.localScale.y);
                    //}
                    //else
                    //{
                    //    transform.position = new Vector2((basePos.x + hitPos.x) * 0.5f + 0.01f * transform.localScale.y, basePos.y);
                    //    transform.localScale = new Vector2(Mathf.Abs(basePos.x - hitPos.x) - 0.05f, transform.localScale.y);
                    //}
                }
            }

            //beamCollider.enabled = true;
        }
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    //if (!other.CompareTag("Player"))
    //    //{
    //    //    beamCollider.enabled = false;
    //    //}
    //    if (isVertical)
    //    {
    //        Vector2 hitPos = other.ClosestPoint(basePos);
    //        if (hit.collider.gameObject.CompareTag("Player"))
    //        {
    //            Debug.Log(":(");
    //            transform.position = new Vector2(basePos.x, (basePos.y + hitPos.y) * 0.5f);
    //            transform.localScale = new Vector2(Mathf.Abs(basePos.y - hitPos.y) + 0.01f, transform.localScale.y);
    //        }
    //        else
    //        {
    //            //Debug.Log(":)");
    //            transform.position = new Vector2(basePos.x, (basePos.y + hitPos.y) * 0.5f + 0.01f * transform.localScale.y);
    //            transform.localScale = new Vector2(Mathf.Abs(basePos.y - hitPos.y) - 0.05f, transform.localScale.y);
    //        }
    //    }
    //    else
    //    {
    //        Vector2 hitPos = hit.collider.ClosestPoint(basePos);
    //        if (hit.collider.gameObject.CompareTag("Player"))
    //        {
    //            transform.position = new Vector2((basePos.x + hitPos.x) * 0.5f, basePos.y);
    //            transform.localScale = new Vector2(Mathf.Abs(basePos.x - hitPos.x), transform.localScale.y);
    //        }
    //        else
    //        {
    //            transform.position = new Vector2((basePos.x + hitPos.x) * 0.5f + 0.01f * transform.localScale.y, basePos.y);
    //            transform.localScale = new Vector2(Mathf.Abs(basePos.x - hitPos.x) - 0.05f, transform.localScale.y);
    //        }
    //    }
    //}
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
                //Debug.Log(i);
            }
            beamRenderer.sprite = beamTextures[5];
            yield return new WaitForSeconds(ONDURATION);
            beamRenderer.sprite = beamTextures[4];
            for (int i = (int)ONTEXTURE; i < TRANSITION; i++)
            {
                yield return new WaitForSeconds(ONDURATION / ONTEXTURE);
                machineRenderer.sprite = machineTextures[i];
                beamRenderer.sprite = beamTextures[TRANSITION - i];
                //Debug.Log(i);
            }
            beamRenderer.sprite = null;
            yield return new WaitForSeconds(OFFDURATION);
        }
    }
}
