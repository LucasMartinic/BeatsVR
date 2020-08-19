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
            StartCoroutine(Grow());
        return clip;
    }

    IEnumerator Grow()
    {
        float counter = 0;
        for (int i = 0; i < 50; i++)
        {
            Vector3 targetScale = initialScale + new Vector3(0.3f, 0.3f, 0.3f);
            transform.localScale = Vector3.Lerp(targetScale, initialScale, counter);
            yield return new WaitForEndOfFrame();
            counter += Time.deltaTime * BPM.instance._bpm / 60;
        }
    }

    public void DestroyAfter(int n)
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
    }
}
