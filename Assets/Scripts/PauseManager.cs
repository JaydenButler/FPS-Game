using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    //TODO: https://stackoverflow.com/questions/16168596/find-inactive-gameobject-by-tag-in-unity3d
    //Get the object and turn the canvas on/off instead of disabling the object!!!
    private GameObject _pauseMenu;
    private GameObject _settingsMenu;
    private void Start()
    {
        _pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        _settingsMenu = GameObject.FindGameObjectWithTag("SettingsMenu");
    }

    public void Resume()
    {
        _pauseMenu.SetActive(false);
        FirstPersonController.Paused = false;
    }

    public void OpenSettings()
    {
        _pauseMenu.SetActive(false);
        _settingsMenu.SetActive(true);
    }

    public void SensitivityChanged(float sens)
    {
        FirstPersonController.MouseSensitivity = sens;
    }
    public void CloseSettings()
    {
        _settingsMenu.SetActive(false);
        _pauseMenu.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FirstPersonController.Paused = true;
            _settingsMenu.gameObject.SetActive(true);
        }
    }
}
