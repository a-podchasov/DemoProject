#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace SpaceBattle
{

    public class DataControllerEditor : MonoBehaviour
    {

        [MenuItem("Data controller/Clear data")]
        public static void ClearData()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "saves.json");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log("Clear data!");
            }
        }

    }
}
#endif
