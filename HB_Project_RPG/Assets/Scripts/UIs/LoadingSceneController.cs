using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoadingSceneController : MonoBehaviour
{
    #region Singleton
    private static LoadingSceneController instance;

    public static LoadingSceneController Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<LoadingSceneController>();
                if (obj != null)
                    instance = obj;
                else
                    instance = Create();
            }

            return instance;
        }
        set
        {

        }
    }

    private static LoadingSceneController Create()
    {
        return Instantiate(Resources.Load<LoadingSceneController>("Prefabs/LoadingScene"));
    }

    #endregion Singleton

    #region Variables
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Image progressBar;
    [SerializeField]
    private Image[] rndImage;

    private string loadSceneName;
    private int imgIndex = 4;

    #endregion Variables

    #region Functions
    public void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += onSceneLoaded;
        loadSceneName = sceneName;
        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        RandomImage();
        progressBar.fillAmount = 0;
        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        // 로딩이 끝나도 화면이 넘어가지 않도록
        op.allowSceneActivation = false;

        float timer = 0f;

        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
                progressBar.fillAmount = op.progress;
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0f, 1f, timer);

                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private void onSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == loadSceneName)
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= onSceneLoaded;
        }
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;
        while(timer <= 1.5f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (!isFadeIn)
            gameObject.SetActive(false);
    }

    private void RandomImage()
    {
        for (int i = 0; i < imgIndex; i++)
            rndImage[i].gameObject.SetActive(false);

        int rnd = UnityEngine.Random.Range(0, 4);
        rndImage[rnd].gameObject.SetActive(true);

        Debug.Log("Random number :" + rnd);
    }
    #endregion Functions
}