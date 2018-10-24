using UnityEngine;
using System.Collections;
using System;

namespace SpaceBattle
{

    public class PlayerHealth : MonoBehaviour
    {
        private int hp = 3;
        private int startHP = 3;
        public event Action<int> GetHealthEvent;

        private void Awake()
        {
            
        }

        public void Reset()
        {
            hp = startHP;
            if (GetHealthEvent != null)
            {
                GetHealthEvent(hp);
            }
        }

        public void Damage()
        {
            if (hp > 0)
            {
                hp--;
            }
            if (GetHealthEvent != null)
            {
                GetHealthEvent(hp);
            }
        }

    }

}