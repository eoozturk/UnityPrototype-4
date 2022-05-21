using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Splash : MonoBehaviour
{
    [SerializeField] TMP_InputField playerName;
 
    private string nameText;

    public void SetPlayerName()
    {

        nameText = playerName.text;
        
    }

    public void StartGame()
    {
        SetPlayerName();
        DataManager.instance.playerName = nameText;
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR

            EditorApplication.ExitPlaymode();

        #else
        
            Application.Quit();

        #endif
    }

}
