using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue : MonoBehaviour
{
    Bar bar;
    // Start is called before the first frame update
    void Start()
    {
        bar = GetComponentInParent<Bar>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Yellow"))
        {
            bar.ConnectAfter(other.GetComponentInParent<Bar>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Yellow"))
        {
            bar.DisconnectAfter(other.GetComponentInParent<Bar>());
        }
    }
}
