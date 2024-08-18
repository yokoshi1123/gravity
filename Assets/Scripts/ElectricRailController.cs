using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TurnOn))]

public class ElectricRailController : MonoBehaviour
{

    private bool turnon;
    [SerializeField] private TurnOn to;

    [SerializeField] private Sprite railoffTexture;
    [SerializeField] private List<Sprite> railonTextures = new();
    [SerializeField] private Sprite edgeoffTexture;
    [SerializeField] private Sprite edgeonTexture;

    [SerializeField] private GameObject edge1;
    [SerializeField] private GameObject edge2;
    [SerializeField] private GameObject rail;

    private SpriteRenderer edge1Renderer;
    private SpriteRenderer edge2Renderer;
    private SpriteRenderer railRenderer;

    private int num;

    private Vector2 baseScale;

    private bool isChanging = false;

    


    //private Vector2 railposi;
    //private Vector2 railscale;


    // Start is called before the first frame update
    void Awake()
    {
        edge1Renderer = edge1.GetComponent<SpriteRenderer>();
        edge2Renderer = edge2.GetComponent<SpriteRenderer>();
        railRenderer = rail.GetComponent<SpriteRenderer>();
        baseScale = rail.transform.localScale;
    }

    void Start()
    {

        //edge2のローカルy座標をedge1のy座標とする。
        edge2.transform.localPosition = new Vector2(edge2.transform.localPosition.x, edge1.transform.localPosition.y);
        
    }

    // Update is called once per frame
    void Update()
    {
        turnon  = to.GetTurnOn();
        rail.transform.localPosition = new Vector2((edge1.transform.localPosition.x + edge2.transform.localPosition.x) * 0.5f, edge1.transform.localPosition.y);
        rail.transform.localScale = new Vector2((Mathf.Abs(edge1.transform.localPosition.x - edge2.transform.localPosition.x)), baseScale.y);
        //Debug.Log(edge1.transform.localPosition.x - edge2.transform.localPosition.x);

        if (turnon)
        {
            rail.tag = "Toxic";
            edge1Renderer.sprite = edgeonTexture;
            edge2Renderer.sprite = edgeonTexture;
        }
        else
        {
            rail.tag = "Untagged";
            edge1Renderer.sprite = edgeoffTexture;
            edge2Renderer.sprite = edgeoffTexture;
        }

        if (!isChanging)
        {
            StartCoroutine(RailOnCycle());
        }
    }


    private IEnumerator RailOnCycle()
    {
        isChanging = true;
        if (turnon)
        {
            railRenderer.sprite = railonTextures[num];
            num = (num + 1) % 2;
        }
        else
        {
            railRenderer.sprite = railoffTexture;
        }
        yield return new WaitForSecondsRealtime(0.1f);

    isChanging = false;
    }
}
