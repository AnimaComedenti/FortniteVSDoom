using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public float travelTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    private void Update()
    {
        travelTime -= Time.deltaTime;
        if (travelTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit");
        //Destroy(gameObject);
    }
}
