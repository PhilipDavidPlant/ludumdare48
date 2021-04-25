using UnityEngine;

public class Spear : MonoBehaviour
{

    [SerializeField] private float _drag;
    [SerializeField] private float _gracePeriod = 0.8f;
    
    private Vector3 _target;
    private Transform _shooterTransform;
    private float _returnSpeed;
    private float _shootSpeed;
    private Vector3 _movement;
    private LineRenderer _lineRenderer;

    public void Initialize(float shootSpeed, Transform shooterTransform, float returnSpeed)
    {
        _shootSpeed = shootSpeed;
        _shooterTransform = shooterTransform;
        _returnSpeed = returnSpeed;
    }

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

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
        } else if (_gracePeriod > 0) {
            _gracePeriod -= Time.deltaTime;
        } else {
            _movement = Vector3.Lerp(_movement, (_shooterTransform.position - transform.position).normalized * _returnSpeed, Time.deltaTime);
            transform.Translate(_movement * Time.deltaTime);
            if(Vector3.Distance(transform.position, _shooterTransform.position) < 0.5) {
                _shooterTransform.GetComponent<SpearShooter>().availableSpears++;
                Destroy(gameObject);
            }
        }
        
        
        transform.GetChild(0).LookAt(counterDirectionToShooter, Vector3.up);
        _lineRenderer.SetPositions(new[] {_shooterTransform.position, transform.position});
    }
}
