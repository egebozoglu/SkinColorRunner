using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkinColorRunner.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private DynamicJoystick joystick;
        [SerializeField] private float movingSpeed = 5f;
        private Rigidbody rb;
        private Animator animator;
        #endregion

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            animator.SetTrigger("Running");
        }

        private void FixedUpdate()
        {
            Movement();
        }

        private void Movement()
        {
            var horizontal = joystick.Horizontal;

            if (horizontal < 0)
            {
                horizontal = -1f;
            }
            else if (horizontal > 0)
            {
                horizontal = 1f;
            }

            // rotation
            Quaternion targetRotation = Quaternion.Euler(0f, 45f * horizontal, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);

            // position
            rb.MovePosition(transform.position + (Time.deltaTime * movingSpeed * (Vector3.forward + Vector3.right * horizontal/2f)));
        }
    }
}