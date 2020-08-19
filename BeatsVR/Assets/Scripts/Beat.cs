using System.Collections;
using UnityEngine;

public class Beat : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    public bool attached;
    private Vector3 initialScale;

    private void Start()
    {
        initialScale = transform.localScale;
    }

    public AudioClip Clip()
    {
        if(attached)
            StartCoroutine(CoGrow(1));
        return clip;
    }

    IEnumerator CoGrow(float vel)
    {
        float counter = 0;
        while(counter <= BPM.instance._bpm / 60)
        {
            Vector3 targetScale = initialScale + Vector3.one * 0.6f;
            transform.localScale = Vector3.Lerp(targetScale, initialScale, counter);
            yield return new WaitForEndOfFrame();
            counter += vel * Time.deltaTime * BPM.instance._bpm / 60;
        }
    }

    public void DestroyAfter(float n)
    {
        Destroy(gameObject, n);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Beat") && !attached)
        {
            other.GetComponentInParent<Bar>().OnTrigger(transform.position, gameObject);
        }
    }

    public void Attach()
    {
        attached = true;
        StartCoroutine(CoGrow(2));
    }
}
