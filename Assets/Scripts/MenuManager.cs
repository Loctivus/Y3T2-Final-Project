using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public FloatVariable mouseSens;

    [Header("Debug Menu")]
    bool dbMenuOpen;
    public GameObject dbMenuObj;

    [Header("Pause Menu")]
    bool pauseMenuOpen;
    public GameObject pauseMenuObj;
    public TMP_Text mouseSensSliderText;
    
    [Header("Game Paused SO")]
    public BoolVariable gamePaused;

    void Awake()
    {
        dbMenuObj.SetActive(false);
        pauseMenuObj.SetActive(false);
        gamePaused.value = false;
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.LeftControl))
        {
            if (dbMenuOpen)
            {
                CloseDebugMenu();
            }
            else
            {
                OpenDebugMenu();
            }
        }
           
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuOpen)
            {
                ClosePauseMenu();
            }
            else
            {
                OpenPauseMenu();
            }
        }    
    }




    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void SetMouseSens(System.Single sens)
    {
        mouseSens.value = sens * 10;
        mouseSensSliderText.text = sens.ToString();
    }

    void UpdateSensText()
    {
        mouseSensSliderText.text = (mouseSens.value / 10).ToString();
    }

    public void OpenPauseMenu()
    {
        gamePaused.value = true;
        UpdateSensText();
        Time.timeScale = 0f;
        pauseMenuObj.SetActive(true);
        pauseMenuOpen = true;
        Cursor.lockState = CursorLockMode.Confined;
        
    }

    public void ClosePauseMenu()
    {
        gamePaused.value = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuObj.SetActive(false);
        pauseMenuOpen = false;
    }

    public void OpenDebugMenu()
    {
        gamePaused.value = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        dbMenuOpen = true;
        dbMenuObj.SetActive(true);
    }

    public void CloseDebugMenu()
    {
        if (!pauseMenuOpen)
        {
            gamePaused.value = false;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            dbMenuObj.SetActive(false);
            dbMenuOpen = false;
        }
    }
}
