using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    #region Variables
    bool dbMenuOpen;
    public GameObject dbMenuObj;
    public BoolVariable gamePaused;
    #endregion

    void Start()
    {
        dbMenuObj.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.LeftControl) && dbMenuOpen == false)
        {
            OpenDebugMenu();
        }
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
        
        gamePaused.value = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        dbMenuObj.SetActive(false);
        dbMenuOpen = false;
    }

    

}
