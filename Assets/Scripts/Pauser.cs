using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pauser : MonoBehaviour {

    private PlayerController playerController;
    private PixelateImageEffect pixelateImageEffect;
    private bool isPaused;
    private GameObject pauseUI;
    private GameObject activeUI;

    private void Awake() {
        pauseUI = GameObject.FindWithTag("UI").transform.Find("Pause").gameObject;
        activeUI = GameObject.FindWithTag("UI").transform.Find("Active").gameObject;
        pauseUI.SetActive(false);
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        pixelateImageEffect = GameObject.FindWithTag("MainCamera").GetComponent<PixelateImageEffect>();
    }

    private void OnApplicationFocus(bool hasFocus) {
        if (enabled && !isPaused && !hasFocus) {
            Pause();
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P) && !isPaused) {
            Pause();
        } else if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) || Input.GetKeyDown(KeyCode.P) && isPaused) {
            Unpause();
        }
    }
    
    private void Pause() {
        Cursor.lockState = CursorLockMode.None;
        isPaused = true;
        playerController.enabled = false;
        pixelateImageEffect.enabled = true;
        pauseUI.SetActive(true);
        SetActiveUI(false);
    }

    private void Unpause() {
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
        playerController.enabled = true;
        pixelateImageEffect.enabled = false;
        pauseUI.SetActive(false);
        SetActiveUI(true);
    }

    private void SetActiveUI(bool isActive) {
        activeUI.transform.Find("Reticle").gameObject.SetActive(isActive);
        activeUI.transform.Find("Text").GetComponent<TMP_Text>().enabled = isActive;
        activeUI.transform.Find("Title").gameObject.SetActive(isActive);
    }

    public bool IsPaused() {
        return isPaused;
    }
    
}
