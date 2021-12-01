using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireflySpawner : MonoBehaviour {

    [SerializeField] private List<Transform> fireflies = new List<Transform>();

    private void Awake() {
        // Shuffle fireflies. Not the most correct and efficient way to do it...
        fireflies = fireflies.OrderBy(x => Random.value).ToList();
        // Spawn 3 of them from the get-go. (7 more will spawn immediately at the start of the game.)
        Spawn();
        Spawn();
        Spawn();
    }

    private void OnEnable() {
        EventManager.StartListening(EventManager.Event.FireflyInLantern, Spawn);
    }

    private void OnDisable() {
        EventManager.StopListening(EventManager.Event.FireflyInLantern, Spawn);
    }

    private void Spawn() {
        if (fireflies.Count > 0) {
            fireflies[0].gameObject.SetActive(true);
            fireflies.RemoveAt(0);
        }
    }
}
