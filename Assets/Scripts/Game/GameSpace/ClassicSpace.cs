using UnityEngine;

namespace Game
{
    /// <summary>
    /// ������� ������������ � ������������� ���������.
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
        /// ���������� ���������� ���������� ���� ��������� � ������ �������� ����.
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