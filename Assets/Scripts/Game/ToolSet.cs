using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Game
{
    /// <summary>
    /// Контейнер управляющий доступными фигурами.
    /// </summary>
    public class ToolSet : MonoBehaviour
    {
        [Serializable]
        private struct ToolContainer {
            [Range(0, 100)]
            [Tooltip("Шанс выпадения фигуры.")]
            public int spawnСhance;
            public FallingTool tool;
        }

        [SerializeField]
        [Tooltip("Список доступных инструментов.")]
        private List<ToolContainer> toolsPrefab;

        private List<ToolContainer> tools;
        private int max;

        [Inject]
        private void Construct(DiContainer container)
        {
            tools = new List<ToolContainer>();
            foreach (var toolPrefab in toolsPrefab)
            {
                var tool = container.InstantiatePrefabForComponent<FallingTool>(toolPrefab.tool);
                tool.gameObject.SetActive(false);
                tools.Add(new ToolContainer { spawnСhance = toolPrefab.spawnСhance, tool = tool });
                max += toolPrefab.spawnСhance;
            }
        }

        public bool TryGetTool(out FallingTool tool) {
            tool = null;
            if (tools == null || tools.Count == 0) {
                return false;
            }

            int chance = UnityEngine.Random.Range(0, max);
            int chanceSum = 0;
            foreach(var container in tools) {
                chanceSum += container.spawnСhance;
                if (chance <= chanceSum) {
                    tool = container.tool;
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<FallingTool> GetTools() {
            return tools.Select(t => t.tool);
        }
    }

}