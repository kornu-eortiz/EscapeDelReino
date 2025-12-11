using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;  // Para música de fondo
        [SerializeField] private AudioSource sfxSource;    // Para efectos de sonido

        [Header("Music Clips")]
        public AudioClip menuMusic;         // Música del menú
        public AudioClip gameMusic;         // Música del juego
        public AudioClip victoryMusic;

        [Header("SFX Clips")]
        public AudioClip confirmSFX;
        public AudioClip declineSFX;
        public AudioClip crossbowSFX;
        public AudioClip healSFX;
        public AudioClip pauseSFX;
        public AudioClip unpauseSFX;
        public AudioClip stepsSFX;
        public AudioClip swordAttackSFX;
        public AudioClip secondSwordAttackSFX;  // Segundo sonido para combo
        public AudioClip enemyHitSFX;           // Cuando el enemigo recibe daño
        public AudioClip playerLoseSFX;         // Cuando el jugador muere
        public AudioClip playerHitSFX;          // Cuando el jugador es golpeado
        public AudioClip jumpSFX;               // Salto
        public AudioClip landingSFX;            // Aterrizar
        public AudioClip climbSFX;              // Subir escaleras
        public AudioClip teleportSFX;           // Teleport (para implementar después)

        private bool useSecondSwordSound = false; // Alternar entre sonidos de espada

        [Header("Settings")]
        [Range(0f, 1f)] public float musicVolume = 0.5f;
        [Range(0f, 1f)] public float sfxVolume = 1f;

        void Awake()
        {
            // Singleton persistente entre escenas
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject); // No se destruye al cambiar de escena
        }

        void Start()
        {
            // Iniciar música según la escena
            string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (sceneName == "MainMenu")
                PlayMusic(menuMusic);
            else
                PlayMusic(gameMusic);
        }

        // ===== MÚSICA =====
        public void PlayMusic(AudioClip clip)
        {
            if (clip == null) return;

            musicSource.clip = clip;
            musicSource.volume = musicVolume;
            musicSource.loop = true;  // La música se repite
            musicSource.Play();
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }

        public void SetMusicVolume(float volume)
        {
            musicVolume = volume;
            musicSource.volume = musicVolume;
        }

        // ===== EFECTOS DE SONIDO =====
        public void PlaySFX(AudioClip clip)
        {
            if (clip == null) return;
            // No reproducir SFX si el juego está pausado (excepto pause/unpause)
            if (GameManager.Instance != null && GameManager.Instance.isPause) return;
            sfxSource.PlayOneShot(clip, sfxVolume);
        }

        // Versión que ignora la pausa (para sonidos de UI)
        public void PlaySFXIgnorePause(AudioClip clip)
        {
            if (clip == null) return;
            sfxSource.PlayOneShot(clip, sfxVolume);
        }

        public void SetSFXVolume(float volume)
        {
            sfxVolume = volume;
        }

        // ===== MÉTODOS RÁPIDOS PARA LLAMAR DESDE OTROS SCRIPTS =====
        public void PlayConfirm() => PlaySFX(confirmSFX);
        public void PlayDecline() => PlaySFX(declineSFX);
        public void PlayCrossbow() => PlaySFX(crossbowSFX);
        public void PlayHeal() => PlaySFX(healSFX);
        public void PlayPause() => PlaySFXIgnorePause(pauseSFX);
        public void PlayUnpause() => PlaySFXIgnorePause(unpauseSFX);
        public void PlaySteps() => PlaySFX(stepsSFX);
        public void PlayEnemyHit() => PlaySFX(enemyHitSFX);
        public void PlayPlayerLose() => PlaySFX(playerLoseSFX);
        public void PlayPlayerHit() => PlaySFX(playerHitSFX);
        public void PlayJump() => PlaySFX(jumpSFX);
        public void PlayLanding() => PlaySFX(landingSFX);
        public void PlayClimb() => PlaySFX(climbSFX);
        public void PlayTeleport() => PlaySFX(teleportSFX);
        public void PlayVictory() => PlayMusic(victoryMusic);
        public void PlayMenuMusic() => PlayMusic(menuMusic);
        public void PlayGameMusic() => PlayMusic(gameMusic);

        // Alterna entre los dos sonidos de espada para el combo
        public void PlaySwordAttack()
        {
            if (useSecondSwordSound)
                PlaySFX(secondSwordAttackSFX);
            else
                PlaySFX(swordAttackSFX);
            
            useSecondSwordSound = !useSecondSwordSound; // Alternar para el próximo ataque
        }
    }
}
