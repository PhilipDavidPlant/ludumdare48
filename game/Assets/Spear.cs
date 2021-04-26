using UnityEngine;
using System.Collections.Generic;
using Dion;

public class Spear : MonoBehaviour
{

    [SerializeField] private float _drag;
    [SerializeField] private float _gracePeriod = 0.8f;
    [SerializeField] private GameObject _rubberThing;
    
    private Vector3 _target;
    private Transform _shooterTransform;
    private float _returnSpeed;
    private float _shootSpeed;
    private Vector3 _movement;
    private LineRenderer _lineRenderer;
    private BoxCollider _collider;
    private Transform _capturedTransform;
    private List<Fish> _caughtFishes = new List<Fish>(); // A spear can catch multiple if shots are lined up
    
    public void Initialize(float shootSpeed, Transform shooterTransform, float returnSpeed)
    {
        _shootSpeed = shootSpeed;
        _shooterTransform = shooterTransform;
        _returnSpeed = returnSpeed;
    }

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _collider = GetComponent<BoxCollider>();

        var camera = Camera.main;
        var mousePos = Input.mousePosition;
        mousePos.z = 0;
        _target = camera.ScreenToWorldPoint(mousePos);
        _target.z = 0;
        var direction = (_target - transform.position).normalized;
        _movement = _shootSpeed * direction;
    }

    void Update()
    {
        var counterDirectionToShooter = transform.position + (_shooterTransform.position - transform.position).normalized * -1;
        
        if(_movement != Vector3.zero && _gracePeriod > 0) 
        {
            transform.Translate(_movement);
            _movement = Vector3.Lerp(_movement, Vector3.zero, _drag * Time.deltaTime);
            if(_movement == Vector3.zero) {
                Destroy(_collider);
            }
        } else if (_gracePeriod > 0) {
            _gracePeriod -= Time.deltaTime;
        } else {
            _movement = Vector3.Lerp(_movement, (_shooterTransform.position - transform.position).normalized * _returnSpeed, Time.deltaTime);
            transform.Translate(_movement * Time.deltaTime);
            if(Vector3.Distance(transform.position, _shooterTransform.position) < 1.25) {
                _shooterTransform.GetComponent<SpearShooter>().availableSpears++;
                var dion = FindObjectOfType<DionController>();
                _caughtFishes.ForEach(fish => dion.money += fish.value / 2); // For some reason we register 2 fish each time we catch one and I don't have time to debug it and figure out why
                Destroy(gameObject);
            }
        }
        
        transform.GetChild(0).LookAt(counterDirectionToShooter, Vector3.up);
        _lineRenderer.SetPositions(new[] {_shooterTransform.position, transform.position});
    }

    private void OnTriggerEnter(Collider other) 
    {
        var fish = other.GetComponent<Fish>();
        if (fish != null) 
        {
            other.gameObject.transform.parent = _rubberThing.transform;
            _caughtFishes.Add(fish);
        }
    }
}
