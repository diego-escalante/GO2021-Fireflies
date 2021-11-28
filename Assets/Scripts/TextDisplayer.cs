using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextDisplayer : MonoBehaviour {
    
    [SerializeField] private float fadeTime = 3f;

    private TMP_Text text;
    private Color color;
    private Pauser pauser;
    private void Awake() {
        text = GetComponent<TMP_Text>();
        text.text = "";
        color = text.color;
        text.color = Color.clear;
    }

    private void Start() {
        pauser = GameObject.FindWithTag("GameController").GetComponent<Pauser>();
        StartCoroutine(RunTutorial());
    }

    private IEnumerator RunTutorial() {
        
        // Wait for player to click.
        StartCoroutine(FadeInText(fadeTime, "Click to start."));
        while (pauser.IsPaused() || !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1)) {
            yield return null;
        }
        
        // Enable the player controller.
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = true;
        GameObject.FindWithTag("UI").transform.Find("Fader").GetComponent<Fader>().FadeIn();
        StartCoroutine(FadeOutText(fadeTime));
        yield return new WaitForSeconds(fadeTime);
        
        // Wait for player to look.
        StartCoroutine(FadeInText(fadeTime, "Move the mouse to look around."));
        yield return new WaitForSeconds(fadeTime);
        while (pauser.IsPaused() || Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0) {
            yield return null;
        }
        StartCoroutine(FadeOutText(fadeTime));
        yield return new WaitForSeconds(fadeTime);
        
        // Wait for player to move.
        StartCoroutine(FadeInText(fadeTime, "Use WASD to move."));
        yield return new WaitForSeconds(fadeTime);
        while (pauser.IsPaused() || Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) {
            yield return null;
        }
        StartCoroutine(FadeOutText(fadeTime));
        yield return new WaitForSeconds(fadeTime);
        
        // Wait for player to capture fireflies.
        StartCoroutine(FadeInText(fadeTime, "Hold the left mouse button while looking\nat a nearby firefly to capture it."));
        BottleController bottleController = GameObject.FindWithTag("Bottle").GetComponent<BottleController>();
        int startingFireflies = bottleController.GetNumberOfFliesInBottle();
        while (bottleController.GetNumberOfFliesInBottle() <= startingFireflies) {
            if (bottleController.GetNumberOfFliesInBottle() < startingFireflies) {
                startingFireflies = bottleController.GetNumberOfFliesInBottle();
            }
            yield return null;
        }
        StartCoroutine(FadeOutText(fadeTime));
        yield return new WaitForSeconds(fadeTime);
        
        // Wait for player to release fireflies.
        StartCoroutine(FadeInText(fadeTime, "Hold the right mouse button to\nrelease fireflies from your lantern."));
        startingFireflies = bottleController.GetNumberOfFliesInBottle();
        while (bottleController.GetNumberOfFliesInBottle() != 0 && bottleController.GetNumberOfFliesInBottle() >= startingFireflies) {
            if (bottleController.GetNumberOfFliesInBottle() > startingFireflies) {
                startingFireflies = bottleController.GetNumberOfFliesInBottle();
            }
            yield return null;
        }
        StartCoroutine(FadeOutText(fadeTime));
        yield return new WaitForSeconds(fadeTime);
        
        // Tell the player it can press Escape.
        StartCoroutine(FadeInText(fadeTime, "Press 'P' at any time to\npause the game."));
        yield return new WaitForSeconds(fadeTime * 2);
        StartCoroutine(FadeOutText(fadeTime));
    }

    private IEnumerator FadeInText(float duration, string message) {
        text.text = message;
        for (float currentTime = 0; currentTime < duration; currentTime += Time.deltaTime) {
            text.color = Color.Lerp(Color.clear, color, currentTime / duration);
            yield return null;
        }
        text.color = color;
    }
    
    private IEnumerator FadeOutText(float duration) {
        Color current = text.color;
        for (float currentTime = 0; currentTime < duration; currentTime += Time.deltaTime) {
            text.color = Color.Lerp(current, Color.clear, currentTime / duration);
            yield return null;
        }
        text.color = Color.clear;
    }
}
