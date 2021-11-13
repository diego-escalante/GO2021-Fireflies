using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyMovement : MonoBehaviour {
    
    [SerializeField] private Vector3 moveRange = new Vector3(3, 1, 3);
    [SerializeField] private Vector3 moveBaseFrequency = new Vector3(1, 0.2f, 1);
    [SerializeField] private int noiseOctaves = 2;
    [SerializeField] private float amplitudeMultiplier = 0.25f;
    [SerializeField] private float frequencyMultiplier = 2f;
    [SerializeField] private float moveRangeRecoveryTime = 1f;
    [SerializeField] private LayerMask solidLayerMask;
    
    private Vector3[] noiseOffsets;
    private Vector3 origin;
    private FireflyLightBehavior lightBehavior;
    private float moveRangeRecoveryTimeLeft;
    private Vector3 currentMoveRange;
    private Transform container;
    private Coroutine travelCo;

    private void Awake() {
        lightBehavior = GetComponent<FireflyLightBehavior>();
        
        origin = transform.position;
        currentMoveRange = moveRange;

        noiseOffsets = new Vector3[noiseOctaves];
        for (int i = 0; i < noiseOctaves; i++) {
            noiseOffsets[i] = new Vector3(Random.Range(0, 1000f), Random.Range(0, 1000f), Random.Range(0, 1000f));
        }
    }
    
    private void Update() {
        if (moveRangeRecoveryTimeLeft > 0) {
            moveRangeRecoveryTimeLeft -= Time.deltaTime;
            currentMoveRange = Vector3.Lerp(Vector3.zero, moveRange, (moveRangeRecoveryTime - moveRangeRecoveryTimeLeft) / moveRangeRecoveryTime);
        }

        if (container != null) {
            origin = container.position;
        }
        
        UpdatePosition();
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
            newPos[d] += (accumulatedNoise / accumulatedAmplitudes - 0.5f) * currentMoveRange[d];
        }
        transform.position = newPos;
    }
    
    public void Send(Vector3 startingPoint, Vector3 forward, float distance, float speed) {
        float radius = 0;
        for (int i = 0; i < 3; i++) {
            if (radius < moveRange[i]) {
                radius = moveRange[i];
            }
        }
        RaycastHit hit;
        Vector3 target;
        if (Physics.SphereCast(startingPoint, radius, forward, out hit, distance, solidLayerMask)) {
            target = hit.point;
        } else {
            target = startingPoint + forward * distance;
        }
        

        RemoveFromContainer(new Vector3(3,1,3));
        if (travelCo != null) {
            StopCoroutine(travelCo);
        }
        travelCo = StartCoroutine(Travel(target, speed));
    }

    public IEnumerator Travel(Vector3 target, float speed) {
        while (Vector3.Distance(transform.position, target) > 0.05f) {
            MoveTowards(target, speed);
            yield return null;
        }
    }

    public void Suck(Vector3 target, float speed) {
        if (travelCo != null) {
            StopCoroutine(travelCo);
        }
        MoveTowards(target, speed);
    }

    private void MoveTowards(Vector3 target, float speed) {
        // Sets the origin to the current fly position and moves it closer to the target.
        origin = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
        lightBehavior.SetLitState(true);
        currentMoveRange = Vector3.zero;
        moveRangeRecoveryTimeLeft = moveRangeRecoveryTime;
    }

    public void PutInContainer(Transform container, Vector3 moveRange) {
        this.container = container;
        this.moveRange = moveRange;
    }

    public void RemoveFromContainer(Vector3 moveRange) {
        container = null;
        this.moveRange = moveRange;
    }

    public bool IsInContainer() {
        return container != null;
    }

}
