using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderBetweenTwoPoints : MonoBehaviour {
    [SerializeField]
    private Transform cylinderPrefab;

    [SerializeField] private GameObject initialPoint;
    [SerializeField] private GameObject rightSphere;
    private GameObject cylinder;

    private void Start () {
        InstantiateCylinder(cylinderPrefab, initialPoint.transform.position, rightSphere.transform.position);
    }

    private void Update () {
        UpdateCylinderPosition(cylinder, initialPoint.transform.position, rightSphere.transform.position);
    }

    private void InstantiateCylinder(Transform cylinderPrefab, Vector3 beginPoint, Vector3 endPoint)
    {
        cylinder = Instantiate<GameObject>(cylinderPrefab.gameObject, Vector3.zero, Quaternion.identity);
        UpdateCylinderPosition(cylinder, beginPoint, endPoint);
    }

    private void UpdateCylinderPosition(GameObject cylinder, Vector3 beginPoint, Vector3 endPoint)
    {
        Vector3 offset = endPoint - beginPoint;
        Vector3 position = beginPoint + (offset / 2.0f);

        cylinder.transform.position = position;
        cylinder.transform.LookAt(beginPoint);
        Vector3 localScale = cylinder.transform.localScale;
        localScale.z = (endPoint - beginPoint).magnitude;
        cylinder.transform.localScale = localScale;
    }
}
