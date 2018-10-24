using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace SpaceBattle.UI
{
    
    public class PausePanel : MonoBehaviour
    {
        public event Action ReturnGameEvent;
        public void OpenMenu()
        {
            UIPanelsController.SetState("Loading");
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
        }

        public void ReturnToGame()
        {
            UIPanelsController.SetState("Game");
            if (ReturnGameEvent != null)
            {
                ReturnGameEvent();
            }
        }
    }
    
}
