using UnityEngine;

namespace Game
{
    /// <summary>
    /// ��������� ��� ������ � ������� �������������.
    /// </summary>
    public interface IGameSpace
    {
        /// <summary>
        /// ������� ����.
        /// </summary>
        public Block[,] Cells { get; }
        public int Height { get; }
        public int Width { get; }

        /// <summary>
        /// ���������� ��������� �� ����� ������ �������� ������������.
        /// </summary>
        public bool IsCellValid(int x, int y);

        /// <summary>
        /// ���������� ��������� �� ����� ������ �������� ������������.
        /// </summary>
        public bool IsCellValid(Vector2Int position);

        /// <summary>
        /// ���������� ����, ����������� � ��������� ����������� ���� ����� ����������.
        /// </summary>
        public bool TryGetBlock(int x, int y, out Block block);

        /// <summary>
        /// ���������� ����, ����������� � ��������� ����������� ���� ����� ����������.
        /// </summary>
        public bool TryGetBlock(Vector2Int position, out Block block);

        /// <summary>
        /// �������������� ���������� ����������.
        /// </summary>
        public bool TryGetValidCell(int x, int y, out Vector2Int cell);

        /// <summary>
        /// �������������� ���������� ����������.
        /// </summary>
        public bool TryGetValidCell(Vector2Int position, out Vector2Int cell);
    }

}
