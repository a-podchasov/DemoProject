using UnityEngine;
using System.Collections;

namespace SpaceBattle
{
    public class Bullet : MonoBehaviour
    {
        private float speed = 0f;
        private Transform selfTransform;

        private void Awake()
        {
            selfTransform = transform;
        }
        
        public void SetBullet(float bulletSpeed)
        {
            speed = bulletSpeed;
        }

        public void Move(float dt)
        {
            selfTransform.position += selfTransform.right * speed * dt;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            BotMovement botMovement = collision.GetComponent<BotMovement>();
            if (botMovement)
            {
                botMovement.DestroyBot();
            }
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }


        public void Activate (Transform shotTransform)
        {
            gameObject.SetActive(true);
            selfTransform.position = shotTransform.position;
            selfTransform.rotation = shotTransform.rotation;
        }
    }
}

#region Comments

//private void Update()
//{
//    selfTransform.position += selfTransform.right * speed * Time.deltaTime;
//    if (selfTransform.position.x > boundary.maxX || selfTransform.position.x < boundary.minX ||
//                selfTransform.position.y > boundary.maxY || selfTransform.position.y < boundary.minY)
//    {
//        Destroy(gameObject);
//    }
//}

#endregion