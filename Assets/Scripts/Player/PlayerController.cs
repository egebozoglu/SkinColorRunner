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
        [SerializeField] private SkinnedMeshRenderer playerSkinnedMeshRenderer;

        // general properties
        private Rigidbody rb;
        private Animator animator;
        private Material playerMaterial;

        // movement properties
        private bool moveActive = true;
        private readonly float limitPosX = 2f;

        // hit wrong material properties
        private readonly float moveBackPosZ = 3f;
        private Vector3 moveBackTargetPosition;

        #endregion

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            animator.SetTrigger("Running");
            playerMaterial = playerSkinnedMeshRenderer.material;
        }

        private void FixedUpdate()
        {
            if (moveActive)
            {
                Movement();
            }
            else
            {
                HitWrongMaterial();
            }
        }

        /// <summary>
        /// Player Movement, limit horizontal movement according to limitPosX, rotate with horizontal joystick input
        /// </summary>
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

            Vector3 horizontalTarget = 0.5f * horizontal * Vector3.right; // horizontal move

            // limit horizontal move
            if ((transform.position.x >= limitPosX && horizontalTarget.x > 0) || (transform.position.x <= -limitPosX && horizontalTarget.x < 0))
            {
                horizontalTarget = Vector3.zero;
            }

            // rotation
            Quaternion targetRotation;

            if (horizontalTarget!=Vector3.zero)
            {
                targetRotation = Quaternion.Euler(0f, 45f * horizontal, 0f);
            }
            else
            {
                targetRotation = Quaternion.Euler(0f, 0f, 0f);
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);

            // position
            rb.MovePosition(transform.position + (Time.deltaTime * movingSpeed * (Vector3.forward + horizontalTarget)));
        }

        // handle trigger collision
        private void OnTriggerEnter(Collider other)
        {
            MeshRenderer renderer = other.gameObject.GetComponent<MeshRenderer>();
            if (renderer.material.name == playerMaterial.name)
            {
                Debug.Log("Same Material");
            }
            else
            {
                moveBackTargetPosition = transform.position - Vector3.forward * moveBackPosZ;
                moveActive = false;
                animator.SetTrigger("Wrong");
            }
        }

        /// <summary>
        /// Call when the player collided with different material
        /// </summary>
        private void HitWrongMaterial()
        {
            if (transform.position.z <= moveBackTargetPosition.z)
            {
                moveActive = true;
                animator.SetTrigger("Running");
                return;
            }
            // move back
            transform.Translate(movingSpeed * Time.deltaTime * -Vector3.forward);
        }
    }
}