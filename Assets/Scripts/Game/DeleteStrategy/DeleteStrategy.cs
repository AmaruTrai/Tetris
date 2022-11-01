using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public struct DeleteResult
    {
        public int lineIndex;
        public int score;
    }

    /// <summary>
    /// Алгоритм для анализа игрового поля и удаления блоков.
    /// </summary>
    [Serializable]
    public abstract class DeleteStratergy : ScriptableObject
    {
        public abstract List<DeleteResult> DeleteBlocks(IGameSpace space);
    }

}

