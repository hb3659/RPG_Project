using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtonManager : MonoBehaviour
{
    public void NewGame()
    {
        LoadingSceneController.Instance.LoadScene("InDungeon_TopDown");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
