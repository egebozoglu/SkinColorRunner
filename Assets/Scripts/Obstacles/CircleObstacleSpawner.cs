using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkinColorRunner.Obstacles
{
    public class CircleObstacleSpawner : MonoBehaviour
    {
        #region Variables
        [Header("Ball Prefab")]
        [Space(3)]
        [SerializeField] private GameObject ballPrefab;

        [Space(5)]

        [Header("Transforms")]
        [SerializeField] private Transform bigBallsContainer;
        [SerializeField] private Transform smallBallsContainer;

        [Space(5)]

        [Header("Materials")]
        [Space(3)]
        [SerializeField] private Material[] materials;

        // readonly properties
        private readonly int ballAmount = 8;
        private readonly float bigRadius = 2.4f;
        private readonly float smallRadius = 1.5f;
        private readonly Vector3 bigScale = new(.9f, .9f, .9f);
        private readonly Vector3 smallScale = new(.65f, .65f, .65f);
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            // spawn balls at big container
            SpawnBalls(bigBallsContainer, bigRadius, bigScale);

            // spawn balls at small container
            SpawnBalls(smallBallsContainer, smallRadius, smallScale);
        }

        private void SpawnBalls(Transform container, float radius, Vector3 scale)
        {
            // the center of circle
            Vector3 center = container.position;

            for (int i = 0; i < ballAmount; i++)
            {
                // distance around the circle
                float radian = 2 * Mathf.PI / ballAmount * i;

                // get the vector direction
                float vertical = Mathf.Sin(radian);
                float horizontal = Mathf.Cos(radian);

                // ball position direction
                Vector3 direction = new(horizontal, 0f, vertical);

                // calculate ball position
                Vector3 ballPosition = center + direction * radius;

                // instantiate ball
                GameObject ball = Instantiate(ballPrefab, ballPosition, Quaternion.identity);
                ball.transform.SetParent(container);
                ball.transform.localScale = scale; // scale ball
                SetColor(i, ball); // set color of ball
            }
        }

        private void SetColor(int i, GameObject ball)
        {
            // get renderer
            MeshRenderer ballRenderer = ball.GetComponent<MeshRenderer>();

            if (i<4)
            {
                // blue
                ballRenderer.material = materials[0];
            }
            else
            {
                // pink
                ballRenderer.material = materials[1];
            }
        }
    }
}