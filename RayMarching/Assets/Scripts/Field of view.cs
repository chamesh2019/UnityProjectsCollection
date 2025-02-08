using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fieldofview : MonoBehaviour
{
    public float viewRadius;
    public float viewAngle;

    public LayerMask targets;
    public LayerMask obsticles;

    public List<Transform> visibleTargets = new();

    void Start() {
        StartCoroutine("FindTargets", .1f);
    }

    IEnumerator FindTargets(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets() {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targets);
        for (int i = 0; i < targetsInViewRadius.Length; i++) {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTartget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTartget) < viewAngle / 2) {
                float disToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTartget, disToTarget, obsticles)) {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) { 
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
