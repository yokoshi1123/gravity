using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Threading.Tasks;//追加

public class RespawnManager : MonoBehaviour
{
    //以下はPlayerに渡すbool
    public bool respawn = false;
    public bool canMove = true;
    //public bool canActive = false;
    

    //以下はAnimationのつなぎのbool
    public bool resAnimation = false;


    //以下はAnimationから渡されるbool
    public bool respawning1 = false;
    public bool respawning2 = false;

    //以下は初期位置かどうかを判定するint
    //public int changePosi = 0;

    //以下はrespawnの位置が変更されたときにtrueを返す
    //public bool respawnchanged = false;

    private GameObject player;

    private GravityManager gravityManager;
    private GameObject pauseButton;

    ///*[SerializeField]*/ private GameObject playerAvatar;
    private SpriteRenderer playerAvatar;
    private PlayerController playerController;

    private int respawn_index_current = 0;
    private int respawn_index_length = 0;
    private GameObject[] RespawnObjectsList;

    private Vector3 respawnPoint;

    [Header("リスポーン1")][SerializeField] private AudioClip respawnSE1;
    [Header("リスポーン2")][SerializeField] private AudioClip respawnSE2;


    private GameObject fadeCanvas;
    private FadeManager fadeManager;

    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindWithTag("Player");
        gravityManager = GameObject.Find("GravityManager").GetComponent<GravityManager>();
        playerAvatar = player.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        playerController = player.GetComponent<PlayerController>();
        pauseButton = GameObject.Find("/Canvas/PauseButton");
        respawnPoint = player.transform.position;

        fadeCanvas = GameObject.FindGameObjectWithTag("Fade");//Canvasをみつける
        fadeManager = fadeCanvas.GetComponent<FadeManager>();
        fadeManager.fadeIn();//フェードインフラグを立てる

        //respawnpointの配列を作成
        GameObject[] RespawnObjects = GameObject.FindGameObjectsWithTag("Respawn");
        foreach (GameObject obj in RespawnObjects)
        {
            respawn_index_length++;
        }
        Debug.Log(respawn_index_length);

        RespawnObjectsList = new GameObject[respawn_index_length + 1];
        int i = 1;
        foreach (GameObject obj in RespawnObjects)
        {
            RespawnObjectsList[i] = obj;
            RespawnUpdater respawnUpdater = obj.transform.GetChild(0).gameObject.GetComponent<RespawnUpdater>();
            respawnUpdater.SetRespawnIndex(i);
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {

        /*if (respawning1)
        {
            Debug.Log("respawning1=true");
        }*/
        //if (respawn_index_current == 0) Debug.Log("index=0");
        
        if (respawn)
        {
            
            gravityManager.DestroyGF();
            //Debug.Log("respawn");
            //playerAvatar.enabled = false; // SetActive(false);
            //playerController.SetCanMove(false); //canMove = false;
            //Debug.Log("消える");
            respawn=false;

            //オブジェクトをもとの位置に戻す
            GameObject[] movableObjs = GameObject.FindGameObjectsWithTag("Movable");
            foreach (GameObject obj in movableObjs)
            { 
                //Debug.Log(obj.name);
                try
                {
                    StartCoroutine(obj.GetComponent<TotalWeight>().Respawn());
                }
                catch { }
            }

            GameObject[] movingFloors = GameObject.FindGameObjectsWithTag("Platform");
            foreach (GameObject obj in movingFloors)
            {
                //Debug.Log(obj.name);
                try
                {
                    obj.GetComponent<MoveObjectWithRoute>().Respawn();
                }
                catch { }
            }


            //SE
        }
        

        /*if (respawning1 || respawn_index_current==0)
        {
            //Debug.Log("OK");
            playerAvatar.enabled = true; // SetActive(true);
            respawning1 = false;
            
        }

        if(respawning2 || respawn_index_current == 0)
        {
            //Debug.Log("OK2");
            playerController.SetCanMove(true); //canMove = true;
            respawning2 = false;
            resAnimation = false;
        }*/

        //canActive = false;
        
    }

    public IEnumerator PlayerRespawn()
    {
        fadeManager.fadeOut();//フェードアウトフラグを立てる
        yield return new WaitForSecondsRealtime(fadeManager.fadeSpeed); ;//暗転するまで待つ
        pauseButton.SetActive(false);
        respawn = true;
        if(respawn_index_current != 0)
        {
            respawnPoint = RespawnObjectsList[respawn_index_current].transform.position + new Vector3(0, 1.99f, 0);
        }
        player.transform.position = respawnPoint;


        if(respawn_index_current != 0)
        {
            playerController.AvatarSpriteSet(false);
            fadeManager.fadeIn();//フェードインフラグを立てる
            yield return new WaitForSecondsRealtime(fadeManager.fadeSpeed);//明転するまで待つ

            GetComponent<AudioSource>().PlayOneShot(respawnSE1, 0.2f);//SE
            //yield return new WaitForSecondsRealtime(fadeManager.fadeSpeed);//明転するまで待つ
            yield return new WaitForSecondsRealtime(1); // 1秒遅延

            resAnimation = true;


            yield return new WaitUntil(() => respawning1);
            GetComponent<AudioSource>().PlayOneShot(respawnSE2, 0.2f);
            playerController.AvatarSpriteSet(true);
            yield return new WaitUntil(() => respawning2);
        }
        else
        {
            fadeManager.fadeIn();//フェードインフラグを立てる
            yield return new WaitForSecondsRealtime(fadeManager.fadeSpeed);//明転するまで待つ

        }
        yield return null;
        resAnimation = false;
        respawning1 = false;
        respawning2 = false;
    }

    public bool GetRespawning1()
    {
        return respawning1;
    }

    public void SetRespawning1(bool value)
    {
        respawning1 = value;
    }

    public bool GetRespawning2()
    {
        return respawning2;
    }

    public void SetRespawning2(bool value)
    {
        respawning2 = value;
    }

    public bool GetRespawnAnimation()
    {
        return resAnimation;
    }
    public void SetRespawnAnimation(bool value)
    {
        resAnimation = value;
    }


    public int GetRespawnIndexCurrent()
    {
        return respawn_index_current;
    }
    public void SetRespawnIndexCurrent(int index)
    {
        respawn_index_current = index;
    }

    public GameObject GetRespawnObject(int index)
    {
        return RespawnObjectsList[index];
    }
    
}
