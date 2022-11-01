using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Алгоритм для расширенного режима.
    /// </summary>
    [CreateAssetMenu(fileName = "AdvancedDeleteStrategy", menuName = "DeleteStrategy/Advanced")]
    public class AdvancedDeleteStrategy : DeleteStratergy
    {
        [SerializeField]
        private int scoreForLine;

        public override List<DeleteResult> DeleteBlocks(IGameSpace space)
        {
            var saved = new HashSet<int>();
            var result = new List<DeleteResult>();

            for (int y = 0; y < space.Height; y++)
            {
                int x = 0;
                for (; x < space.Width; x++)
                {
                    if (space.Cells[x, y] == null)
                    {
                        break;
                    }
                }
                if (x == space.Width)
                {
                    saved.Add(y);
                }
            }

            foreach(var y in saved)
            {
                if (saved.Contains(y - 1) || saved.Contains(y + 1))
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