using UnityEngine;

namespace Game
{
    /// <summary>
    /// »гровое пространство с расширенными правилами.
    /// </summary>
    public class AdvancedSpace : BaseSpace
    {
        public override bool TryGetBlock(int x, int y, out Block block)
        {
            if (TryGetValidCell(x, y, out var cell) && cells[cell.x, cell.y] != null)
            {
                block = cells[cell.x, cell.y];
                return true;
            }

            block = default;
            return false;
        }

        /// <summary>
        /// ¬озвращает переданные координаты если наход€тс€ в рамках игрового пол€.
        /// ≈сли координаты выход€т за рамки игрового пол€, переносит их на противоположную сторону.
        /// </summary>
        public override bool TryGetValidCell(int x, int y, out Vector2Int cell)
        {
            if (IsCellValid(x, y))
            {
                cell = new Vector2Int(x, y);
                return true;
            } else if (y < 0 || y >= Height)
            {
                cell = default;
                return false;
            } else 
            {
                int newX = x;
                if (newX >= Width) 
                {
                    newX -= Width;
                } else if (newX < 0) 
                {
                    newX += Width;
                }

                cell = new Vector2Int(newX, y);
            }

            return true;
        }
    }
}