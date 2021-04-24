using System;
using System.Collections;
using System.Collections.Generic;
using Dion;
using UnityEngine;

public class WaterLevel : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        var dionHead = other.gameObject.GetComponent<DionHead>();
        if (dionHead == null) return;
    
        dionHead.dion.isUnderWater = other.transform.position.y < transform.position.y;
    }
}
