using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour
{
    public GameObject pausePanel;
    public Button pauseButton;
    public Button soundButton;
    public Sprite[] pauseSprites;
    public Sprite[] soundSprites;

    private void Start()
    {
        
    }

    public void Pause()
    {
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;

        if (Time.timeScale == 1)
        {
            pausePanel.SetActive(false);
            pauseButton.image.sprite = pauseSprites[0];
        }
        else if (Time.timeScale == 0)
        {
            pausePanel.SetActive(true);
            pauseButton.image.sprite = pauseSprites[1];
        }            
    }

    public void Sound()
    {
        if (MusicController.instance.audioSource.isPlaying)
        {
            MusicController.instance.PlayMusic(false);
            soundButton.image.sprite = soundSprites[1];
        }
        else
        {
            MusicController.instance.PlayMusic(true);
            soundButton.image.sprite = soundSprites[0];
        }
    }
}
