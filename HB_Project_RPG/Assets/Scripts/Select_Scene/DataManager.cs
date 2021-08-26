using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Characters
{
    Warrior, Archer, Wizard
}

public class DataManager : MonoBehaviour
{
    #region Singleton
    public static DataManager instance;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            return;

        DontDestroyOnLoad(gameObject);
    }
    #endregion singleton

    public Characters currentCharacter;
}
