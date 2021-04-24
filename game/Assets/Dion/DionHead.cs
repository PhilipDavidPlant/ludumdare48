using System.Collections;
using System.Collections.Generic;
using Dion;
using UnityEngine;

public class DionHead : MonoBehaviour
{
    public DionController dion;
    
    void Start()
    {
        dion = transform.parent.GetComponent<DionController>();
    }
}
