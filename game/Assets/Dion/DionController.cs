using System;
using System.Collections;
using UnityEngine;

namespace Dion
{
    public class DionController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] [Range(0, 1f)] private float fallAcceleration;

        [SerializeField] [Range(0, 1f)] private float baseMovementAcceleration;
        [SerializeField] [Range(0, 1f)] private float baseMovementDeceleration;
        
        [SerializeField] private float maxFallSpeed;
        
        [SerializeField] private float baseMovementSpeed;
        
        [Header("Animation")]
        [SerializeField] private GameObject _mesh;
        [SerializeField] private Animator _animator;
        
        [Header("Life")]
        [SerializeField]
        private float baseOxygenTime = 30f;

        [SerializeField] [Range(1f, 5f)] private float gainBreathFactor;
        

        private Vector3 _movement;
        private int _maxDepth = 0;
        private float _meshScaleModifier = 1;
        
        public float oxygenLeft;
        public float maxOxygen;
        public bool isDead;
        
        private int _money = 0;
        public int money {
            get => _money;
            set 
            {
                _money = value;
                EventManager.ChangeMoney(value);
            }
        }

        private bool _isUnderWater;
        
        public bool isUnderWater
        {
            get => _isUnderWater;
            set
            {
                _isUnderWater = value;
                if (!_isUnderWater) _movement.y = 0;
            }
        }

        private void Start()
        {
            oxygenLeft = baseOxygenTime;
            maxOxygen = oxygenLeft;
            StartCoroutine("SaveDepth");
        }

        private void Update()
        {
            if (isDead) return;
            
            Move();
            Breathe();
        }

        private void Breathe()
        {
            if (_isUnderWater)
            {
                oxygenLeft -= Time.deltaTime;    
            }
            else if (oxygenLeft <= maxOxygen) // do not fill up the tanks, just player's "lungs"
            {
                oxygenLeft = Math.Min(maxOxygen, oxygenLeft + Time.deltaTime * gainBreathFactor);
            }
            
            
            if (oxygenLeft <= 0 && !isDead)
            {
                oxygenLeft = 0;
                isDead = true;    
                EventManager.KillPlayer(_maxDepth);
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                Destroy(this);
            }
        }

        private void Move2()
        {
            var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            MoveMesh(input);

            input.y = !_isUnderWater && input.y > 0 ? 0 : input.y;
            var accelerationModifiers = new Vector2(
                Math.Abs(input.x) < 0.25 ? baseMovementDeceleration : baseMovementAcceleration,
                Math.Abs(input.y) < 0.25 ? baseMovementDeceleration : baseMovementAcceleration
            );

            _movement.y = Mathf.Min(baseMovementSpeed, Mathf.Lerp(_movement.y, input.y * baseMovementSpeed,
                accelerationModifiers.x * Time.deltaTime));

            // // Decide if we need to apply gravity or player input            
            // if (Math.Abs(input.y) < 0.25)
            // {
            //     _movement.y = Mathf.Lerp(_movement.y, -maxFallSpeed,
            //         fallAcceleration * Time.deltaTime);
            // }
            // else
            // {
            //     _movement.y = Mathf.Lerp(_movement.y, input.y * baseMovementSpeed,
            //         accelerationModifiers.y * Time.deltaTime);
            // }


            transform.Translate(transform.right * _movement.y);
        }

        private void Move()
        {
            var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            MoveMesh(input);
            input.y = !_isUnderWater && input.y > 0 ? 0 : input.y;
            var accelerationModifiers = new Vector2(
                Math.Abs(input.x) < 0.25 ? baseMovementDeceleration : baseMovementAcceleration,
                Math.Abs(input.y) < 0.25 ? baseMovementDeceleration : baseMovementAcceleration
            );
            
            _movement.x = Mathf.Min(baseMovementSpeed, Mathf.Lerp(_movement.x, input.x * baseMovementSpeed,
                accelerationModifiers.x * Time.deltaTime));
            
            // Decide if we need to apply gravity or player input            
            if (Math.Abs(input.y) < 0.25)
            {
                _movement.y = Mathf.Lerp(_movement.y, -maxFallSpeed,
                    fallAcceleration * Time.deltaTime);
            }
            else
            {
                _movement.y = Mathf.Lerp(_movement.y, input.y * baseMovementSpeed,
                    accelerationModifiers.y * Time.deltaTime);
            }

            transform.Translate(_movement);
            _animator.speed = Mathf.Max(0.2f, _movement.magnitude / baseMovementSpeed);
            
        }

        private void MoveMesh2(Vector2 input)
        {
            _mesh.transform.Rotate(new Vector3(input.x * 50f * Time.deltaTime, 0, 0));
            if (_mesh.transform.eulerAngles.x > 90 || _mesh.transform.eulerAngles.x < -90)
            {
                var scale = _mesh.transform.localScale;
                _mesh.transform.localScale = new Vector3(scale.x, scale.y, -Mathf.Abs(scale.z));
            } else {
                var scale = _mesh.transform.localScale;
                _mesh.transform.localScale = new Vector3(scale.x, scale.y, Mathf.Abs(scale.z));
            }
        }
        private void MoveMesh(Vector2 input)
        {
            if(input.x != 0) {
                _meshScaleModifier = Mathf.Sign(input.x);
                _mesh.transform.localScale = new Vector3(_mesh.transform.localScale.z, _mesh.transform.localScale.y, _meshScaleModifier * Mathf.Abs(_mesh.transform.localScale.z));
            }

            var xRotation = 0.0f;

            if(input.y > 0) {
                if(input.x > 0) {
                    xRotation = -45;
                } else if (input.x < 0) {
                    xRotation = -45;
                } else {
                    xRotation = -90;
                }
            } else if (input.y < 0) {
                if(input.x > 0) {
                    xRotation = 45;
                } else if (input.x < 0) {
                    xRotation = 45;
                } else {
                    xRotation = 90;
                }
            }

            xRotation *= _meshScaleModifier;

            var rotation = _mesh.transform.eulerAngles;
            _mesh.transform.rotation = Quaternion.Euler(new Vector3(xRotation, rotation.y, rotation.z));
        }

        private IEnumerator SaveDepth()
        {
            for(;;)
            {
                var currentDepth = (int) Mathf.Abs(transform.position.y);
                if(currentDepth > _maxDepth)
                {
                    _maxDepth = currentDepth;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        public void IncreaseMaxOxygen(int value) 
        {
            gainBreathFactor += gainBreathFactor * (value / maxOxygen);
            maxOxygen += value;
        }

        public void UpgradeSpeed(float amount)
        {
            baseMovementAcceleration += amount * 50;
            baseMovementSpeed += amount;
        }

        public void AddMoney(int value)
        {
            this.money += value;
        }
    }
}