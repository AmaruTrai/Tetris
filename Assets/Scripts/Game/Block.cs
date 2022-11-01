using System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// ������� ������� �������� ������������.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class Block : MonoBehaviour
    {
        public event Action<Block> OnReturn;

        [SerializeField]
        private Animator animator;

        [NonSerialized]
        public Shape Shape;

        public void Return()
        {
            OnReturn?.Invoke(this);
        }

        public void SetColor(string color) {
            animator.Play(color);
        }
    }

}
