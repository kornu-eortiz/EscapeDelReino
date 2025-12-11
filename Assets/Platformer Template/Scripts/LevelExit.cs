using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class LevelExit : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("Tiempo de espera antes de cargar el siguiente nivel")]
        public float delayBeforeLoad = 1f;

        [Tooltip("Desactivar al jugador mientras carga")]
        public bool disablePlayerOnTouch = true;

        [Header("Fallback (si no hay LevelManager)")]
        [Tooltip("Nombre de la siguiente escena si no existe LevelManager")]
        public string nextSceneName = "";

        private bool triggered = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (triggered) return;

            if (other.CompareTag("Player"))
            {
                triggered = true;

                // Reproducir sonido de teletransporte
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlayTeleport();
                }

                // Desactivar movimiento del jugador
                if (disablePlayerOnTouch)
                {
                    PlayerController player = other.GetComponent<PlayerController>();
                    if (player == null)
                    {
                        player = other.GetComponentInParent<PlayerController>();
                    }
                    if (player != null)
                    {
                        player.enabled = false;
                    }
                }

                // Cargar siguiente nivel después del delay
                StartCoroutine(LoadNextLevel());
            }
        }

        private System.Collections.IEnumerator LoadNextLevel()
        {
            yield return new WaitForSeconds(delayBeforeLoad);

            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.NextLevel();
            }
            else if (!string.IsNullOrEmpty(nextSceneName))
            {
                // Fallback: cargar escena directamente
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("LevelExit: No se encontró LevelManager y no hay nextSceneName configurado!");
            }
        }
    }
}
