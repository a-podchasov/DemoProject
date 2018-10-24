using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBattle.UI
{
    /// <summary>
    /// Простой контроллер для панелей интерфейса
    /// </summary>
    public class UIPanelsController : MonoBehaviour
    {
        public static UIPanelsController Instance
        {
            private set;
            get;
        }

        public UIPanel [] panels;

        private Dictionary<string, UIPanel> panelsDictionary = new Dictionary<string, UIPanel>();
        private Stack<UIPanel> activePanelsStack = new Stack<UIPanel>();

        public void Init()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            CheckPanels();
        }

        private void CheckPanels()
        {
            if (panelsDictionary.Count == 0)
            {
                foreach (var uiPanel in panels)
                {
                    panelsDictionary.Add(uiPanel.panelName, uiPanel);
                    foreach (var panel in uiPanel.panels)
                    {
                        panel.SetActive(false);
                    }
                }
            }
        }

        public string GetActiveStateName()
        {
            return activePanelsStack.Count > 0 ? activePanelsStack.Peek().panelName : "";
        }

        public void SetStateByName (string panelName)
        {
            CheckPanels();
            if (panelsDictionary.ContainsKey(panelName))
            {
                UIPanel uIPanel = panelsDictionary[panelName];

                if (!uIPanel.isAdditive && activePanelsStack.Count > 0)
                {
                    while (activePanelsStack.Count > 0)
                    {
                        UIPanel stackPanel = activePanelsStack.Pop();
                        foreach (var panel in stackPanel.panels)
                        {
                            panel.SetActive(false);
                        }
                    }
                }
                
                foreach (var panel in uIPanel.panels)
                {
                    panel.SetActive(true);
                }
                activePanelsStack.Push(uIPanel);
            }
        }

        public void SetStateByIndex(int index)
        {
            CheckPanels();
            if (panelsDictionary.Count > index)
            {
                UIPanel uIPanel = panelsDictionary.Values.ElementAt(index);

                if (!uIPanel.isAdditive && activePanelsStack.Count > 0)
                {
                    while (activePanelsStack.Count > 0)
                    {
                        UIPanel stackPanel = activePanelsStack.Pop();
                        foreach (var panel in stackPanel.panels)
                        {
                            panel.SetActive(false);
                        }
                    }
                }

                foreach (var panel in uIPanel.panels)
                {
                    panel.SetActive(true);
                }
                activePanelsStack.Push(uIPanel);
            }
        }

        public void CloseState ()
        {
            if (activePanelsStack.Count > 0)
            {
                UIPanel stackPanel = activePanelsStack.Pop();
                foreach (var panel in stackPanel.panels)
                {
                    panel.SetActive(false);
                }
            }
        }

        public static string GetStateName()
        {
            if (Instance != null)
            {
                return Instance.GetActiveStateName();
            }
            return "";
        }

        public static void SetState(string panelName)
        {
            if (Instance != null && panelName.Length > 0)
            {
                Instance.SetStateByName(panelName);
            }
        }
        public static void SetState(int index)
        {
            if (Instance != null && index >= 0)
            {
                Instance.SetStateByIndex(index);
            }
        }

        public static void CloseCurrentState()
        {
            if (Instance != null)
            {
                Instance.CloseState();
            }
        }

    }

    [System.Serializable]
    public struct UIPanel
    {
        public string panelName;
        public GameObject[] panels;
        public bool isAdditive;
    }

}

