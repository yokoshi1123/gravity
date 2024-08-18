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
    private Transform hitObj;
    private const float RAYDISTANCE = 50f;

    private Vector3 basePos;

    private BoxCollider2D beamCollider;
    [SerializeField] private BoxCollider2D beamCollider2;

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
        beamCollider = GetComponent<BoxCollider2D>();

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
        beamCollider.enabled = false;
        beamCollider2.enabled = false;
        if (beamRenderer.sprite != null)
        {        
            if (isVertical)
            {
                //Debug.DrawRay(basePos, Vector2.down * machine.transform.localScale.y * RAYDISTANCE, Color.green, 0.015f);
                int layerMask = ~(1 << 2 | 1 << 7);
                ////Debug.Log(layerMask);
                hit = Physics2D.Raycast(basePos, Vector2.down * machine.transform.localScale.y, RAYDISTANCE, layerMask);
                if (hit.collider != null) // && !hit.collider.CompareTag("Player"))
                {
                    hitObj = hit.collider.transform;
                    Debug.Log(hit.collider.name);
                    //Debug.Log(hitObj.position.y + Mathf.Sign(machine.transform.localScale.y) * hitObj.localScale.y * 0.5f); // (basePos.x + hitObj.position.y + 9) * 0.5f); // + Mathf.Sign(machine.transform.localScale.y) * hitObj.localScale.y * 0.5f) * 0.5f);
                    transform.position = new Vector2(basePos.x, (basePos.y + hitObj.position.y + Mathf.Sign(machine.transform.localScale.y) * hitObj.localScale.y * 0.5f) * 0.5f);
                    transform.localScale = new Vector2(Mathf.Abs(basePos.y - hitObj.position.y) - hitObj.localScale.y * 0.5f - 0.05f, transform.localScale.y); //, beamTransform.localScale.z);
                }
            }
            else
            {
                //Debug.DrawRay(basePos, Vector2.left * RAYDISTANCE, Color.green, 0.015f);
                int layerMask = ~(1 << 2 | 1 << 6);
                hit = Physics2D.Raycast(basePos, Vector2.left * machine.transform.localScale.x, RAYDISTANCE, layerMask);
                if (hit.collider != null) // && !hit.collider.CompareTag("Player"))
                {
                    hitObj = hit.collider.transform;
                    Debug.Log(hit.collider.name);
                    //Debug.Log(hitObj.position);
                    transform.position = new Vector2((basePos.x + hitObj.position.x + Mathf.Sign(machine.transform.localScale.x) * hitObj.localScale.x * 0.5f) * 0.5f, basePos.y);
                    transform.localScale = new Vector2(Mathf.Abs(basePos.x - hitObj.position.x) - hitObj.localScale.x * 0.5f - 0.05f, transform.localScale.y); //, beamTransform.localScale.z);
                }
            }

            beamCollider.enabled = true;
            beamCollider2.enabled = true;
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
                //Debug.Log(i);
            }
            beamRenderer.sprite = beamTextures[5];
            yield return new WaitForSeconds(ONDURATION);
            //beamRenderer.sprite = beamTextures[4];
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(LaserWait());
        //beamCollider.enabled = false;
        //if (beamRenderer.sprite != null)
        //{        
        //    if (isVertical)
        //    {
        //        //Debug.DrawRay(basePos, Vector2.down * machine.transform.localScale.y * RAYDISTANCE, Color.green, 0.015f);
        //        int layerMask = ~(1 << 2 | 1 << 7);
        //        ////Debug.Log(layerMask);
        //        hit = Physics2D.Raycast(basePos, Vector2.down * machine.transform.localScale.y, RAYDISTANCE, layerMask);
        //        if (hit.collider != null) // && !hit.collider.CompareTag("Player"))
        //        {
        //            hitObj = hit.collider.transform;
        //            Debug.Log(hit.collider.name);
        //            //Debug.Log(hitObj.position.y + Mathf.Sign(machine.transform.localScale.y) * hitObj.localScale.y * 0.5f); // (basePos.x + hitObj.position.y + 9) * 0.5f); // + Mathf.Sign(machine.transform.localScale.y) * hitObj.localScale.y * 0.5f) * 0.5f);
        //            transform.position = new Vector2(basePos.x, (basePos.y + hitObj.position.y + Mathf.Sign(machine.transform.localScale.y) * hitObj.localScale.y * 0.5f) * 0.5f);
        //            transform.localScale = new Vector2(Mathf.Abs(basePos.y - hitObj.position.y) - hitObj.localScale.y * 0.5f - 0.05f, transform.localScale.y); //, beamTransform.localScale.z);
        //        }
        //    }
        //    else
        //    {
        //        //Debug.DrawRay(basePos, Vector2.left * RAYDISTANCE, Color.green, 0.015f);
        //        int layerMask = ~(1 << 2 | 1 << 6);
        //        hit = Physics2D.Raycast(basePos, Vector2.left * machine.transform.localScale.x, RAYDISTANCE, layerMask);
        //        if (hit.collider != null) // && !hit.collider.CompareTag("Player"))
        //        {
        //            hitObj = hit.collider.transform;
        //            //Debug.Log(hit.collider.name);
        //            //Debug.Log(hitObj.position);
        //            transform.position = new Vector2((basePos.x + hitObj.position.x + Mathf.Sign(machine.transform.localScale.x) * hitObj.localScale.x * 0.5f) * 0.5f, basePos.y);
        //            transform.localScale = new Vector2(Mathf.Abs(basePos.x - hitObj.position.x) - hitObj.localScale.x * 0.5f - 0.05f, transform.localScale.y); //, beamTransform.localScale.z);
        //        }
        //    }

        //    beamCollider.enabled = true;
        //}
        //if (!collision.gameObject.CompareTag("Player") || collision.gameObject.GetComponent<PlayerController>().GetIsGrabbing())
        //{
        //    //StopCoroutine(LaserCycle());
        //    beamCollider.enabled = false;
        //    beamRenderer.sprite = null;
        //    //int layerMask = ~(1 << 2 | 1 << 6);
        //    //hit = Physics2D.Raycast(basePos, Vector2.left * machine.transform.localScale.x, RAYDISTANCE, layerMask);
        //    //if (hit.collider != null) // && !hit.collider.CompareTag("Player"))
        //    //{
        //    //    hitObj = hit.collider.transform;
        //    //    //Debug.Log(hit.collider.name);
        //    //    //Debug.Log(hitObj.position);
        //    //    transform.position = new Vector2((basePos.x + hitObj.position.x + Mathf.Sign(machine.transform.localScale.x) * hitObj.localScale.x * 0.5f) * 0.5f, basePos.y);
        //    //    transform.localScale = new Vector2(Mathf.Abs(basePos.x - hitObj.position.x) - hitObj.localScale.x * 0.5f - 0.05f, transform.localScale.y); //, beamTransform.localScale.z);
        //    //}
        //    //StartCoroutine(LaserCycle());
        //}

        //Debug.Log(collision.gameObject.name);
        //if (collision.gameObject.name == "Player")
        //{
        //    collision.gameObject.GetComponent<PlayerController>().SetIsPlayer(true);
        //}
        //else if (collision.gameObject.CompareTag("Player"))
        //{
        //    Debug.Log("Safe");
        //    collision.gameObject.GetComponentInParent<PlayerController>().SetIsPlayer(false);
        //}

    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.name == "Player")
    //    {
    //        collision.gameObject.GetComponent<PlayerController>().SetIsPlayer(false);
    //    }
    //}

    private IEnumerator LaserWait()
    {
        beamCollider.enabled = false;
        beamCollider2.enabled = false;
        Debug.Log("Hit");
        yield return new WaitForSeconds(1f);
    }
}
