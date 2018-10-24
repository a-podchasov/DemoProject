using UnityEngine;
using System.Collections;

namespace SpaceBattle
{
    public class CameraController : MonoBehaviour
    {
        private Transform cameraTransform;
        private Boundary boundary;
        private Transform playerTransform;

        private void Awake()
        {
            cameraTransform = transform;
        }
        
        private void LateUpdate()
        {
            if (playerTransform)
            {
                cameraTransform.position = new Vector3(Mathf.Clamp(playerTransform.position.x, boundary.minX, boundary.maxX),
                    Mathf.Clamp(playerTransform.position.y, boundary.minY, boundary.maxY), cameraTransform.position.z);
            }
        }


        public void SetCamera(Boundary boundary, Transform player)
        {
            playerTransform = player;
            this.boundary = new Boundary(boundary, 0.75f);
        }

    }

}