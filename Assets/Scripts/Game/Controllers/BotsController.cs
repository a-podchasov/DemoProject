using UnityEngine;
using System.Collections;
using System;

namespace SpaceBattle
{
    /// <summary>
    /// Контроллер ботов, создание новых и удаление всех активных при окончании раунда
    /// </summary>
    public class BotsController : MonoBehaviour
    {
        [SerializeField]
        private BotMovement[] botPrefabs;

        [SerializeField]
        [Range(0, 50)]
        private int maxBots = 20;
        
        private float sqrAgressiveRadius = 5f;
        private BotsPool botsPool;
        private Coroutine waveRoutine;
        private Transform playerTransform;

        public event Action<int> GetScoreEvent;

        public void Initialize(float mSpeed, float rSpeed, Boundary boundary)
        {
            botsPool = new BotsPool(botPrefabs, new Boundary(boundary, 1.1f), mSpeed, rSpeed);
            if (Camera.main.orthographic)
            {
                float size = Camera.main.orthographicSize * 1.0f;
                sqrAgressiveRadius = size * size;
            }
            botsPool.GetScoreEvent += GetScore;
            waveRoutine = StartCoroutine(WaveCoroutine());
        }

        public void SetPlayer (Transform player)
        {
            playerTransform = player;
        }

        private void Update()
        {
            if (playerTransform != null && botsPool != null && botsPool.botsList.Count > 0)
            {
                float deltaTime = Time.deltaTime;
                foreach (BotMovement bot in botsPool.botsList)
                {
                    if (bot.gameObject.activeSelf)
                    {
                        bot.Move(deltaTime);
                        if ((playerTransform.position - bot.GetPosition()).sqrMagnitude < sqrAgressiveRadius)
                        {
                            bot.SetTarget(playerTransform);
                        }
                        else
                        {
                            bot.SetTarget(null);
                        }
                    }
                }
                botsPool.CheckBoundaries();
            }
        }

        public void StopBots()
        {
            if (waveRoutine != null)
            {
                StopCoroutine(waveRoutine);
            }
            foreach (BotMovement bot in botsPool.botsList)
            {
                if (bot.gameObject.activeSelf)
                {
                    bot.DisableBot();
                }
            }
        }

        public void GetScore(int score)
        {
            if (GetScoreEvent != null)
            {
                GetScoreEvent(score);
            }
        }

        private IEnumerator WaveCoroutine()
        {
            YieldInstruction yiPause = new WaitForSeconds (1f);
            yield return yiPause;
            while (true)
            {
                if (botsPool.botsList.Count < maxBots)
                {
                    botsPool.CreateBot();
                }
                yield return yiPause;                                    
            }
        }

    }

}