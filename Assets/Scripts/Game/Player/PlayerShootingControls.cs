using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBattle
{

    public class PlayerShootingControls : MonoBehaviour
    {
        public Bullet bulletPrefab;
        public Transform shootPoint;

        private Boundary boundary;
        private float bulletSpeed = 10f;
        private Coroutine shootCoroutine;
        private YieldInstruction yi = new WaitForSeconds(0.5f);
        private BulletsPool bulletsPool;

        public void Initialize (Boundary groundBoundary, float bSpeed)
        {
            boundary = new Boundary(groundBoundary);
            bulletSpeed = bSpeed;
            bulletsPool = new BulletsPool(bulletPrefab, groundBoundary, bSpeed, shootPoint);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shooting(true);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                Shooting(false);
            }
#endif

            if (bulletsPool != null && bulletsPool.bulletsList.Count > 0)
            {
                float deltaTime = Time.deltaTime;
                foreach (Bullet bullet in bulletsPool.bulletsList)
                {
                    if (bullet.gameObject.activeSelf)
                    {
                        bullet.Move(deltaTime);
                    }
                }
                bulletsPool.CheckBoundaries();
            }
        }

        public void CreateBullet ()
        {
            if (bulletsPool != null)
            {
                bulletsPool.CreateBullet();
            }
        }

        public void Shooting (bool isActive)
        {
            if (isActive)
            {
                shootCoroutine = StartCoroutine(ShootingCoroutine());
            }
            else
            {
                if (shootCoroutine != null)
                {
                    StopCoroutine(shootCoroutine);
                }
            }
        }

        private IEnumerator ShootingCoroutine()
        {
            while (true)
            {
                yield return yi;
                CreateBullet();
            }
        }

    }

}