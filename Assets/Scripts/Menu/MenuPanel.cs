using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceBattle.UI
{
    public class MenuPanel : MonoBehaviour {
        [SerializeField]
        private Text recordText;

        private void Awake()
        {
            
        }

        public void SetRecord (int record)
        {
            if (recordText)
            {
                recordText.text = "Record: " + record;
            }
        }

        public void OpenGameScene()
        {
            UIPanelsController.SetState("Loading");
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
        }
    }
}