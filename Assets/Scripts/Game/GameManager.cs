using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game
{
    [RequireComponent(typeof(ToolSet))]
    [RequireComponent(typeof(InputController))]
    public class GameManager : MonoBehaviour
    {
        private enum State
        {
            ToolControl  = 0,
            FallLines = 1
        }

        [Header("Настройки игровой логики")]
        [SerializeField]
        [Tooltip("Алгоритм удаления строк и подсчета, полученный очков.")]
        private DeleteStratergy deleteStratergy;

        [Header("Настройки скорости опадения")]
        [SerializeField]
        [Tooltip("Стандартная скорость опадения блоков.")]
        private float speed;
        [SerializeField]
        [Tooltip("Множитель скорости опадения блоков во время активации ускорения.")]
        private float speedBoost;

        [Header("Элементы UI")]
        [SerializeField]
        private Text scoreText;
        [SerializeField]
        private GameObject EndPanel;

        [Header("Компоненты")]
        [SerializeField]
        private ToolSet toolManager;
        [SerializeField]
        private InputController inputController;

        private Vector2Int spawnPosition;
        private float currentSpeed;
        private int score;
        private BaseSpace space;
        private FallingTool currentTool;

        private Stack<int> deletedLines;
        private int? currentFallIndex;

        private State state;
        private bool isGamePaused;

        private int Score
        {
            get 
            {
                return score;
            }
            set 
            {
                score = value;
                scoreText.text = $"Score: {score}";
            }
        }

        [Inject]
        private void Construct(BaseSpace space) {
            this.space = space;
        }

        private void Start()
        {
            Score = 0;
            currentSpeed = speed;
            isGamePaused = true;
            state = State.ToolControl;
            deletedLines = new Stack<int>();

            inputController.OnMove += Move;
            inputController.OnRotate += Rotate;
            inputController.OnSetSpeedBoostState += SetSpeedBoost;

            foreach (var tool in toolManager.GetTools())
            {
                tool.OnLanding += OnShapeLanded;
            }

            ChangeTool();
        }

        private void Update()
        {
            if (isGamePaused)
            {
                return;
            } else if (state == State.ToolControl)
            {
                if (currentTool.State == FallingState.Idle)
                {
                    currentTool.TryStartFall();
                }
                else if (currentTool.State == FallingState.Falling)
                {
                    currentTool.Fall(currentSpeed);
                }
            } else if (state == State.FallLines)
            {
                UpdateBlocksFall();
            }
        }

        private void OnShapeLanded(Shape shape) {
            shape.CleanUp();
            ChechGameSpace();

            for(int x = 0; x < space.Width; x++)
            {
                if (space.Cells[x, space.Height - 1] != null)
                {
                    GameEnd();
                    return;
                }
            }

            ChangeTool();
        }

        private void ChangeTool()
        {
            if (currentTool != null)
            {
                currentTool.gameObject.SetActive(false);
                currentTool = null;
            }


            if (toolManager.TryGetTool(out var newTool))
            {
                newTool.gameObject.SetActive(true);
                newTool.transform.SetParent(space.transform);
                currentTool = newTool;

                if (currentTool.State == FallingState.Landed)
                {
                    currentTool.ResetState();
                }
                spawnPosition = new Vector2Int((space.Width - currentTool.Size.x + 1) / 2, space.Height - currentTool.Size.y);
                currentTool.SetPosition(spawnPosition);
            }
        }

        /// <summary>
        /// Анализ игрового поля на наличие линий подлежащих удалению.
        /// </summary>
        private void ChechGameSpace()
        {
            var toDeletion = deleteStratergy.DeleteBlocks(space);

            foreach (var line in toDeletion)
            {
                Score += line.score;
                deletedLines.Push(line.lineIndex);
            }

            if (deletedLines.TryPop(out int newIndex))
            {
                state = State.FallLines;
                currentFallIndex = newIndex;
                NewBlocksFall();
            }
        }

        /// <summary>
        /// Обработка логики опадения блоков.
        /// </summary>
        private void NewBlocksFall()
        {
            for (int y = currentFallIndex.Value + 1; y < space.Height; y++)
            {
                for (int x = 0; x < space.Width; x++)
                {
                    if (space.Cells[x, y] != null && space.Cells[x, y - 1] == null)
                    {
                        space.Cells[x, y - 1] = space.Cells[x, y];
                        space.Cells[x, y] = null;
                    }
                }
            }
        }

        /// <summary>
        /// Обработка плавного опадения блоков после уничтожения линий.
        /// </summary>
        private void UpdateBlocksFall()
        {
            bool allBlockFall = true;
            for (int y = currentFallIndex.Value; y < space.Height; y++)
            {
                for (int x = 0; x < space.Width; x++)
                {
                    if (
                        space.TryGetBlock(x, y, out var block) &&
                        block.Shape == null &&
                        !BlockFall(x, y, currentSpeed)
                    ) {
                        allBlockFall = false;
                    }
                }
            }

            if (allBlockFall)
            {
                if (deletedLines.TryPop(out int newIndex))
                {
                    currentFallIndex = newIndex;
                    NewBlocksFall();
                } else
                {
                    state = State.ToolControl;
                    currentFallIndex = null;
                }
            }
        }

        /// <summary>
        /// Логика плавного опадения блока.
        /// </summary>
        private bool BlockFall(int x, int y, float speed)
        {
            var block = space.Cells[x, y];

            if (block == null)
            {
                return false;
            }

            var offset = y - block.transform.localPosition.y;
            var delta = -Time.deltaTime * speed;

            if (offset >= 0)
            {
                block.transform.localPosition = new Vector3(block.transform.localPosition.x, y);
                return true;
            }
            else
            {
                block.transform.localPosition = new Vector3(block.transform.localPosition.x, block.transform.localPosition.y + delta);
                return false;
            }
        }

        /// <summary>
        /// Активирует/деактивирует ускорение.
        /// </summary>
        private void SetSpeedBoost(bool active)
        {
            currentSpeed = active ? speed * speedBoost : speed;
        }

        /// <summary>
        /// Активирует перемещение текущего инструмента.
        /// </summary>
        private void Move(Vector2Int direction)
        {
            if (state != State.ToolControl || isGamePaused)
            {
                return;
            }

            currentTool.TryMove(direction);
        }

        /// <summary>
        /// Активирует поворот текущего инструмента.
        /// </summary>
        private void Rotate(Vector2Int direction)
        {
            if (state != State.ToolControl || isGamePaused)
            {
                return;
            }

            var isLeftRotation = direction == Vector2Int.left;
            currentTool.TryRotate(isLeftRotation);
        }

        private void GameEnd()
        {
            isGamePaused = true;
            EndPanel.SetActive(true);
        }

        public void SetPause(bool pause)
        {
            isGamePaused = pause;
        }
    }

}
