using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectScene : MonoBehaviour
{
    #region Variables
    private ButtonManager button;
    private CamController camController;
    #endregion Variables

    #region Functions
    // Start is called before the first frame update
    void Start()
    {
        camController = FindObjectOfType<CamController>();
        button = FindObjectOfType<ButtonManager>();

        if (camController != null)
            camController.MoveToSpot(button.targetChild[0]);
    }
    #endregion Functions
}
