using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [Header("Level Configuration")]
        [Tooltip("Lista de nombres de escenas en orden (Level1, Level2, etc.)")]
        public string[] levelScenes;

        [Tooltip("Nombre de la escena del menú principal")]
        public string mainMenuScene = "MainMenu";

        [Tooltip("Nombre de la escena de victoria/créditos")]
        public string victoryScene = "Victory";

        private int currentLevelIndex = 0;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartGame()
        {
            currentLevelIndex = 0;
            if (levelScenes != null && levelScenes.Length > 0)
            {
                SceneManager.LoadScene(levelScenes[0]);
            }
            else
            {
                Debug.LogWarning("LevelManager: No hay niveles configurados!");
            }
        }

        public void NextLevel()
        {
            currentLevelIndex++;

            if (currentLevelIndex < levelScenes.Length)
            {
                SceneManager.LoadScene(levelScenes[currentLevelIndex]);
            }
            else
            {
                Victory();
            }
        }

        public void RestartLevel()
        {
            if (levelScenes != null && currentLevelIndex < levelScenes.Length)
            {
                SceneManager.LoadScene(levelScenes[currentLevelIndex]);
            }
        }

        public void Victory()
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayVictory();
            }

            if (!string.IsNullOrEmpty(victoryScene))
            {
                SceneManager.LoadScene(victoryScene);
            }
            else
            {
                SceneManager.LoadScene(mainMenuScene);
            }
        }

        public void ReturnToMenu()
        {
            currentLevelIndex = 0;
            SceneManager.LoadScene(mainMenuScene);
        }

        public int GetCurrentLevelIndex()
        {
            return currentLevelIndex;
        }
    }
}
