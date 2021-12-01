using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplayer : MonoBehaviour {
    
    [SerializeField] private float fadeTime = 3f;

    private TMP_Text text;
    private Color color;
    private Pauser pauser;
    private Coroutine tutorialCo;

    private RawImage reticle;
    private Color reticleColor;
    private void Awake() {
        text = GetComponent<TMP_Text>();
        text.text = "";
        color = text.color;
        text.color = Color.clear;
        reticle = transform.parent.Find("Reticle").GetComponent<RawImage>();
        reticleColor = reticle.color;
        reticle.color = Color.clear;
    }

    private void Start() {
        pauser = GameObject.FindWithTag("GameController").GetComponent<Pauser>();
        tutorialCo = StartCoroutine(RunTutorial());
        StartCoroutine(Win());
    }

    private IEnumerator RunTutorial() {
        
        // Wait for player to click.
        StartCoroutine(FadeInText(fadeTime, GameObject.FindWithTag("Title").GetComponent<TMP_Text>()));
        yield return new WaitForSeconds(1);
        StartCoroutine(FadeInText(fadeTime, "Headphones and fullscreen recommended.\nClick to start."));
        while (pauser.IsPaused() || !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1)) {
            yield return null;
        }
        
        // Enable the player controller.
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = true;
        GameObject.FindWithTag("UI").transform.Find("Fader").GetComponent<Fader>().FadeIn();
        StartCoroutine(FadeInReticle(fadeTime));
        StartCoroutine(FadeOutText(fadeTime));
        StartCoroutine(FadeOutText(fadeTime, GameObject.FindWithTag("Title").GetComponent<TMP_Text>()));
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
        GameObject.FindWithTag("GameController").GetComponent<Pauser>().enabled = true;
        yield return new WaitForSeconds(fadeTime * 2);
        StartCoroutine(FadeOutText(fadeTime));
        
        
    }

    private IEnumerator Win() {
        // Wait for the player to go past the door.
        Transform player = GameObject.FindWithTag("Player").transform;
        while (player.position.z < 16) {
            yield return null;
        }
        
        // Player wins!
        StopCoroutine(tutorialCo);
        StartCoroutine(FadeOutText(0.5f));
        GameObject.FindWithTag("UI").transform.Find("Fader").GetComponent<Fader>().FadeOut();
        StartCoroutine(FadeOutReticle(fadeTime));
        yield return new WaitForSeconds(4);
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().enabled = false;
        GameObject.FindWithTag("MainCamera").GetComponent<Animator>().enabled = false;
        StartCoroutine(FadeInText(fadeTime, "You made it out!\nThanks for playing."));
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
    
    private IEnumerator FadeInText(float duration, TMP_Text obj) {
        for (float currentTime = 0; currentTime < duration; currentTime += Time.deltaTime) {
            obj.color = Color.Lerp(Color.clear, color, currentTime / duration);
            yield return null;
        }
        obj.color = color;
    }
    
    private IEnumerator FadeOutText(float duration, TMP_Text obj) {
        Color current = obj.color;
        for (float currentTime = 0; currentTime < duration; currentTime += Time.deltaTime) {
            obj.color = Color.Lerp(current, Color.clear, currentTime / duration);
            yield return null;
        }
        obj.color = Color.clear;
    }
    
    private IEnumerator FadeInReticle(float duration) {
        for (float currentTime = 0; currentTime < duration; currentTime += Time.deltaTime) {
            reticle.color = Color.Lerp(Color.clear, reticleColor, currentTime / duration);
            yield return null;
        }
        reticle.color = reticleColor;
    }
    
    private IEnumerator FadeOutReticle(float duration) {
        for (float currentTime = 0; currentTime < duration; currentTime += Time.deltaTime) {
            reticle.color = Color.Lerp(reticleColor, Color.clear, currentTime / duration);
            yield return null;
        }
        reticle.color = Color.clear;
    }
}
