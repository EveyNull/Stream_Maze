using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuckingExplode : MonoBehaviour
{
    public float radius = 5.0F;
    public float power = 10.0F;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius);
        }

        Destroy(this, 5.0f);
    }
}
