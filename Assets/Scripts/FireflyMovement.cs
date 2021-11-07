using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyMovement : MonoBehaviour {
    
    [SerializeField] private Vector3 moveRange = new Vector3(3, 1, 3);
    [SerializeField] private Vector3 moveBaseFrequency = new Vector3(1, 0.2f, 1);
    [SerializeField] private int noiseOctaves = 2;
    [SerializeField] private float amplitudeMultiplier = 0.25f;
    [SerializeField] private float frequencyMultiplier = 2f;
    
    [SerializeField] private float minTimeLit, maxTimeLit, minTimeUnlit, maxTimeUnlit, transitionTime;
    [SerializeField] private Color color = new Color(0.8f, 1f, 0, 1);

    private Vector3[] noiseOffsets;
    private Vector3 origin;
    private bool isLit;
    private float stateTimeLeft;
    private float transitionTimeLeft;
    private Material material;
    private Color clearColor;
    private Light light;
    private float lightIntensity;

    private void Awake() {
        origin = transform.position;
        material = GetComponent<Renderer>().material;
        light = GetComponent<Light>();
        lightIntensity = light.intensity;
        clearColor = color;
        clearColor.a = 0;

        noiseOffsets = new Vector3[noiseOctaves];
        for (int i = 0; i < noiseOctaves; i++) {
            noiseOffsets[i] = new Vector3(Random.Range(0, 1000f), Random.Range(0, 1000f), Random.Range(0, 1000f));
        }
    }
    
    private void Update() {
        if (transitionTimeLeft > 0) {
            transitionTimeLeft -= Time.deltaTime;
            float t = (transitionTime - transitionTimeLeft) / transitionTime;
            if (isLit) {
                material.SetColor("_Color",Color.Lerp(clearColor, color, t));
                light.intensity = Mathf.Lerp(0, lightIntensity, t);
            } else {
                material.SetColor("_Color",Color.Lerp(color, clearColor, t));
                light.intensity = Mathf.Lerp(lightIntensity, 0, t);
            }

        } else {
            stateTimeLeft -= Time.deltaTime;
            if (stateTimeLeft <= 0) {
                isLit = !isLit;
                stateTimeLeft = isLit ? Random.Range(minTimeLit, maxTimeLit) : Random.Range(minTimeUnlit, maxTimeUnlit);
                transitionTimeLeft = transitionTime;
            }
        }
        
        if (isLit || transitionTimeLeft > 0) {
            UpdatePosition();
        }
        
        
    }

    private void UpdatePosition() {
        Vector3 newPos = origin;
        float t = Time.time;

        for (int d = 0; d < 3; d++) {
            float accumulatedNoise = 0;
            float accumulatedAmplitudes = 0;
            float amplitude = 1;
            float frequency = 1;
            for (int i = 0; i < noiseOctaves; i++) {
                accumulatedNoise += amplitude * Mathf.PerlinNoise(t * moveBaseFrequency[d] * frequency + noiseOffsets[i][d], noiseOffsets[i][d]);
                accumulatedAmplitudes += amplitude;
                amplitude *= amplitudeMultiplier;
                frequency *= frequencyMultiplier;
            }
            newPos[d] += (accumulatedNoise / accumulatedAmplitudes - 0.5f) * moveRange[d];
        }
        transform.position = newPos;
    }
    
    
    
}
