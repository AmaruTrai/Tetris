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
    /// �������� ��� ������� �������� ���� � �������� ������.
    /// </summary>
    [Serializable]
    public abstract class DeleteStratergy : ScriptableObject
    {
        public abstract List<DeleteResult> DeleteBlocks(IGameSpace space);
    }

}

