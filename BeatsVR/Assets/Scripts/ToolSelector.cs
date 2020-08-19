using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSelector : MonoBehaviour
{
    public enum Hand { Left, Right};
    [SerializeField] Hand hand;
    [SerializeField] float grabThreshold = 0.6f;
    [SerializeField] GameObject handGO;
    [SerializeField] GameObject beatTool;

    float grabAmount;
    Vector2 thumbstickDir;
    bool beatToolActive;

    //BeatTool
    [SerializeField] MeshRenderer hexagonRenderer;
    [SerializeField] Material highlightMat;
    private Material initialMat;
    private int actualSound;
    private Material[] matArray;
    private SoundSelector soundSelector;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initialMat = hexagonRenderer.material;
        soundSelector = beatTool.GetComponent<SoundSelector>();
    }

    void Update()
    {
        if(hand == Hand.Left)
        {
            thumbstickDir = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
            if(thumbstickDir != Vector2.zero)
            {
                soundSelector.RotateTool(true);
                Highlight(thumbstickDir);
                if(!beatToolActive)
                    ActivateBeatTool();
            }
            else
            {
                soundSelector.RotateTool(false);
            }
            grabAmount = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);
        }
        else if(hand == Hand.Right)
        {
            thumbstickDir = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
            if (thumbstickDir != Vector2.zero)
            {
                soundSelector.RotateTool(true);
                Highlight(thumbstickDir);
                if (!beatToolActive)
                    ActivateBeatTool();
            }
            else
            {
                soundSelector.RotateTool(false);
            }
            grabAmount = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);
        }
        if (grabAmount >= grabThreshold)
        {
            ActivateHand();
        }
    }

    void Highlight(Vector2 vec)
    {
        float angle;
        if(vec.y < 0)
        {
            angle = 360 - Vector2.Angle(Vector2.left, vec);
        }
        else
        {
            angle = Vector2.Angle(Vector2.left, vec);

        }
        if (angle >= 0 && angle < 60)
        {
            ChooseSound(0);
        }
        else if (angle >= 60 && angle < 120)
        {
            ChooseSound(1);
        }
        else if (angle >= 120 && angle < 180)
        {
            ChooseSound(2);
        }
        else if (angle >= 180 && angle < 240)
        {
            ChooseSound(3);
        }
        else if (angle >= 240 && angle < 300)
        {
            ChooseSound(4);
        }
        else if (angle >= 300 && angle < 360)
        {
            ChooseSound(5);
        }
    }

    void ChooseSound(int n)
    {
        matArray = hexagonRenderer.materials;
        if(n != actualSound)
        {
            matArray[actualSound] = initialMat;
            hexagonRenderer.materials = matArray;
            GetComponent<OculusHaptics>().Vibrate(VibrationForce.Light);
            soundSelector.SelectBeat(n);
            PlayAudio(n);
        }
        actualSound = n;
        matArray[actualSound] = highlightMat;
        hexagonRenderer.materials = matArray;
    }

    void ActivateHand()
    {
        beatToolActive = false;
        handGO.SetActive(true);
        beatTool.SetActive(false);
    }

    void ActivateBeatTool()
    {
        beatToolActive = true;
        beatTool.SetActive(true);
        handGO.SetActive(false);
    }

    void PlayAudio(int n)
    {
        audioSource.clip = soundSelector.beats[n].GetComponent<Beat>().Clip();
        audioSource.Play();
    }
}
