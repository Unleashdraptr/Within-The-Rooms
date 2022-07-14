using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Buttons : MonoBehaviour
{
    public GameObject PauseState;
    public GameObject DeathState;
    public bool Death;
    public GameObject Player;
    public AudioSource PauseSFX;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && Player.GetComponent<ResetSetup>().Resets < 5)
        {
            PauseButton();
            Cursor.visible = true;
            PauseSFX.Play();
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
    void PauseButton()
    {
        if(GameObject.Find("FPSPlayer").GetComponent<Movement>().PauseState == true)
        {
            Cursor.visible = true;
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
        GameObject.Find("Death Text").GetComponent<Text>().text = "You Died!";
        PauseState.transform.localScale = new Vector3(0, 0, 0);
        DeathState.transform.localScale = new Vector3(0, 0, 0);
        GameObject.Find("FPSPlayer").GetComponent<Movement>().PauseState = false;
    }
    public void ExitToMenuButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
