using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dion
{
    public class DionController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] [Range(0, 1f)] private float fallAcceleration;

        [SerializeField] [Range(0, 1f)] private float baseMovementAcceleration;
        [SerializeField] [Range(0, 1f)] private float baseMovementDeceleration;
        
        [SerializeField]
        private float maxFallSpeed;
        
        [SerializeField]
        private float baseMovementSpeed;
        
        [Header("Life")]
        [SerializeField]
        private float baseOxygenTime = 30f;

        [SerializeField] [Range(1f, 5f)] private float gainBreathFactor;
        

        private Vector3 _movement;
        
        public float oxygenLeft;
        public bool isDead;

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
            else if (oxygenLeft <= baseOxygenTime) // do not fill up the tanks, just player's "lungs"
            {
                oxygenLeft = Math.Min(baseOxygenTime, oxygenLeft + Time.deltaTime * gainBreathFactor);
            }
            
            
            if (oxygenLeft <= 0)
            {
                oxygenLeft = 0;
                isDead = true;    
            }
        }

        private void Move()
        {
            var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
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
        }
    }
}