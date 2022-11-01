using System;
using UnityEngine;

namespace Game
{
    public enum FallingState
    {
        Idle = 0,
        Falling = 1,
        Landed = 2,
    }

    /// <summary>
    /// Контейнер для реализации логики опадения фигуры.
    /// </summary>
    [RequireComponent(typeof(Shape))]
    public class FallingTool : MonoBehaviour
    {
        /// <summary>
        /// Событие вызываемое в момент, когда фигура больше не может двигаться вниз.
        /// </summary>
        public event Action<Shape> OnLanding;

        [SerializeField]
        private Shape shape;

        private FallingState state;

        public FallingState State => state;
        public Vector2Int Size => shape.Size;

        private void Awake()
        {
            state = FallingState.Idle;
        }

        public bool TryStartFall()
        {
            var direction = Vector2Int.down;
            if (!shape.IsMovePossible(direction))
            {
                OnLanding?.Invoke(shape);
                return false;
            }

            shape.SetPosition(shape.Position + direction);
            state = FallingState.Falling;
            return true;
        }

        public void Fall(float speed) {
            if (state != FallingState.Falling)
            {
                return;
            }

            var offset = shape.Position.y - transform.localPosition.y;
            var delta = - Time.deltaTime * speed;

            if (offset >= 0)
            {
                if (shape.IsMovePossible(Vector2Int.down))
                {
                    shape.SetPosition(shape.Position + Vector2Int.down);
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + delta);
                } else
                {
                    state = FallingState.Landed;
                    transform.localPosition = new Vector3(transform.localPosition.x, shape.Position.y);
                    OnLanding?.Invoke(shape);
                }
            } else
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + delta);
            }
        }

        private void UpdateShapeBlocksPositions()
        {
            foreach (var cell in shape.GetValidBlocks())
            {
                var spacePosition = cell.Value;
                var block = cell.Key;
                var localPos = spacePosition - shape.Position;
                block.transform.localPosition = new Vector3(localPos.x, localPos.y);
            }
        }

        public bool TryMove(Vector2Int direction)
        {
            if (!shape.IsMovePossible(direction))
            {
                return false;
            }
            shape.SetPosition(shape.Position + direction);
            UpdateShapeBlocksPositions();
            transform.localPosition = new Vector3(shape.Position.x, transform.localPosition.y);
            return true;
        }

        public void SetPosition(Vector2Int position)
        {
            shape.SetPosition(position);
            transform.localPosition = new Vector3(position.x, position.y);
        }

        public void ResetState()
        {
            shape.CleanUp();
            shape.ConfigureMap();
            state = FallingState.Idle;
        }

        public bool TryRotate(bool isLeftRotation = false)
        {
            if (shape.TryRotate(isLeftRotation)) {
                UpdateShapeBlocksPositions();
                return true;
            }
            return false;
        }
    }
}
