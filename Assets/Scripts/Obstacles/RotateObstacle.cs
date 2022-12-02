using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkinColorRunner.Obstacles
{
    public class RotateObstacle : MonoBehaviour
    {
        [SerializeField] private float speed = 60f;
        [SerializeField] private bool inverse = false;

        void Start()
        {
            if (inverse)
            {
                speed *= -1;
            }
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(new Vector3(0f, Time.deltaTime * speed, 0f));
        }
    }
}