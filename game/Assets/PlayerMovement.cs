using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 10;
    void Update()
    {
        if (Input.GetKey("w"))
        {
            transform.Translate(Vector3.Scale((Vector3.up * Time.deltaTime),new Vector3(speed,speed,speed)));
        }
        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.Scale((Vector3.left * Time.deltaTime),new Vector3(speed,speed,speed)));
        }
        if (Input.GetKey("s"))
        {
            transform.Translate(Vector3.Scale((Vector3.down * Time.deltaTime),new Vector3(speed,speed,speed)));
        }
        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.Scale((Vector3.right * Time.deltaTime),new Vector3(speed,speed,speed)));
        }
    }
}
