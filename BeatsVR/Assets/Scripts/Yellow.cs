using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yellow : MonoBehaviour
{
    Beat beat;
    // Start is called before the first frame update
    void Start()
    {
        beat = GetComponentInParent<Beat>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blue"))
        {
            beat.ConnectBefore(beat);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Blue"))
        {
            beat.DisconnectBefore(beat);
        }
    }
}
