using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlickerer : MonoBehaviour {

    [SerializeField] private float minFlickerUpdateTime = 0.1f;
    [SerializeField] private float maxFlickerUpdateTime = 0.2f;
    [SerializeField] private float minIntensity = 0.25f;
    [SerializeField] private float maxIntensity = 0.3f;
    
    private Light lanternLight;
    
    private void Awake() {
        lanternLight = GetComponent<Light>();
    }

    private void Start() {
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker() {
        yield return new WaitForSeconds(Random.Range(minFlickerUpdateTime, maxFlickerUpdateTime));
        lanternLight.intensity = Random.Range(minIntensity, maxIntensity);
        StartCoroutine(Flicker());
    }
}
