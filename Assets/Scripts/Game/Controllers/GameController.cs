using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBattle
{

    public class GameController : MonoBehaviour
    {
        [Range(1f, 200f)]
        public float movingSpeed = 10f;
        [Range(1f, 200f)]
        public float rotationSpeed = 10f;
        public Boundary boundary;
        public FixedJoystick playerJoystick;
        
        public event Action PlayerDeathEvent;

        public void Initialize (PlayerMoveComponent playerMovement)
        {
            playerMovement.SetSpeed (movingSpeed, rotationSpeed);
            playerMovement.SetJoystick (playerJoystick);
            playerMovement.SetBoundary (boundary);
        }

        public void CheckPlayerHealth(int health)
        {
            if (health == 0)
            {
                if (PlayerDeathEvent != null)
                {
                    PlayerDeathEvent();
                }            
            }
        }

        public void SetPause()
        {
            Time.timeScale = 0f;
        }
        public void SetGame()
        {
            Time.timeScale = 1f;
        }
    }
}