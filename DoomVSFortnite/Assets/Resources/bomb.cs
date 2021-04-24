using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public float delay;
    public float explodRadius;
    public float explodeForce;
    public float upModifier;
    public LayerMask interactionLayer;
    public GameObject particlePrefab;
    private GameObject particleInstance;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Explode), delay);
    }

    // Update is called once per frame
    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodRadius, interactionLayer);
        foreach(Collider c in colliders)
        {
            c.GetComponent<Rigidbody>().
                AddExplosionForce(explodeForce,transform.position,explodRadius,upModifier,ForceMode.Impulse);
        }
        particleInstance = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        GetComponent<AudioSource>().Play();
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        
        Invoke(nameof(Kill), 5);
    }

    void Kill()
    {
        Destroy(particleInstance);
        Destroy(gameObject);
    }
}
