using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yellow : MonoBehaviour
{
    Bar bar;
    // Start is called before the first frame update
    void Start()
    {
        bar = GetComponentInParent<Bar>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blue"))
        {
            bar.ConnectBefore(bar);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Blue"))
        {
            bar.DisconnectBefore(bar);
        }
    }
}
