using UnityEngine;

namespace Game
{
    /// <summary>
    /// Ѕазовый класс дл€ описани€ игрового пространства.
    /// </summary>
    public abstract class BaseSpace : MonoBehaviour, IGameSpace
    {
        [Min(1)]
        [SerializeField]
        protected int width = 1;
        [Min(1)]
        [SerializeField]
        protected int height = 1;

        protected Block[,] cells;

        public int Height => height;
        public int Width => width;

        public Block[,] Cells => cells;

        public virtual bool IsCellValid(int x, int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }

        public virtual bool IsCellValid(Vector2Int position)
        {
            return IsCellValid(position.x, position.y);
        }

        public abstract bool TryGetBlock(int x, int y, out Block block);

        public virtual bool TryGetBlock(Vector2Int position, out Block block)
        {
            return TryGetBlock(position.x, position.y, out block);
        }

        public abstract bool TryGetValidCell(int x, int y, out Vector2Int cell);

        public virtual bool TryGetValidCell(Vector2Int position, out Vector2Int cell)
        {
            return TryGetValidCell(position.x, position.y, out cell);
        }

        protected virtual void OnAwake() 
        {
            cells = new Block[width, height];
        }

        private void Awake() 
        {
            OnAwake();
        }

        private void OnDrawGizmos()
        {
            for (int x = 0; x <= width; x++)
            {
                float xPos = x;
                var startPos = new Vector3(xPos, 0, 0) + transform.position;
                var endPos = new Vector3(xPos, height, 0) + transform.position;
                Gizmos.color = Color.red;
                Gizmos.DrawLine(startPos, endPos);
            }

            for (int y = 0; y <= height; y++)
            {
                float yPos = y;
                var startPos = new Vector3(0, yPos, 0) + transform.position;
                var endPos = new Vector3(width, yPos, 0) + transform.position;
                Gizmos.color = Color.red;
                Gizmos.DrawLine(startPos, endPos);
            }
        }
    }
}