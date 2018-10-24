using UnityEngine;
using System.Collections;

namespace SpaceBattle
{

    public class MenuViewController : MonoBehaviour
    {
        public UI.MenuPanel menuPanel;
        public DataController dataController;
        public UI.UIPanelsController uiPanelsController;

        private void Awake()
        {
            Time.timeScale = 1f;
            if (dataController)
            {
                if (menuPanel)
                {
                    dataController.GetRecordEvent += menuPanel.SetRecord;
                }
                dataController.LoadPlayerData();
            }
            if (uiPanelsController)
            {
                uiPanelsController.Init();
                uiPanelsController.SetStateByIndex(0);
            }
        }
    }

}
