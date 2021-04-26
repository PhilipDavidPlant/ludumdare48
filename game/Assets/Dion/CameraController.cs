using System.Collections;
using System.Collections.Generic;
using Dion;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] [Range(0, 2f)] private float _smoothnes;

    private GameObject _player;
    private Vector3 _initialPosition;
    
    void Start()
    {
        _player = FindObjectOfType<DionController>().gameObject;
        _initialPosition = transform.position;
    }

    void Update()
    {
        var target = new Vector3(_initialPosition.x, _player.transform.position.y, _initialPosition.z);
        transform.position = Vector3.Lerp(transform.position, target, _smoothnes * Time.deltaTime);
    }
}
