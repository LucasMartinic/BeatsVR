using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour
{
    [SerializeField] GameObject blue;
    [SerializeField] GameObject yellow;
    [SerializeField] GameObject fillEfect;
    [SerializeField] int bars = 1;
    float filler = 0;
    int barsCompleted;
    public Vector3 distance;

    // Update is called once per frame
    void Update()
    {
        if (BPM._beatFull)
        {
            barsCompleted++;
            if(barsCompleted >= bars)
            {
                filler = 0;
                barsCompleted = 0;
            }
        }
        FillMeter();
    }

    void FillMeter()
    {
        distance = blue.transform.position - yellow.transform.position;
        fillEfect.transform.up = distance;
        filler += Time.deltaTime;
        fillEfect.transform.position = Vector3.Lerp(yellow.transform.position, blue.transform.position, filler * BPM.instance._bpm /60 * (bars * 1.0f));
    }
}
