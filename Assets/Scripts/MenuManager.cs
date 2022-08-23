using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public static bool isRestart = false;

    public bool gameState;
    public GameObject objMenuElement;
    public GameObject objGameOverMenuElement;
    public PlayerManager playerManager;

    private void Start()
    {
        instance = this;
        gameState = false;

        if(isRestart)
        {
            startGame();
        }
        else
        {
            enableMainMenu();
        }
        
    }

    public void startGame()
    {
        gameState = true;
        // objMenuElement.SetActive(false);
        disableAll();
        playerManager.gameStart();
    }

    public void reStartGame()
    {
        isRestart = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void enableMainMenu()
    {
        objMenuElement.SetActive(true);
        objGameOverMenuElement.SetActive(false);
    }

    public void enableGameOverMenu()
    {
        objMenuElement.SetActive(false);
        objGameOverMenuElement.SetActive(true);
    }

    void disableAll()
    {
        objMenuElement.SetActive(false);
        objGameOverMenuElement.SetActive(false);
    }
}
