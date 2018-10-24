using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace SpaceBattle
{
    /// <summary>
    /// Контроллер данных о рекорде игрока и других параметров, которые могут понадобиться в игре
    /// </summary>
    public class DataController : MonoBehaviour
    {
        public static DataController Instance
        {
            private set; get;
        }

        private PlayerData playerData;
        private string filePath;

        public event Action<int> GetRecordEvent;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadPlayerData()
        {
            filePath = Path.Combine(Application.persistentDataPath, "saves.json");
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                playerData = JsonUtility.FromJson<PlayerData>(jsonString);
            }
            else
            {
                playerData = new PlayerData();
                playerData.roundSeconds = 0;
                playerData.playerRecord = 0;
                SavePlayerData();
            }

            if (playerData != null && GetRecordEvent != null)
            {
                GetRecordEvent (playerData.playerRecord);
            }
        }

        public void SavePlayerData()
        {
            string json = JsonUtility.ToJson (playerData);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            File.WriteAllText(filePath, json);
        }

        public void SetPlayerData (int scores)
        {
            if (playerData != null)
            {
                if (playerData.playerRecord < scores)
                {
                    playerData.playerRecord = scores;
                    SavePlayerData();
                }
            }
        }

        public void CheckPlayerRecord()
        {
            if (playerData != null && GetRecordEvent != null)
            {
                GetRecordEvent(playerData.playerRecord);
            }
        }

        public PlayerData GetPlayerData()
        {
            return playerData;
        }

        #region Static functions
        public static void Save()
        {
            if (Instance != null)
            {
                Instance.SavePlayerData();
            }
        }
        public static void Load()
        {
            if (Instance != null)
            {
                Instance.LoadPlayerData();
            }
        }
        public static PlayerData GetPlayerParameteres()
        {
            return Instance != null ? Instance.GetPlayerData() : null;
        }
        #endregion

    }

}