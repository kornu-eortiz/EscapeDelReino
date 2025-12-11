using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class UIManager : MonoBehaviour
    {
        public enum ScreenState { Game, Pause, Lose }

        public static UIManager Instance;

        [Header("Components")]
        public GameObject gameScreen, pauseScreen, loseScreen;

        public ScreenState screenState;

        public Image hpBar;

        public GameObject mobileUI;

        void SingletonInit()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }

        void Awake()
        {
            SingletonInit();
        }

        private void Start()
        {
#if UNITY_ANDROID || UNITY_IOS
            mobileUI.SetActive(true); //if mobile platform, mobile UI'll turn on
#endif

            // Verificar que GameManager existe antes de actualizar HP
            if (GameManager.Instance != null && GameManager.Instance.playerStats != null)
            {
                UpdateHP(GameManager.Instance.playerStats.statsData.HP); //clear UI
            }
        }

        //Player hp update method
        public void UpdateHP(float hpValue)
        {
            hpBar.fillAmount = hpValue / 100;
        }
        //Method to start new game from menu
        public void NewGame()
        {
            SoundManager.Instance.PlayConfirm(); // Sonido de confirmar
            Time.timeScale = 1; //return time
            
            // Usar LevelManager para reiniciar el nivel actual
            if (LevelManager.Instance != null)
                LevelManager.Instance.RestartLevel();
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recargar escena actual
        }

        //Method calls pause from UI
        public void Pause()
        {
            SoundManager.Instance.PlayPause(); // Sonido de pausa
            ChangeScreen(ScreenState.Pause);
        }
        public void UnPause()
        {
            SoundManager.Instance.PlayUnpause(); // Sonido de despausar
            ChangeScreen(ScreenState.Game);
        }
        //Method for change screen
        public void ChangeScreen(ScreenState screenState)
        {
            switch (screenState)
            {
                case ScreenState.Game: //if game
                    //turn off other and turn on game
                    gameScreen.SetActive(true);
                    pauseScreen.SetActive(false);

                    //Disable pause
                    GameManager.Instance.isPause = false;
                    //return normal time
                    Time.timeScale = 1;
                    break;
                case ScreenState.Pause:
                    gameScreen.SetActive(false);
                    pauseScreen.SetActive(true);

                    GameManager.Instance.isPause = true;
                    Time.timeScale = 0;
                    break;
                case ScreenState.Lose:
                    loseScreen.SetActive(true);
                    Time.timeScale = 0;
                    break;
            }
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameManager.Instance.isPause)
                    UnPause(); // Ahora usa el método con sonido
                else
                    Pause(); // Ahora usa el método con sonido
            }
        }

        public void Quit()
        {
            SoundManager.Instance.PlayDecline(); // Sonido de salir
            Application.Quit();
        }
    }
}