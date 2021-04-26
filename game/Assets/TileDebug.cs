using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDebug : MonoBehaviour
{
    public LevelTile tile;
    
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("tiggered");
        tile.GenerateNeighbours();
    }
}
