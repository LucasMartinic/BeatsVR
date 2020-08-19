using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField] GameObject blue;
    [SerializeField] GameObject yellow;
    [SerializeField] GameObject fillEfect;
    [SerializeField] int bars = 1;
    [SerializeField] GameObject[] beats = new GameObject[16];
    float filler = 0;
    int barsCompleted;
    Vector3 distance;
    [SerializeField] CylinderBetweenTwoPoints cylinderProcedural;
    [HideInInspector]public GameObject cylinderObject;
    int currentBeat;
    AudioSource audioSource;

    public bool hasBarBefore;
    public bool hasBarAfter;
    private Bar beforeBar;
    private Bar afterBar;


    // Update is called once per frame
    void Update()
    {
        audioSource = GetComponent<AudioSource>();
        if (BPM._beatD16)
        {
            PlayNextSound();
            barsCompleted++;
            if(barsCompleted >= bars)
            {
                filler = 0;
                barsCompleted = 0;
            }
        }
        FillMeter();
    }

    void PlayNextSound()
    {
        if (beats[barsCompleted] != null)
            audioSource.PlayOneShot(beats[barsCompleted].GetComponent<Beat>().Clip());
    }

    void FillMeter()
    {
        distance = blue.transform.position - yellow.transform.position;
        fillEfect.transform.up = distance;
        filler += Time.deltaTime;
        fillEfect.transform.position = Vector3.Lerp(cylinderProcedural.attachPoints[0].transform.position, cylinderProcedural.attachPoints[16].transform.position, filler * BPM.instance._bpm / 60.0f);
        /*if(Vector3.Distance(cylinderProcedural.attachPoints[currentBeat].transform.position, fillEfect.transform.position) < 0.01f)
        {
            Debug.Log(currentBeat);
            if(beats[currentBeat] != null)
                audioSource.PlayOneShot(beats[currentBeat].GetComponent<Sample>().clip);
            currentBeat++;
            if(currentBeat == 16)
            {
                currentBeat = 0;
            }
        }*/
    }

    public void OnTrigger(Vector3 contactPos, GameObject obj)
    {
        GameObject nearestBeatPos = GetNearestAttachmentPoint(contactPos);
        int n = int.Parse(nearestBeatPos.name);
        if (beats[n] == null)
        {
            AttachBeat(nearestBeatPos, obj, n);
        }
    }
    void AttachBeat(GameObject positionGO, GameObject beatGO, int index)
    {
        GameObject obj = Instantiate(beatGO, positionGO.transform.position, positionGO.transform.rotation);
        obj.GetComponent<Beat>().Attach();
        obj.transform.SetParent(positionGO.transform);
        beats[index] = obj;
    }

    GameObject GetNearestAttachmentPoint(Vector3 location)
    {
        float nearestSqrMag = float.PositiveInfinity;
        GameObject nearestPos = null;

        for (int i = 0; i < cylinderProcedural.attachPoints.Length; i++)
        {
            float sqrMag = (cylinderProcedural.attachPoints[i].transform.position - location).sqrMagnitude;
            if (sqrMag < nearestSqrMag)
            {
                nearestSqrMag = sqrMag;
                nearestPos = cylinderProcedural.attachPoints[i];
            }
        }

        return nearestPos;
    }

    public void ConnectBefore(Bar bar)
    {
        if (!hasBarBefore)
        {
            hasBarBefore = true;
            beforeBar = bar;
        }
    }

    public void DisconnectBefore(Bar bar)
    {
        if(beforeBar == bar)
        {
            hasBarBefore = false;
        }
    }

    public void ConnectAfter(Bar bar)
    {
        if (!hasBarAfter)
        {
            hasBarAfter = true;
            afterBar = bar;
        }
    }

    public void DisconnectAfter(Bar bar)
    {
        if(afterBar == bar)
        {
            hasBarAfter = false;
        }
    }
}
