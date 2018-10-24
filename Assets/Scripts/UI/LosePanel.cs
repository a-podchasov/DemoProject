using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace SpaceBattle.UI
{

    public class LosePanel : MonoBehaviour
    {
        [SerializeField]
        private Text scoreText;

        public void OpenLosePanel()
        {
            UIPanelsController.SetState("Lose");
        }

        public void SetLosePanel (int record)
        {
            if (scoreText)
            {
                scoreText.text = "Record: " + record;
            }
            StartCoroutine(GoToMenu());
        }

        private IEnumerator GoToMenu()
        {
            yield return new WaitForSeconds(3f);
            OpenMenu();
        }

        public void OpenMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
        }
    }
    
}
