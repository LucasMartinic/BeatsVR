using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderBetweenTwoPoints : MonoBehaviour {
    [SerializeField]
    private Transform cylinderPrefab;

    [SerializeField] private GameObject initialPoint;
    [SerializeField] private GameObject rightSphere;
    private GameObject cylinder;
    public GameObject[] attachPoints = new GameObject[17];
    [SerializeField] Bar bar;

    private void Start () {
        InstantiateCylinder(cylinderPrefab, initialPoint.transform.position, rightSphere.transform.position);
    }

    private void Update () {
        UpdateCylinderPosition(cylinder, initialPoint.transform.position, rightSphere.transform.position);
    }

    private void InstantiateCylinder(Transform cylinderPrefab, Vector3 beginPoint, Vector3 endPoint)
    {
        cylinder = Instantiate<GameObject>(cylinderPrefab.gameObject, Vector3.zero, Quaternion.identity);
        cylinder.transform.SetParent(bar.transform);
        UpdateCylinderPosition(cylinder, beginPoint, endPoint);

        CreateAttachPoints();
        bar.cylinderObject = cylinder;
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

    void CreateAttachPoints()
    {
        for (int i = 0; i < 17; i++)
        {
            attachPoints[i] = new GameObject(i.ToString());
            attachPoints[i].transform.SetParent(cylinder.transform);
            attachPoints[i].transform.localPosition = Vector3.zero + new Vector3(0, 0, (0.4f) - (0.0533f * i));
            attachPoints[i].transform.localRotation = cylinder.transform.localRotation;
        }
    }
}
