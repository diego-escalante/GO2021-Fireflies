using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Light), typeof(Renderer))]
public class FireflyLightBehavior : MonoBehaviour {

    [SerializeField] private float minTimeLit = 0.5f; 
    [SerializeField] private float maxTimeLit = 1f; 
    [SerializeField] private float minTimeUnlit = 2f; 
    [SerializeField] private float maxTimeUnlit = 5f; 
    [SerializeField] private float transitionTime = 0.2f;
    
    private Color fireflyColor;         // The starting color of the firefly.
    private float lightIntensity;       // The starting light intensity.
    private Coroutine co;               // The current coroutine handling lighting behavior.
    
    // Components
    private Material material;
    private Light pointLight;
    
    // Properties
    /**
     * The amount of "litness," from 0 (unlit) to 1 (lit).
     */
    public float NormalizedLitness { get; private set; }

    private void Awake() {
        material = GetComponent<Renderer>().material;
        fireflyColor = material.GetColor("_Color");
        
        pointLight = GetComponent<Light>();
        pointLight.color = fireflyColor;
        lightIntensity = pointLight.intensity;
    }

    private void OnEnable() {
        // Start the firely at a random lit state.
        SetLitState(Random.value < 0.5f);
    }

    private void OnDisable() {
        // Keep the firefly it its default lit state indefinitely.
        if (gameObject.activeInHierarchy) {
            SetLitState(true);
        }
    }

    private IEnumerator UpdateLitState(bool isLit) {
        // Transition to the desired state if needed.
        float timeLeft = transitionTime * (isLit ? 1 - NormalizedLitness : NormalizedLitness);
        while (timeLeft >= 0) {
            timeLeft -= Time.deltaTime;
            ChangeLitness(isLit
                ? (transitionTime - timeLeft) / transitionTime
                : 1 - (transitionTime - timeLeft) / transitionTime);
            yield return null;
        }
        
        // Wait some time.
        yield return new WaitForSeconds(isLit ? Random.Range(minTimeLit, maxTimeLit) : Random.Range(minTimeUnlit, maxTimeUnlit));

        // Change the lit state if this is active.
        if (isActiveAndEnabled) {
            co = StartCoroutine(UpdateLitState(!isLit));
        }
    }
    
    private void ChangeLitness(float normalizedLitness) {
        NormalizedLitness = normalizedLitness;
        pointLight.intensity = Mathf.Lerp(0, lightIntensity, NormalizedLitness);
        fireflyColor.a = Mathf.Lerp(0, 1, NormalizedLitness);
        material.SetColor("_Color", fireflyColor);
    }
    
    /**
     * Updates firefly's lit state. The firefly will transition to the state if it needs to change state,
     * and it will restart its random state duration if it's already in that state. If the script is disabled, the state
     * change will be permanent.
     */
    public void SetLitState(bool isLit) {
        if (co != null) {
            StopCoroutine(co);
        }
        co = StartCoroutine(UpdateLitState(isLit));
    }
}
