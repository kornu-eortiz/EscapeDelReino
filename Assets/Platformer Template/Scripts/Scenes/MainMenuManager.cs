using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class MainMenuManager : MonoBehaviour
    {
        //Simle main menu script
        //Nothing unusual

        public void NewGame()
        {
            SoundManager.Instance.PlayConfirm(); // Sonido de confirmar
            
            // Usar LevelManager si existe, si no cargar Level1 directamente
            if (LevelManager.Instance != null)
                LevelManager.Instance.StartGame();
            else
                SceneManager.LoadScene("Level1");
        }

        public void Quit()
        {
            SoundManager.Instance.PlayDecline(); // Sonido de salir
            Application.Quit();
        }
    }
}