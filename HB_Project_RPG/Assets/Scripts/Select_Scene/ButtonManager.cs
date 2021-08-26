using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    #region Variables
    private CamController camController;
    public GameObject target;
    public GameObject[] targetChild;
    public GameObject[] lightChild;

    public int charIndex = 0;
    #endregion Variables

    #region Functions
    // Start is called before the first frame update
    void Start()
    {
        camController = FindObjectOfType<CamController>();
        target = GameObject.Find("Targets");

        for (int i = 0; i < target.transform.childCount; i++)
        {
            targetChild[i] = target.transform.GetChild(i).gameObject;
            lightChild[i] = GameObject.Find("Lights").transform.GetChild(i + 10).gameObject;
        }
    }

    public void nextButton()
    {
        lightChild[charIndex].SetActive(false);
        charIndex = (charIndex + 1) % target.transform.childCount;

        camController.MoveToSpot(targetChild[charIndex]);
        lightChild[charIndex].SetActive(true);

        Debug.Log("charIndex : " + charIndex);
    }

    public void previousButton()
    {
        lightChild[charIndex].SetActive(false);
        charIndex--;
        if (charIndex < 0)
            charIndex += target.transform.childCount;

        camController.MoveToSpot(targetChild[charIndex]);
        lightChild[charIndex].SetActive(true);

        Debug.Log("charIndex : " + charIndex);
    }

    public void selectButton()
    {
        switch (charIndex)
        {
            case 0:
                DataManager.instance.currentCharacter = Characters.Warrior;
                break;
            case 1:
                DataManager.instance.currentCharacter = Characters.Archer;
                break;
            case 2:
                DataManager.instance.currentCharacter = Characters.Wizard;
                break;
        }
        LoadingSceneController.Instance.LoadScene("InDungeon_ver_2");

        Debug.Log("선택된 캐릭터 : " + DataManager.instance.currentCharacter);
    }
    public void backButton()
    {
        LoadingSceneController.Instance.LoadScene("Title");
    }
    #endregion Functions
}
