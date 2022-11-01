using Array2DEditor;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
    /// <summary>
    /// Инструмент для организации блоков в различные формы.
    /// </summary>
    public class Shape : MonoBehaviour
    {

        [SerializeField]
        [Tooltip("Определяет положение блоков внутри формы.")]
        private Array2DBool shapeMatrix;

        [SerializeField]
        [Tooltip("Цвет блоков, используемый внутри фигуры (доступные цвета смотреть анимации блоков).")]
        string blockColor = "Color1";

        private BlockPool pool;
        private Block[,] map;
        private IGameSpace space;
        private Vector2Int position;
        private Vector2Int size;

        public Vector2Int Size => size;
        public Vector2Int Position => position;

        [Inject]
        private void Construct(BlockPool pool, BaseSpace space)
        {
            this.pool = pool;
            this.space = space;
        }

        private void Awake()
        {
            position = -Vector2Int.one;

            if (shapeMatrix == null) {
                shapeMatrix = new Array2DBool();
            }

            size = shapeMatrix.GridSize;
            map = new Block[Size.x, Size.y];
        }

        private void Start()
        {
            ConfigureMap();
        }

        private Block GetBlockFromPool(int xPos, int yPos)
        {
            var block = pool.GetBlock();
            block.gameObject.SetActive(true);
            block.Shape = this;
            block.transform.SetParent(transform);
            block.transform.localPosition = new Vector3(xPos, Size.y - yPos - 1);
            block.SetColor(blockColor);
            return block;
        }

        public void ConfigureMap()
        {
            for (int x = 0; x < Size.x; x++) {
                for (int y = 0; y < Size.y; y++) {
                    if (shapeMatrix.GetCell(x, y)) {
                        map[x, Size.y - y - 1] = GetBlockFromPool(x, y);
                    }
                }
            }
        }

        public void CleanUp()
        {
            foreach(var block in map) {
                if (block != null) {
                    block.Shape = null;
                    block.transform.SetParent(transform.parent);
                }
            }
            size = shapeMatrix.GridSize;
            map = new Block[Size.x, Size.y];
        }

        public bool IsMovePossible(Vector2Int direction)
        {
            if (space == null) {
                Debug.Log("Null space");
                return false;
            }

            for (int x = 0; x < Size.x; x++) {
                for (int y = 0; y < Size.y; y++) {
                    if (map[x, y] != null) {
                        var next = position + direction + new Vector2Int(x , y);
                        if (
                            !space.TryGetValidCell(next, out var cell) ||
                            (space.TryGetBlock(cell, out var block) && block.Shape != this)
                        ) {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public void SetPosition(Vector2Int position)
        {
            if (space == null) {
                Debug.Log("Null space");
                return;
            }

            if (position.x >= space.Width) {
                position = new Vector2Int(0, position.y);
            } else if (position.x < 0) {
                position = new Vector2Int(space.Width - 1, position.y);
            }

            if (space.IsCellValid(this.position)) {
                for (int x = 0; x < Size.x; x++) {
                    for (int y = 0; y < Size.y; y++) {
                        if (map[x, y] != null) {
                            var offset = new Vector2Int(x, y);
                            var current = offset + this.position;
                            var next = offset + position;

                            if (
                                space.TryGetValidCell(current, out var currentCell) &&
                                space.Cells[currentCell.x, currentCell.y] == map[x, y]
                            ) {
                                space.Cells[currentCell.x, currentCell.y] = null;
                            }

                            if (space.TryGetValidCell(next, out var nextCell)) {
                                space.Cells[nextCell.x, nextCell.y] = map[x, y];
                            }
                        }
                    }
                }
            } else {
                for (int x = 0; x < Size.x; x++) {
                    for (int y = 0; y < Size.y; y++) {
                        if (map[x, y] != null) {
                            var next = new Vector2Int(x, y) + position;
                            if (space.TryGetValidCell(next, out var nextCell)) {
                                space.Cells[nextCell.x, nextCell.y] = map[x, y];
                            }
                        }
                    }
                }
            }

            this.position = position;
        }

        public bool TryRotate(bool isLeftRotation = false) {
            if (space == null) {
                Debug.Log("Null space");
                return false;
            }

            var newMap = new Block[Size.y, Size.x];

            if (isLeftRotation) {
                for (int x = 0; x < Size.x; x++) {
                    for (int y = 0; y < Size.y; y++) {
                        newMap[y, Size.x - x - 1] = map[x, y];
                    }
                }
            } else {
                for (int x = 0; x < Size.x; x++) {
                    for (int y = 0; y < Size.y; y++) {
                        newMap[y, x] = map[x, Size.y - 1 - y];
                    }
                }
            }

            for (int x = 0; x < newMap.GetLength(0); x++) {
                for (int y = 0; y < newMap.GetLength(1); y++) {
                    if (newMap[x, y] != null) {
                        var next = position + new Vector2Int(x, y);
                        if (
                            !space.TryGetValidCell(next, out var newCell) ||
                            (space.TryGetBlock(newCell, out var block) && block.Shape != this)
                        ) {
                            return false;
                        }
                    }
                }
            }

            for (int x = 0; x < Size.x; x++) {
                for (int y = 0; y < Size.y; y++) {
                    if (map[x, y] != null) {
                        var next = position + new Vector2Int(x, y);
                        if (
                            !space.TryGetValidCell(next, out var newCell) ||
                            (space.TryGetBlock(newCell, out var block))
                        ) {
                            space.Cells[newCell.x, newCell.y] = null;
                        }
                    }
                }
            }

            for (int x = 0; x < newMap.GetLength(0); x++) {
                for (int y = 0; y < newMap.GetLength(1); y++) {
                    var next = position + new Vector2Int(x, y);
                    if (newMap[x, y] != null && space.TryGetValidCell(next, out var newCell)) {
                        space.Cells[newCell.x, newCell.y] = newMap[x, y];
                        newMap[x, y].transform.localPosition = new Vector3(x, y);
                    }
                }
            }

            map = newMap;
            size = new Vector2Int(map.GetLength(0), map.GetLength(1));

            return true;
        }

        public IEnumerable<KeyValuePair<Block, Vector2Int>> GetValidBlocks()
        {
            for (int x = 0; x < Size.x; x++) {
                for (int y = 0; y < Size.y; y++) {
                    var next = new Vector2Int(x, y) + position;
                    if (map[x, y] != null && space.TryGetValidCell(next, out var nextCell)) {
                        yield return new KeyValuePair<Block, Vector2Int>(map[x, y], nextCell);
                    }
                }
            }
        }
    }
}

