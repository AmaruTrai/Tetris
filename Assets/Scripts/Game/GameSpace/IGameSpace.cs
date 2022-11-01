using UnityEngine;

namespace Game
{
    /// <summary>
    /// »нтерфейс дл€ работы с игровым пространством.
    /// </summary>
    public interface IGameSpace
    {
        /// <summary>
        /// »гровое поле.
        /// </summary>
        public Block[,] Cells { get; }
        public int Height { get; }
        public int Width { get; }

        /// <summary>
        /// ќпредел€ет находитс€ ли точка внутри игрового пространства.
        /// </summary>
        public bool IsCellValid(int x, int y);

        /// <summary>
        /// ќпредел€ет находитс€ ли точка внутри игрового пространства.
        /// </summary>
        public bool IsCellValid(Vector2Int position);

        /// <summary>
        /// ¬озвращает блок, наход€щийс€ в указанных координатах если такой существует.
        /// </summary>
        public bool TryGetBlock(int x, int y, out Block block);

        /// <summary>
        /// ¬озвращает блок, наход€щийс€ в указанных координатах если такой существует.
        /// </summary>
        public bool TryGetBlock(Vector2Int position, out Block block);

        /// <summary>
        /// “рансформирует переданные координаты.
        /// </summary>
        public bool TryGetValidCell(int x, int y, out Vector2Int cell);

        /// <summary>
        /// “рансформирует переданные координаты.
        /// </summary>
        public bool TryGetValidCell(Vector2Int position, out Vector2Int cell);
    }

}
