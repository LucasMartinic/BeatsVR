using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSelector : MonoBehaviour
{
    public GameObject[] beats;
    bool toolRotating;
    [SerializeField] Transform toolContainer;
    [SerializeField] Vector3 rotation;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SelectBeat(int n)
    {
        foreach (var item in beats)
        {
            item.SetActive(false);
        }
        beats[n].SetActive(true);
        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void RotateTool(bool b)
    {
        if(b && !toolRotating)
        {
            toolRotating = true;
        }
        else if(!b && toolRotating)
        {
            toolRotating = false;
        }
    }

    private void Update()
    {
        if (toolRotating)
        {
            toolContainer.Rotate(rotation * Time.deltaTime);
        }
    }
}
