using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampMecanic : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void activation(int p = 1)
    {
        if(p == 1)
            rb.isKinematic = false;
    }

}
