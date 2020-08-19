using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField] GameObject blue;
    [SerializeField] GameObject yellow;
    [SerializeField] GameObject fillEfect;
    MeshRenderer fillRenderer;
    ParticleSystem fillParticles;
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
    bool single = true;

    public bool myTurn;
    bool readyToFill;
    bool firstFrame = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        fillRenderer = fillEfect.GetComponentInChildren<MeshRenderer>();
        fillParticles = fillEfect.GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (!hasBarAfter && !hasBarBefore) single = true;
        else single = false;

        if (BPM._beatFull)
        {
            CheckIfCandidate();
        }

        if(myTurn && readyToFill)
        {
            if (!fillParticles.isPlaying) fillParticles.Play();
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
        else
        {
            fillRenderer.enabled = false;
            fillParticles.Stop();
        }

    }

    void CheckIfCandidate()
    {
        if (single)
        {
            myTurn = true;
            readyToFill = true;
        }
        if(!single && myTurn)
        {
            readyToFill = true;
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
        readyToFill = false;
        myTurn = false;
        fillRenderer.enabled = false;
        if (hasBarAfter && afterBar!= null)
        {
            myTurn = false;
            afterBar.SetTurn(true);
        }
        else if(!hasBarAfter && hasBarBefore)
        {
            myTurn = false;
            GetFirstBar(this).SetTurn(true);
        }
        fillParticles.Stop();
    }

    public void SetTurn(bool b)
    {
        myTurn = b;
        firstFrame = true;
    }

    void StartFromBegining()
    {
        GetFirstBar(this).SetTurn(true);
    }

    public void ResetBar()
    {
        filler = 0;
        barsCompleted = 0;
    }

    Bar GetFirstBar(Bar currentBar)
    {
        if (currentBar.hasBarBefore && currentBar.beforeBar != null)
        {
            currentBar.ResetBar();
            currentBar.SetTurn(false);
            return GetFirstBar(currentBar.beforeBar);
        }
        else
        {
            return currentBar;
        }
    }

    void FillMeter()
    {
        if (!fillRenderer.enabled)
            fillRenderer.enabled = true;
        distance = blue.transform.position - yellow.transform.position;
        fillEfect.transform.up = distance;
        filler += Time.deltaTime;
        fillEfect.transform.position = Vector3.Lerp(yellow.transform.position, blue.transform.position, filler * BPM.instance._bpm / 60.0f);
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
        StartFromBegining();
    }

    public void DisconnectBefore(Bar bar)
    {
        if(beforeBar == bar)
        {
            hasBarBefore = false;
            afterBar = null;
            CheckIfAnyBarHasTurn(false);
        }
    }

    public void ConnectAfter(Bar bar)
    {
        if (!hasBarAfter)
        {
            hasBarAfter = true;
            afterBar = bar;
        }
        StartFromBegining();
    }

    public void DisconnectAfter(Bar bar)
    {
        if(afterBar == bar)
        {
            hasBarAfter = false;
            afterBar = null;
            CheckIfAnyBarHasTurn(true);
        }
    }

    void CheckIfAnyBarHasTurn(bool forward)
    {
        if (forward)
        {

        }
        else
        {

        }
    }
}
