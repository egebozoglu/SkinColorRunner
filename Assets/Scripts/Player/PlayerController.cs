using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SkinColorRunner.Manager;
using UnityEngine;

namespace SkinColorRunner.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private DynamicJoystick joystick;
        [SerializeField] private float movingSpeed = 5f;
        [SerializeField] private SkinnedMeshRenderer playerSkinnedMeshRenderer;
        [SerializeField] private List<Material> materials = new();
        [SerializeField] private GameObject smokePrefab;

        // event
        public static event Action GameEnd;

        // general properties
        private Rigidbody rb;
        private Animator animator;
        private Material playerMaterial;

        // movement properties
        private bool gameActive = false;
        private bool moveActive = true;
        private readonly float limitPosX = 2f;

        // hit wrong material properties
        private bool movingBack = false;
        private readonly float moveBackPosZ = 3f;
        private Vector3 moveBackTargetPosition;

        #endregion

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            playerMaterial = playerSkinnedMeshRenderer.material;

            GameManager.GameStart += GameStart;
        }

        private void FixedUpdate()
        {
            if (gameActive)
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
        }

        private void GameStart()
        {
            animator.SetTrigger("Running");
            gameActive = true;
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
            string tag = other.gameObject.tag;
            if (tag.Contains("Door"))
            {
                int doorIndex = int.Parse(tag[^1].ToString());
                playerSkinnedMeshRenderer.material = materials[doorIndex];
                playerMaterial = playerSkinnedMeshRenderer.material;
            }
            else if (tag == "FinishLine")
            {
                Debug.Log("Finish");
                other.gameObject.SetActive(false);
                gameActive = false;
                animator.SetTrigger("Dancing");
                GameEnd?.Invoke();
            }
            else
            {
                if (renderer.material.name == playerMaterial.name)
                {
                    Debug.Log("Same Material");
                }
                else
                {
                    if (!movingBack)
                    {
                        InstantiateSmoke();
                        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                        moveBackTargetPosition = transform.position - Vector3.forward * moveBackPosZ;
                        animator.SetTrigger("Wrong");
                        moveActive = false;
                        movingBack = true;
                    }
                }
            }
        }

        /// <summary>
        /// Call when the player collided with different material
        /// </summary>
        private void HitWrongMaterial()
        {
            if (transform.position.z <= moveBackTargetPosition.z)
            {
                movingBack = false;
                moveActive = true;
                animator.SetTrigger("Running");
                return;
            }
            // move back
            transform.Translate(movingSpeed * Time.deltaTime * -Vector3.forward);
        }

        private void InstantiateSmoke()
        {
            GameObject smoke = Instantiate(smokePrefab, transform.position, Quaternion.identity);

            Destroy(smoke, 1f);
        }
    }
}