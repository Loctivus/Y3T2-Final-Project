using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    bool menuOpen;
    public GameObject pauseMenuObj;
    public BoolRef gamePaused;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            
        }
    }

    void TogglePauseMenu()
    {
        if (menuOpen)
        {
            pauseMenuObj.SetActive(false);
            
        }
    }
}
