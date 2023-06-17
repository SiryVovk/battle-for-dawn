using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackMenu : MonoBehaviour
{
    [SerializeField] private GameObject endTurnButton;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject optionButton;
    [SerializeField] private GameObject menuButton;

    [SerializeField] private GameObject optionsCanvas;
    [SerializeField] private GameObject backImage;

    public static Action endTurn;

    public void EndTurn()
    {
        endTurn?.Invoke();
    }

    public void Back()
    {
        endTurnButton.SetActive(false);
        backButton.SetActive(false);
        resumeButton.SetActive(true);
        optionButton.SetActive(true);
        menuButton.SetActive(true);
        backImage.SetActive(true);
    }

    public void Resume()
    {
        endTurnButton.SetActive(true);
        backButton.SetActive(true);
        resumeButton.SetActive(false);
        optionButton.SetActive(false);
        menuButton.SetActive(false);
        backImage.SetActive(false);
    }

    public void Options()
    {
        optionsCanvas.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void MainMenu()
    {
        Destroy(GameObject.FindGameObjectWithTag("Audio"));
        this.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }
}
