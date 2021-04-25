using System;
using Dion;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fish : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;
    [SerializeField] private float _sightDistance;

    private Transform _player;
    private Vector3 _target;
    private float _nextTargetArea = 10f;
    
    enum State { Idling, MovingToTarget, RunningFromPlayer, LookingForTarget } // there might be a new one here for being caught
    
    // somehow variate the speed
    private Vector3 _movement;
    private State _currentState = State.Idling;
    private float _idlingTime = 0;
    private float _waterHeight;
    private Vector3 _movingDirection;
    
    private void Start()
    {
        _player = FindObjectOfType<DionController>().gameObject.transform;
        _waterHeight = FindObjectOfType<WaterLevel>().gameObject.transform.position.y;
    }

    private void Update()
    {
        switch (_currentState)
        {
            case State.Idling:
                HandleIdling();
                break;
            case State.MovingToTarget:
                HandleMovingToTarget();
                break;
            case State.RunningFromPlayer:
                HandleRunningFromPlayer();
                break;
            case State.LookingForTarget:
                HandleLookingForTarget();
                break;
        }
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.DrawSphere(_target, .5f);
        Gizmos.DrawSphere(transform.position + transform.forward * 2 , .5f);
        Gizmos.DrawRay(transform.position, _movingDirection * _sightDistance);
    }

    private void ChangeState(State newState, [CanBeNull] ChangeStateParameters parameters = null)
    {
        // clean up
        switch (_currentState)
        {
            case State.Idling:
                _idlingTime = 0;
                break;
            case State.MovingToTarget:
                _movement = new Vector3();
                break;
        }
        
        // initialize
        switch (newState)
        {
            case State.Idling:
                _idlingTime = Random.Range(2f, 5f);
                break;
            case State.LookingForTarget:
                var props = (LookingForTargetChangeStateParameters) parameters;
                
                if (props != null)
                {
                    var directionToTarget = (_target - transform.position).normalized * -1;
                    var furthestTarget = transform.position + (directionToTarget * _nextTargetArea);
                    do
                    {
                        _target = transform.position + new Vector3(Random.Range(furthestTarget.x - _nextTargetArea / 2, furthestTarget.x + _nextTargetArea / 2),
                            Random.Range(furthestTarget.y - _nextTargetArea / 2, furthestTarget.y + _nextTargetArea / 2), 0);    
                    } while (_target.y >= _waterHeight - 5);
                    
                }
                else
                {
                    do
                    {
                        _target = transform.position + new Vector3(Random.Range(-_nextTargetArea, _nextTargetArea),
                            Random.Range(-_nextTargetArea, _nextTargetArea), 0);
                    } while (_target.y >= _waterHeight - 5);    
                }
                
                break;
        }

        _currentState = newState;
    }

    private void HandleIdling()
    {
        _idlingTime = Math.Max(_idlingTime - Time.deltaTime, 0);
        if (_idlingTime == 0)
        {
            ChangeState(State.LookingForTarget);
        }
    }

    private void HandleMovingToTarget()
    {
        _movingDirection = (_target - transform.position).normalized;
        _movingDirection.z = 0;
        _movement = Vector3.Lerp(_movement, _movingDirection * _speed, _acceleration * Time.deltaTime) * GetSpeedModifier(transform.position);
        
        transform.Translate(_movement);
        
        if (Vector3.Distance(transform.position, _target) < 2f)
        {
            ChangeState(State.Idling);
        } else if (IsGoingToHitSomething())
        {
            ChangeState(State.LookingForTarget, new LookingForTargetChangeStateParameters{LookBehind = true});
        }
    }

    private void HandleRunningFromPlayer()
    {
        
    }

    private void HandleLookingForTarget()
    {
        ChangeState(State.MovingToTarget);
    }

    private bool IsGoingToHitSomething()
    {
        int layerMask = 1 << 6;
        layerMask = ~layerMask;
        return Physics.Raycast(transform.position, _movingDirection * _sightDistance, _sightDistance, layerMask); 
    }

    private float GetSpeedModifier(Vector2 position) {
        return  Mathf.PerlinNoise(position.x / 10, position.y / 10);
    }
    
    class ChangeStateParameters {}

    class LookingForTargetChangeStateParameters : ChangeStateParameters
    {
        public bool LookBehind { get; set; }
    }
    
}
