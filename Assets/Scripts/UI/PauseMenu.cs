using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
                PauseGame();
        }
    }
    public void PauseGame()
    {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
    }
}
