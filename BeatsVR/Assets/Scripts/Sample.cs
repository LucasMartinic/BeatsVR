using UnityEngine;

public class Sample : MonoBehaviour
{
    public AudioClip clip;
    public bool attached;

    public void DestroyAfter(int n)
    {
        Destroy(gameObject, n);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Beat") && !attached)
        {
            other.GetComponentInParent<Beat>().OnTrigger(transform.position, gameObject);
        }
    }

    public void Attach()
    {
        attached = true;
    }
}
