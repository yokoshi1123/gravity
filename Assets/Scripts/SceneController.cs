using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;//追加
using UnityEngine.SceneManagement;//追加

public class SceneController : MonoBehaviour
{
    public GameObject fade;//インスペクタからPrefab化したCanvasを入れる
    public GameObject fadeCanvas;//操作するCanvas、タグで探す

    void Start()
    {
        if (!FadeManager.isFadeInstance)//isFadeInstanceは後で用意する
        {
            Instantiate(fade);
        }
        //fadeCanvas = GameObject.FindGameObjectWithTag("Fade");
        Invoke("findFadeObject", 0.02f);//起動時用にCanvasの召喚をちょっと待つ
        //Invoke("findFadeObject", 0.0f);//起動時用にCanvasの召喚をちょっと待つ

        //11/13 https://qiita.com/gino_gino/items/f4b0277fb4e260461a3d　の通りに作ったがエラー止まらず。
        //prefab化したFadeCanbasをすでにシーン内に入れていることが原因かもしれない
    }

    void findFadeObject()
    {
        fadeCanvas = GameObject.FindGameObjectWithTag("Fade");//Canvasをみつける
        fadeCanvas.GetComponent<FadeManager>().FadeIn();//フェードインフラグを立てる
    }

    public async void SceneChange(string sceneName)//ボタン操作などで呼び出す
    {
        fadeCanvas.GetComponent<FadeManager>().FadeOut();//フェードアウトフラグを立てる
        await Task.Delay(200);//暗転するまで待つ
        SceneManager.LoadScene(sceneName);//シーンチェンジ
    }
}

