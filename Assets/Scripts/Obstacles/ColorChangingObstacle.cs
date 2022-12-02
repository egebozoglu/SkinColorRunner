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

        void Start()
        {
            StartCoroutine(ChangeColors());
        }

        private IEnumerator ChangeColors()
        {
            while (true)
            {
                yield return new WaitForSeconds(.1f);

                // first reorder material list and set sticks' colors again
                ReorderMaterials();
                SetColors();
            }
        }

        private void SetColors()
        {
            for (int i = 0; i < sticks.Count; i++)
            {
                MeshRenderer stickRenderer = sticks[i].GetComponent<MeshRenderer>();
                stickRenderer.material = materials[i];
            }
        }

        private void ReorderMaterials()
        {
            List<Material> newMaterials = new();

            for (int i = 1; i < materials.Count; i++)
            {
                newMaterials.Add(materials[i]);
            }
            newMaterials.Add(materials[0]);

            materials = newMaterials;
        }
    }
}