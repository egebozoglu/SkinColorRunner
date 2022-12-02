using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkinColorRunner.Obstacles
{
    public class ColorChangingObstacle : MonoBehaviour
    {
        #region Variables
        [Header("Materials")]
        [Space(3)]
        [SerializeField] private List<Material> materials = new();

        [Space(5)]

        [Header("Sticks")]
        [Space(3)]
        [SerializeField] private List<GameObject> sticks = new();
        #endregion

        // Update is called once per frame
        void Update()
        {

        }
        

        private IEnumerator ChangeColors()
        {
            yield return new WaitForSeconds(1.5f);

            // continue
        }

        private void SetColors()
        {
            for (int i = 0; i < sticks.Count; i++)
            {
                MeshRenderer stickRenderer = sticks[i].GetComponent<MeshRenderer>();
                stickRenderer.material = materials[i];
            }
        }
    }
}