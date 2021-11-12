using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BottleController : MonoBehaviour {

    [SerializeField] private LayerMask fliesLayer;
    [SerializeField] private Transform coneBaseCenter;
    [SerializeField] private Transform coneApex;
    [SerializeField] private float coneBaseRadius;
    [SerializeField] private float suckSpeed = 5f;

    private Animation anim;
    private AnimationState animState;
    private float cosOfHalfAperture;

    private List<Collider> fliesInBottle = new List<Collider>(); 

    private void Awake() {
        anim = GetComponent<Animation>();
        animState = anim[anim.clip.name];
        animState.wrapMode = WrapMode.ClampForever;
        cosOfHalfAperture = Mathf.Cos(Mathf.Atan2(coneBaseRadius, Vector3.Distance(coneApex.position, coneBaseCenter.position)));
    }
    private void Update() {
        // Handle bottle animation.
        animState.speed = (Input.GetButton("Fire1") ? 1 : -1);
        animState.normalizedTime = Mathf.Clamp01(animState.normalizedTime);

        // If bottle is extended.
        if (animState.normalizedTime >= 1) {
            Collider[] flies = getFliesInBroadBox();
            foreach (Collider fly in flies) {
                if (fliesInBottle.Contains(fly)) {
                    continue;
                }
                if (isInCone(fly.transform.position)) {
                    // TODO: Can be cached for better performance if needed.
                    FireflyMovement fireflyMovement = fly.GetComponent<FireflyMovement>();
                    if (Vector3.Distance(transform.position, fly.transform.position) < 0.1f) {
                        fireflyMovement.PutInContainer(transform, new Vector3(0.175f, 0.2f, 0.175f));
                        fliesInBottle.Add(fly);
                    } else {
                        fireflyMovement.MoveTowards(transform.position, suckSpeed);
                    }
                }
            }
        }
    } 

    // https://stackoverflow.com/questions/10768142/verify-if-point-is-inside-a-cone-in-3d-space
    private bool isInCone(Vector3 point) {
        Vector3 apexToPoint = coneApex.position - point;
        Vector3 coneAxis = coneApex.position - coneBaseCenter.position;
        float apexToPointDotConeAxis = Vector3.Dot(apexToPoint, coneAxis);

        // Return early if the point is not inside a "infinite" version of the cone.
        if (apexToPointDotConeAxis / apexToPoint.magnitude / coneAxis.magnitude < cosOfHalfAperture) {
            return false;
        }
        
        // At this point, we know that angle-wise, the point is inside the code, but is it distance-wise?
        return apexToPointDotConeAxis / coneAxis.magnitude < coneAxis.magnitude;
    }
    
    private Collider[] getFliesInBroadBox() {
        Vector3 boxCenter = Vector3.Lerp(coneApex.position, coneBaseCenter.position, 0.5f);
        Vector3 boxHalfExtents = new Vector3(coneBaseRadius, coneBaseRadius, Vector3.Distance(boxCenter, coneBaseCenter.position));
        return Physics.OverlapBox(boxCenter, boxHalfExtents, Quaternion.LookRotation(coneApex.position - coneBaseCenter.position), fliesLayer);
    }
}
