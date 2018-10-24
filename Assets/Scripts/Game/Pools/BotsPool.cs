using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace SpaceBattle
{
    public class BotsPool
    {
        private BotMovement[] botPrefabs;
        private Boundary groundBoundary;
        private Boundary checkBoundary;
        private float movingSpeed = 0f, rotationSpeed = 0f;
        private int kills = 0;

        public List<BotMovement> botsList;
        public event Action<int> GetScoreEvent;

        public BotsPool (BotMovement[] bots, Boundary boundary, float mSpeed, float rSpeed)
        {
            botsList = new List<BotMovement>();
            botPrefabs = bots;
            groundBoundary = boundary;
            movingSpeed = mSpeed;
            rotationSpeed = rSpeed;
            checkBoundary = new Boundary(boundary, 1.1f);
        }

        public void CreateBot ()
        {
            bool hasBots = false;
            BotMovement newBot = null;
            if (botsList.Count > 0)
            {
                BotMovement[] availableBots = botsList.Where(x => !x.gameObject.activeSelf).ToArray();
                if (availableBots.Length > 0)
                {
                    hasBots = true;
                    newBot = availableBots[Random.Range(0, availableBots.Length)];
                    newBot.ResetBot();
                }
            }
            if (!hasBots && botPrefabs.Length > 0)
            {
                newBot = GameObject.Instantiate(botPrefabs[Random.Range(0, botPrefabs.Length)]);
                newBot.SetSpeed(movingSpeed, rotationSpeed);
                newBot.DestroyBotEvent += AddPoint;
                botsList.Add(newBot);
            }
            if (newBot != null)
            {
                Vector3 newPosition = Vector3.zero;
                float dz = 0f;
                int random = Random.Range(0, 4);
                switch (random)
                {
                    case 0:
                        newPosition = new Vector2(groundBoundary.minX, Random.Range(groundBoundary.minY, groundBoundary.maxY));
                        if (newPosition.y > (groundBoundary.maxY + groundBoundary.minY) * 0.5f)
                        {
                            dz = Random.Range(-15f, 0f);
                        }
                        else
                        {
                            dz = Random.Range(0f, 15f);
                        }
                        break;
                    case 1:
                        newPosition = new Vector2(Random.Range(groundBoundary.minX, groundBoundary.maxX), groundBoundary.maxY);
                        if (newPosition.x > (groundBoundary.maxX + groundBoundary.minX) * 0.5f)
                        {
                            dz = Random.Range(255f, 270f);
                        }
                        else
                        {
                            dz = Random.Range(270f, 285f);
                        }
                        break;
                    case 2:
                        newPosition = new Vector2(groundBoundary.maxX, Random.Range(groundBoundary.minY, groundBoundary.maxY));
                        if (newPosition.y > (groundBoundary.maxY + groundBoundary.minY) * 0.5f)
                        {
                            dz = Random.Range(180f, 195f);
                        }
                        else
                        {
                            dz = Random.Range(165f, 180f);
                        }
                        break;
                    case 3:
                        newPosition = new Vector2(Random.Range(groundBoundary.minX, groundBoundary.maxX), groundBoundary.minY);
                        if (newPosition.x > (groundBoundary.maxX + groundBoundary.minX) * 0.5f)
                        {
                            dz = Random.Range(90f, 105f);
                        }
                        else
                        {
                            dz = Random.Range(75f, 90f);
                        }
                        break;
                }
                newBot.SetPosition(newPosition, new Vector3(0f, 0f, dz));
            }
        }

        public void CheckBoundaries()
        {
            foreach (BotMovement bot in botsList)
            {
                if (bot.gameObject.activeSelf)
                {
                    Vector3 pos = bot.GetPosition();
                    if (pos.x > checkBoundary.maxX || pos.x < checkBoundary.minX ||
                        pos.y > checkBoundary.maxY || pos.y < checkBoundary.minY)
                    {
                        DisposeBot(bot);
                    }
                }
            }
        }

        private void DisposeBot (BotMovement bot)
        {
            if (!botsList.Contains(bot))
            {
                botsList.Add(bot);
            }
            bot.SetTarget(null);
            bot.gameObject.SetActive(false);
        }

        private void AddPoint()
        {
            kills++;
            if (GetScoreEvent != null)
            {
                GetScoreEvent(kills);
            }
        }
    }
}
