using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public float chosenVolume;

    public Slider audioSlider;

    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject gameUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        gameUI.SetActive(true);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    
    public void Pause()
    {
        audioMixer.GetFloat("MainVolume", out chosenVolume);
        audioSlider.value = chosenVolume;
        pauseMenuUI.SetActive(true);
        gameUI.SetActive(false);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MainVolume", volume);
        chosenVolume = volume;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
