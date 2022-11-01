using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tools
{
    public class StartLevelButton : MonoBehaviour
    {
        [SerializeField]
        private string sceneId;

        public void StartLevel()
        {
            SceneManager.LoadScene(sceneId);
        }

    }

}