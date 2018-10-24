using UnityEngine;
using System;
using System.Collections;

namespace SpaceBattle
{
    /// <summary>
    /// Контроллер бота
    /// </summary>
    public class BotMovement : MonoBehaviour
    {
        private Transform selfTransform;
        private Transform target = null;
        private float movingSpeed = 10f, rotationSpeed = 10f;
        private float startRotationtSpeed, startMovingSpeed;
        private Vector3 axisZ = new Vector3 (0f, 0f, 1f);
        private Coroutine destroyCoroutine, freezeCoroutine;        
        private bool isFreeze = false;
        private BoxCollider2D boxCollider;

        public event Action DestroyBotEvent;
        public bool IsDestroyed
        {
            get; private set;
        }

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            selfTransform = transform;
        }

        private void OnDisable()
        {
            StopFreezing();
        }

        public void Move (float deltaTime)
        {
            if (!IsDestroyed)
            {
                selfTransform.position += selfTransform.right * movingSpeed * deltaTime;
                if (target != null)
                {
                    float angleZ = Vector3.SignedAngle(target.position - selfTransform.position, selfTransform.right, axisZ);
                    if (angleZ > 1f || angleZ < -1f)
                    {
                        selfTransform.Rotate(0f, 0f, rotationSpeed * deltaTime * (angleZ > 0f ? -1f : 1f));
                    }
                }
            }
        }

        public void SetSpeed(float move, float rotate)
        {
            movingSpeed = move;
            rotationSpeed = rotate;
            startMovingSpeed = move;
            startRotationtSpeed = rotate;
        }

        public void SetPosition(Vector2 position, Vector3 direction)
        {
            selfTransform.position = position;
            selfTransform.eulerAngles = direction;
        }

        public void SetTarget (Transform newTarget)
        {
            if (target == null)
            {
                target = newTarget;
            }
        }

        public Vector3 GetPosition()
        {
            return selfTransform.position;
        }


        public void ResetBot()
        {
            isFreeze = IsDestroyed = false;
            movingSpeed = startMovingSpeed;
            rotationSpeed = startRotationtSpeed;
            boxCollider.isTrigger = false;
            selfTransform.localScale = new Vector3(1f, 1f, 1f);
            gameObject.SetActive(true);
        }

        public void DisableBot()
        {
            Deactivate(false);
        }
        
        public void DestroyBot()
        {
            Deactivate(true);
        }

        private void Deactivate(bool wasKilled)
        {
            if (!IsDestroyed)
            {
                IsDestroyed = true;
                boxCollider.isTrigger = true;
                if (destroyCoroutine == null)
                {
                    if (wasKilled && DestroyBotEvent != null)
                    {
                        DestroyBotEvent();
                    }
                    destroyCoroutine = StartCoroutine(DestroyProcess());
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!isFreeze)
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth)
                {
                    playerHealth.Damage();
                    StopFreezing();
                    freezeCoroutine = StartCoroutine(Freeze());
                }
                GetComponent<Rigidbody2D>().AddRelativeForce(collision.contacts[0].normal * 2f, ForceMode2D.Impulse);
            }
        }

        private IEnumerator Freeze ()
        {
            isFreeze = true;
            movingSpeed = rotationSpeed = 0f;

            yield return new WaitForSeconds(3f);

            movingSpeed = startMovingSpeed;
            rotationSpeed = startRotationtSpeed;
            isFreeze = false;
        }

        private void StopFreezing()
        {
            if (freezeCoroutine != null)
            {
                StopCoroutine(freezeCoroutine);
            }
        }

        private IEnumerator DestroyProcess()
        {
            movingSpeed = rotationSpeed = 0;
            float timer = 1f, dt = Time.fixedDeltaTime;
            Vector3 ds = Vector3.one * dt;
            while (timer > 0f)
            {
                yield return null;
                timer -= dt;
                selfTransform.localScale -= ds;
            }
            destroyCoroutine = null;
            gameObject.SetActive(false);
        }

    }
}