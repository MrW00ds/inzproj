using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject menuCanvas;
    [SerializeField]
    private GameObject timesCanvas;
    public void StartDemo()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void ExitGame()
    {
        Debug.Log("exit");
        Application.Quit();
    }

    public void changeCanvas(int menuSelect)
    {
        Debug.Log("scene change: " + menuSelect);
        switch (menuSelect) { 
        case 1:
                menuCanvas.SetActive(false);
                timesCanvas.SetActive(true);
                break;
        default:
                menuCanvas.SetActive(true);
                timesCanvas.SetActive(false);
                break;

        }
    }

}
