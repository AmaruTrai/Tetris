using UnityEngine;

namespace Game
{
    /// <summary>
    /// ������� ������������ � ������������ ���������.
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
        /// ���������� ���������� ���������� ���� ��������� � ������ �������� ����.
        /// ���� ���������� ������� �� ����� �������� ����, ��������� �� �� ��������������� �������.
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