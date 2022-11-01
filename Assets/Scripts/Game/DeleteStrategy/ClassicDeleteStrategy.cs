using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Алгоритм для классического режима.
    /// </summary>
    [CreateAssetMenu(fileName = "ClassicDeleteStrategy", menuName = "DeleteStrategy/Classic")]
    public class ClassicDeleteStrategy : DeleteStratergy
    {
        [SerializeField]
        private int scoreForLine;

        public override List<DeleteResult> DeleteBlocks(IGameSpace space)
        {
            var result = new List<DeleteResult>();

            for (int y = 0; y < space.Height; y++)
            {
                int x = 0;
                for ( ; x < space.Width; x++)
                {
                    if (space.Cells[x, y] == null)
                    {
                        break;
                    }
                }
                if (x == space.Width)
                {
                    result.Add(new DeleteResult { lineIndex = y, score = scoreForLine });
                    for (int i = 0; i < space.Width; i++)
                    {
                        var block = space.Cells[i, y];
                        space.Cells[i, y] = null;
                        block.Return();
                    }
                }
            }

            return result;
        }
    }

}