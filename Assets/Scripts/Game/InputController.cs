using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    /// <summary>
    /// Класс для обработки ввода и представления его в удобном виде.
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class InputController : MonoBehaviour
    {
        public event Action<Vector2Int> OnMove;
        public event Action<Vector2Int> OnRotate;
        public event Action<bool> OnSetSpeedBoostState;

        [SerializeField]
        [Tooltip(
            "Время, по истечению которого," +
            " будет повторно вызвано событие на передвижение фигуры при зажатой клавише."
        )]
        private float moveTimeOut;
        [SerializeField]
        [Tooltip(
            "Время, по истечению которого," +
            " будет повторно вызвано событие на поворот фигуры при зажатой клавише."
        )]
        private float rotateTimeOut;

        private bool isMoving;
        private Vector2Int currentMoveDirection;
        private Coroutine moveCoroutine;

        private bool isRotating;
        private Vector2Int currentRotateDirection;
        private Coroutine rotateCoroutine;

        private IEnumerator MoveCoroutine()
        {
            while (isMoving)
            {
                OnMove?.Invoke(currentMoveDirection);
                yield return new WaitForSeconds(moveTimeOut);
            }
        }

        private void Move(Vector2Int direction, InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (isMoving && moveCoroutine != null)
                {
                    StopCoroutine(moveCoroutine);
                }

                isMoving = true;
                currentMoveDirection = direction;
                moveCoroutine = StartCoroutine(MoveCoroutine());
            } else if (
                context.canceled &&
                isMoving &&
                moveCoroutine != null &&
                currentMoveDirection == direction
            ) {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
                isMoving = false;
            }
        }

        /// <summary>
        /// Вызывается с помощью PlayerInput.
        /// </summary>
        public void MoveRight(InputAction.CallbackContext context)
        {
            Move(Vector2Int.right, context);
        }

        /// <summary>
        /// Вызывается с помощью PlayerInput.
        /// </summary>
        public void MoveLeft(InputAction.CallbackContext context)
        {
            Move(Vector2Int.left, context);
        }

        private IEnumerator RotateCoroutine()
        {
            while (isRotating)
            {
                OnRotate?.Invoke(currentRotateDirection);
                yield return new WaitForSeconds(rotateTimeOut);
            }
        }

        private void Rotate(Vector2Int direction, InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (isRotating && rotateCoroutine != null)
                {
                    StopCoroutine(rotateCoroutine);
                }

                isRotating = true;
                currentRotateDirection = direction;
                rotateCoroutine = StartCoroutine(RotateCoroutine());
            }
            else if (
                context.canceled &&
                isRotating &&
                rotateCoroutine != null &&
                currentRotateDirection == direction
            )
            {
                StopCoroutine(rotateCoroutine);
                rotateCoroutine = null;
                isRotating = false;
            }
        }

        /// <summary>
        /// Вызывается с помощью PlayerInput.
        /// </summary>
        public void RotateRight(InputAction.CallbackContext context)
        {
            Rotate(Vector2Int.right, context);
        }

        /// <summary>
        /// Вызывается с помощью PlayerInput.
        /// </summary>
        public void RotateLeft(InputAction.CallbackContext context)
        {
            Rotate(Vector2Int.left, context);
        }

        /// <summary>
        /// Вызывается с помощью PlayerInput.
        /// </summary>
        public void SetActiveSpeedBoost(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnSetSpeedBoostState?.Invoke(true);
            } else if (context.canceled)
            {
                OnSetSpeedBoostState?.Invoke(false);
            }
        }
    }

}