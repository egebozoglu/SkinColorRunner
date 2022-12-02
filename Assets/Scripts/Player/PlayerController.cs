using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkinColorRunner.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float speed = 5f;
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
            rb.MovePosition(transform.position + (Time.deltaTime * speed * Vector3.forward));
        }
    }
}