using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    public class ResolutionControl : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        private void Awake()
        {
            #if UNITY_ANDROID
                animator.Play("Portrait");
            #endif
        }
    }

}