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
    public int barsCompleted;
    Vector3 distance;
    [SerializeField] CylinderBetweenTwoPoints cylinderProcedural;
    [HideInInspector]public GameObject cylinderObject;
    AudioSource audioSource;

    public bool hasBarBefore;
    public bool hasBarAfter;
    public Bar beforeBar;
    public Bar afterBar;

    public bool myTurn;
    public bool finishedTurn;
    bool firstFrame = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(!hasBarBefore && !hasBarAfter && myTurn)
        {
            FillMeter();
            if (firstFrame) return;
            if (BPM._beatD16)
            {
                PlayNextSound();
                barsCompleted++;
                if (barsCompleted >= bars)
                {
                    filler = 0;
                    barsCompleted = 0;
                    FinishTurn();
                }
            }
        }
        else if(myTurn || !finishedTurn)
        {
            FillMeter();
            if (firstFrame) return;
            if (BPM._beatD16)
            {
                PlayNextSound();
                barsCompleted++;
                if (barsCompleted >= bars)
                {
                    filler = 0;
                    barsCompleted = 0;
                    FinishTurn();
                }
            }
        }
        else if(!myTurn && BPM._beatFull)
        {
            myTurn = true;
        }
    }

    private void LateUpdate()
    {
        firstFrame = false;
    }

    void PlayNextSound()
    {
        if (beats[barsCompleted] != null)
            audioSource.PlayOneShot(beats[barsCompleted].GetComponent<Beat>().Clip());
    }

    void FinishTurn()
    {
        finishedTurn = true;
        if (hasBarAfter)
        {
            myTurn = false;
            afterBar.SetTurn(true);
        }
        else if(!hasBarAfter && hasBarBefore)
        {
            myTurn = false;
            GetFirstBar(this).SetTurn(true);
        }
    }

    public void SetTurn(bool b)
    {
        myTurn = b;
        finishedTurn = false;
        firstFrame = true;
    }

    Bar GetFirstBar(Bar currentBar)
    {
        if (currentBar.hasBarBefore && currentBar.beforeBar != null)
        {
            return GetFirstBar(currentBar.beforeBar);
        }
        else
        {
            return currentBar;
        }
    }

    void FillMeter()
    {
        distance = blue.transform.position - yellow.transform.position;
        fillEfect.transform.up = distance;
        filler += Time.deltaTime;
        fillEfect.transform.position = Vector3.Lerp(cylinderProcedural.attachPoints[0].transform.position, cylinderProcedural.attachPoints[16].transform.position, filler * BPM.instance._bpm / 60.0f);
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
            afterBar = null;
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
            afterBar = null;
        }
    }
}
