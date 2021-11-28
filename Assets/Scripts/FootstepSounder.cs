using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSounder : MonoBehaviour {

    [SerializeField] private AudioClip[] footsteps;

    private AudioSource audioSource;
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootstep() {
        audioSource.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)], 0.75f);
    }
    
}
