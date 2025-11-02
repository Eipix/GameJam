using System;
using UnityEngine;

namespace Common
{
    public class SetVisibility : MonoBehaviour
    {
        [SerializeField] private bool visible = true;

        private void Awake()
        {
            SetRenderersVisibility(gameObject, visible);
        }

        private void SetRenderersVisibility(GameObject target, bool isVisible)
        {
            Renderer[] renderers = target.GetComponents<Renderer>();
            foreach (Renderer rend in renderers)
            {
                rend.enabled = isVisible;
            }

            for (int i = 0; i < target.transform.childCount; i++)
            {
                Transform child = target.transform.GetChild(i);
                SetRenderersVisibility(child.gameObject, isVisible);
            }
        }
    }
}
