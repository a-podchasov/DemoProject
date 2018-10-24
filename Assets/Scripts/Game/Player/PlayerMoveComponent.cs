using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBattle
{

    public class PlayerMoveComponent : MonoBehaviour
    {
        [SerializeField]
        private float movingSpeed = 10f;
        [SerializeField]
        private float rotationSpeed = 10f;

        private Boundary boundary;
        private Transform selfTransform;
        private FixedJoystick joystick;
        private Vector3 axisZ = new Vector3(0f, 0f, 1f);
        private void Awake()
        {
            selfTransform = transform;
        }

        private void Update()
        {
#if UNITY_EDITOR
            Vector2 vecMove = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (vecMove.magnitude != 0)
            {
                SetMove(vecMove);
            }
#endif
            if (joystick.Direction.magnitude != 0)
            {
                SetMove (joystick.Direction);
            }
        }

        public void SetMove (Vector2 vec)
        {
            float dt = Time.deltaTime;
            float angleZ = Vector3.SignedAngle(vec, selfTransform.right, axisZ);
            if (angleZ > 1f || angleZ < -1f)
            {
                selfTransform.Rotate(0f, 0f, rotationSpeed * dt * (angleZ > 0f ? -1f : 1f));
            }
            
            Vector2 delta = selfTransform.right * movingSpeed * dt;
            selfTransform.position = new Vector2(Mathf.Clamp(selfTransform.position.x + delta.x, boundary.minX, boundary.maxX),
                Mathf.Clamp(selfTransform.position.y + delta.y, boundary.minY, boundary.maxY));
        }


        public void SetBoundary(Boundary boundary)
        {
            this.boundary = boundary;
        }

        public void SetJoystick(FixedJoystick fixedJoystick)
        {
            joystick = fixedJoystick;
        }

        public void SetSpeed (float move, float rotate)
        {
            movingSpeed = move;
            rotationSpeed = rotate;
        }

        public void DestroyPlayer()
        {
            Destroy(gameObject);
        }
    }

}