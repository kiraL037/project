using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject SavePanel;
    public void Start()
    {
        SceneManager.LoadScene(1);
    }
    public void OpenSave()
    {
        SavePanel.SetActive(true);
    }
    public void CloseSave()
    {
        SavePanel.SetActive(false);
    }
    public void Exit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

}
