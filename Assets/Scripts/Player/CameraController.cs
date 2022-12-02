using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkinColorRunner.Player
{
    public class CameraController : MonoBehaviour
    {
        public GameObject player;
        private float offsetZ = -5f;

        private void Update()
        {
            if (player != null)
            {
                Vector3 targetPos = new(transform.position.x, transform.position.y, player.transform.position.z + offsetZ);
                transform.position = targetPos;
            }
        }
    }
}