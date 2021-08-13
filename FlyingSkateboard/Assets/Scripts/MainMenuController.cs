using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private void Start()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene("Gameplay");        
    }

    public void Quit()
    {
        Application.Quit();
    }
}
