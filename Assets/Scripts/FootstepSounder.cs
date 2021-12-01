using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSounder : MonoBehaviour {

    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private AudioClip[] sucks;
    [SerializeField] private AudioClip[] releases;

    private AudioSource audioSource;
    
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        EventManager.StartListening(EventManager.Event.CaptureFly, PlayCaptureFly);
        EventManager.StartListening(EventManager.Event.ReleaseFly, PlayReleaseFly);
    }
    
    private void OnDisable() {
        EventManager.StopListening(EventManager.Event.CaptureFly, PlayCaptureFly);
        EventManager.StopListening(EventManager.Event.ReleaseFly, PlayReleaseFly);
    }

    public void PlayFootstep() {
        audioSource.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)], 0.75f);
    }

    private void PlayCaptureFly() {
        audioSource.PlayOneShot(sucks[Random.Range(0, sucks.Length)], 0.4f);
    }
    
    private void PlayReleaseFly() {
        audioSource.PlayOneShot(releases[Random.Range(0, releases.Length)], 0.75f);
    }
    
}
