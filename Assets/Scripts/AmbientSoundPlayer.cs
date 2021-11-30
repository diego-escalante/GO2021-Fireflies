using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbientSoundPlayer : MonoBehaviour {
    [SerializeField] private float minWaitTime;
    [SerializeField] private float maxWaitTime;

    private AudioSource audioSource;
    private float currentWaitTime;
    
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        currentWaitTime = Random.Range(minWaitTime, maxWaitTime);
    }
    private void Update() {
        if (audioSource.isPlaying) {
            return;
        }

        if (currentWaitTime <= 0) {
            audioSource.Play();
            currentWaitTime = Random.Range(minWaitTime, maxWaitTime);
        }

        currentWaitTime -= Time.deltaTime;
    }
    
}
