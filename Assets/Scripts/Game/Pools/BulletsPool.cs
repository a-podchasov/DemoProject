using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace SpaceBattle
{
    public class BulletsPool
    {
        private Bullet bulletPrefab;
        private Boundary groundBoundary;
        private float movingSpeed = 0f;
        private int kills = 0;
        private Transform shotTransform;
        public List<Bullet> bulletsList;
        public event Action<int> GetScoreEvent;

        public BulletsPool (Bullet bullet, Boundary boundary, float mSpeed, Transform bulletPoint)
        {
            bulletsList = new List<Bullet>();
            bulletPrefab = bullet;
            groundBoundary = new Boundary(boundary, 1.2f);
            shotTransform = bulletPoint;
            movingSpeed = mSpeed;
        }

        public void CreateBullet ()
        {
            bool hasBullets = false;
            Bullet newBullet = null;
            if (bulletsList.Count > 0)
            {
                Bullet[] availableBullets = bulletsList.Where(x => !x.gameObject.activeSelf).ToArray();
                if (availableBullets.Length > 0)
                {
                    hasBullets = true;
                    newBullet = availableBullets[Random.Range(0, availableBullets.Length)];
                    newBullet.Activate(shotTransform);
                }
            }
            if (!hasBullets && bulletPrefab != null)
            {
                newBullet = GameObject.Instantiate(bulletPrefab, shotTransform.position, shotTransform.rotation);
                newBullet.SetBullet(movingSpeed);
                bulletsList.Add(newBullet);
            }
                
        }

        public void CheckBoundaries()
        {
            foreach (Bullet bot in bulletsList)
            {
                if (bot.gameObject.activeSelf)
                {
                    Vector3 pos = bot.transform.position;
                    if (pos.x > groundBoundary.maxX || pos.x < groundBoundary.minX ||
                        pos.y > groundBoundary.maxY || pos.y < groundBoundary.minY)
                    {
                        DisposeBullet(bot);
                    }
                }
            }
        }

        private void DisposeBullet (Bullet bot)
        {
            if (!bulletsList.Contains(bot))
            {
                bulletsList.Add(bot);
            }
            bot.gameObject.SetActive(false);
        }
    }
}
