using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;          //  신 변경

public class TitleScene : MonoBehaviour
{
    #region Variables
    //private Fade fade;
    private bool inputState = false;        // 입력받을 수 있는 시점 제어
    #endregion Variables

    #region Functions
    // Start is called before the first frame update
    void Start()
    { 
        Invoke("GameStart", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        gameInput();
    }

    void GameStart()
    {
        inputState = true;
    }
    void nextScene()
    {
        SceneManager.LoadSceneAsync("Select_Character");
    }
    void gameInput()
    {
        if (!inputState)
            return;

        if(Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            LoadingSceneController.Instance.LoadScene("Select_Character");
        }
    }
    #endregion Functions
}
