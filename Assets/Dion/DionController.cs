using System;
using UnityEngine;

namespace Dion
{
    public class DionController : MonoBehaviour
    {
        [SerializeField] [Range(0, 1f)] private float fallAcceleration;

        [SerializeField] [Range(0, 1f)] private float baseMovementAcceleration;
        [SerializeField] [Range(0, 1f)] private float baseMovementDeceleration;
        
        [SerializeField]
        private float maxFallSpeed;
        
        [SerializeField]
        private float baseMovementSpeed;
        

        private Vector3 _movement;
        private float _gravity;

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
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