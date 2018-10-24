using UnityEngine;
using System.Collections;

namespace SpaceBattle
{
    public class GameViewController : MonoBehaviour
    {
        [Header("Controllers:")]
        public GameController gameController;
        public BotsController botsController;
        public PlayerMoveComponent playerMove;
        public PlayerShootingControls playerShooting;
        public PlayerHealth playerHealth; 
        public CameraController cameraController;
        public DataController dataController;

        [Header("UI:")]
        public UI.GamePanel gamePanel;
        public UI.PausePanel pausePanel;
        public UI.LosePanel losePanel;
        public UI.UIPanelsController uiPanelsController;
        
        private void Awake()
        {
            if (gameController)
            {
                gameController.SetGame();
                if (botsController)
                {
                    botsController.SetPlayer(playerMove.transform);
                    botsController.Initialize(gameController.movingSpeed, gameController.rotationSpeed * 0.8f, gameController.boundary);
                    gameController.PlayerDeathEvent += botsController.StopBots;
                }

                if (playerMove)
                {
                    if (cameraController)
                    {
                        cameraController.SetCamera(gameController.boundary, playerMove.transform);
                    }
                    gameController.PlayerDeathEvent += playerMove.DestroyPlayer;
                    gameController.Initialize(playerMove);
                }
                if (playerShooting)
                {
                    playerShooting.Initialize(gameController.boundary, gameController.movingSpeed * 2f);
                    if (gamePanel)
                    {
                        gamePanel.ShootStatusEvent += playerShooting.Shooting;
                    }
                }
                if (playerHealth)
                {
                    playerHealth.GetHealthEvent += (int x) =>
                    {
                        if (gamePanel)
                        {
                            gamePanel.SetHealth (x);
                        }
                        gameController.CheckPlayerHealth(x);
                    };
                }

                #region UI
                if (losePanel)
                {
                    gameController.PlayerDeathEvent += losePanel.OpenLosePanel;
                    if (dataController)
                    {
                        gameController.PlayerDeathEvent += dataController.CheckPlayerRecord;
                        dataController.GetRecordEvent += losePanel.SetLosePanel;
                    }
                }
                if (pausePanel)
                {
                    pausePanel.ReturnGameEvent += gameController.SetGame;
                }
                if (gamePanel)
                {
                    if (botsController)
                    {
                        botsController.GetScoreEvent += gamePanel.SetScore;
                        gamePanel.SetScore(0);
                    }
                    gamePanel.PauseEvent += gameController.SetPause;
                }
                #endregion
            }

            #region Data controller
            if (dataController)
            {
                if (botsController)
                {
                    botsController.GetScoreEvent += dataController.SetPlayerData;
                }
                dataController.LoadPlayerData();
            }
            #endregion

            if (uiPanelsController)
            {
                uiPanelsController.Init();
                uiPanelsController.SetStateByIndex(0);
            }
            
        }

    }
}

