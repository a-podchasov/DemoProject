using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace SpaceBattle.UI
{

    public class GamePanel : MonoBehaviour
    {
        [SerializeField]
        private Image[] healthImages;
        [SerializeField]
        private Text scoreText;

        public event Action<bool> ShootStatusEvent;
        public event Action PauseEvent;

        public void SetHealth (int health)
        {
            for (int i = 0; i < healthImages.Length; i++)
            {
                healthImages[i].enabled = (i<health);
            }
        }


        public void OpenPause()
        {
            if (PauseEvent != null)
            {
                PauseEvent();
            }
            UIPanelsController.SetState("Pause");
        }

        public void SetScore(int kills)
        {
            if (scoreText)
            {
                scoreText.text = "Score: " + kills;
            }
        }

        public void OnShoot()
        {
            if (ShootStatusEvent != null)
            {
                ShootStatusEvent (true);
            }
        }

        public void OffShoot()
        {
            if (ShootStatusEvent != null)
            {
                ShootStatusEvent (false);
            }
        }
    }
    
}
