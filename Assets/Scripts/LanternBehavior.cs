using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternBehavior : MonoBehaviour {

    [SerializeField] private int maxFlies = 5;
    [SerializeField] private LayerMask fireflyMask;
    [SerializeField] private float suckSpeed = 5.5f;
    [SerializeField] private float suckRadius = 1f;

    private Queue<Collider> fliesInLantern = new Queue<Collider>();

    private void Update() {
        if (fliesInLantern.Count < maxFlies) {
            SuckUpFlies();
        } else {
            foreach (Collider fly in fliesInLantern) {
                fly.GetComponent<Light>().enabled = false;
                Destroy(fly.gameObject, Random.Range(2f, 5f));
            }
            transform.GetChild(0).gameObject.SetActive(true);
            EventManager.TriggerEvent(EventManager.Event.LanternLit);
            enabled = false;
        }
    }
    
    private void SuckUpFlies() {
        Collider[] flies = Physics.OverlapSphere(transform.position, suckRadius, fireflyMask);
        foreach (Collider fly in flies) {
            if (fliesInLantern.Count >= maxFlies) {
                break;
            }
            if (fliesInLantern.Contains(fly)) {
                continue;
            }
            FireflyMovement fireflyMovement = fly.GetComponent<FireflyMovement>();
            if (Vector3.Distance(transform.position, fly.transform.position) < 0.1f) {
                fireflyMovement.PutInContainer(transform, new Vector3(0.45f, 0.65f, 0.45f));
                fliesInLantern.Enqueue(fly);
                EventManager.TriggerEvent(EventManager.Event.FireflyInLantern);
            } else {
                fireflyMovement.Suck(transform.position, suckSpeed);
            }
        }
    }
}
