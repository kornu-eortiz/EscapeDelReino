using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class HealthBottle : PickUpItem
    {
        [Header("Variables")]
        public float HealthNumber;

        private void Start()
        {
            //Add event
            OnPickedUP += OnPickedEvent;
        }

        //Method for event when player enter in trigger
        void OnPickedEvent()
        {
            /////////////////////////////////////////////////Here your logic
            if (GameManager.Instance == null || GameManager.Instance.playerStats == null) return;
            
            if (GameManager.Instance.playerStats.statsData.HP < 100)
            {
                GameManager.Instance.playerStats.statsData.HP += HealthNumber;

                if (GameManager.Instance.playerStats.statsData.HP > 100)
                    GameManager.Instance.playerStats.statsData.HP = 100;

                if (UIManager.Instance != null)
                    UIManager.Instance.UpdateHP(GameManager.Instance.playerStats.statsData.HP);

                if (SoundManager.Instance != null)
                    SoundManager.Instance.PlayHeal(); // Sonido de curacion

                Destroy(gameObject);
            }
            /////////////////////////////////////////////////
        }
    }
}