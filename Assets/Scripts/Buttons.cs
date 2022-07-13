using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public GameObject PauseState;
    public GameObject DeathState;
    public bool Death;
    public GameObject Player;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && Player.GetComponent<ResetSetup>().Resets < 5)
        {
            PauseButton();
        }
        if(Death == true)
        {
            DeathButton();
        }
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Game World");
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void OptionsButton()
    {

    }
    void PauseButton()
    {
        if(GameObject.Find("FPSPlayer").GetComponent<Movement>().PauseState == true)
        {
            GameObject.Find("FPSPlayer").GetComponent<Movement>().PauseState = false;
            PauseState.transform.localScale = new Vector3(0, 0, 0);
        }
        else if (GameObject.Find("FPSPlayer").GetComponent<Movement>().PauseState == false)
        {
            GameObject.Find("FPSPlayer").GetComponent<Movement>().PauseState = true;
            PauseState.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    void DeathButton()
    {
        DeathState.transform.localScale = new Vector3(1, 1, 1);
        GameObject.Find("Play Button").transform.localScale = new Vector3(0, 0, 0);
    }
    public void ResetUI()
    {
        GameObject.Find("Play Button").transform.localScale = new Vector3(1, 1, 1);
        PauseState.transform.localScale = new Vector3(0, 0, 0);
        DeathState.transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("FPSPlayer").GetComponent<Movement>().PauseState = false;
    }
    public void ExitToMenuButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
