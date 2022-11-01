using UnityEngine;

namespace Game
{
    /// <summary>
    /// »гровое пространство с классическими правилами.
    /// </summary>
    public class ClassicSpace : BaseSpace
    {
        public override bool TryGetBlock(int x, int y, out Block block)
        {
            if (IsCellValid(x, y) && cells[x, y] != null)
            {
                block = cells[x, y];
                return true;
            }

            block = null;
            return false;
        }

        /// <summary>
        /// ¬озвращает переданные координаты если наход€тс€ в рамках игрового пол€.
        /// </summary>
        public override bool TryGetValidCell(int x, int y, out Vector2Int cell)
        {
            if (IsCellValid(x, y)) {
                cell = new Vector2Int(x, y);
                return true;
            }

            cell = default;
            return false;
        }
    }
}