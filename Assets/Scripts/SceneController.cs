using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;//�ǉ�
using UnityEngine.SceneManagement;//�ǉ�

public class SceneController : MonoBehaviour
{
    public GameObject fade;//�C���X�y�N�^����Prefab������Canvas������
    public GameObject fadeCanvas;//���삷��Canvas�A�^�O�ŒT��

    void Start()
    {
        if (!FadeManager.isFadeInstance)//isFadeInstance�͌�ŗp�ӂ���
        {
            Instantiate(fade);
        }
        //fadeCanvas = GameObject.FindGameObjectWithTag("Fade");
        Invoke("findFadeObject", 0.02f);//�N�����p��Canvas�̏�����������Ƒ҂�
        //Invoke("findFadeObject", 0.0f);//�N�����p��Canvas�̏�����������Ƒ҂�

        //11/13 https://qiita.com/gino_gino/items/f4b0277fb4e260461a3d�@�̒ʂ�ɍ�������G���[�~�܂炸�B
        //prefab������FadeCanbas�����łɃV�[�����ɓ���Ă��邱�Ƃ�������������Ȃ�
    }

    void findFadeObject()
    {
        fadeCanvas = GameObject.FindGameObjectWithTag("Fade");//Canvas���݂���
        fadeCanvas.GetComponent<FadeManager>().FadeIn();//�t�F�[�h�C���t���O�𗧂Ă�
    }

    public async void SceneChange(string sceneName)//�{�^������ȂǂŌĂяo��
    {
        fadeCanvas.GetComponent<FadeManager>().FadeOut();//�t�F�[�h�A�E�g�t���O�𗧂Ă�
        await Task.Delay(200);//�Ó]����܂ő҂�
        SceneManager.LoadScene(sceneName);//�V�[���`�F���W
    }
}

