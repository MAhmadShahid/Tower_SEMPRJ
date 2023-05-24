using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower
{
    [System.Serializable]
    public class Cooldown
    {
        public float _cooldownTimer = 1.0f;
        public float _startTime = 0.0f;

        public bool IsCoolingDown()
        {
            if (Time.time > _startTime + _cooldownTimer)
            {
                return false;
            }
            return true;
        }

        public void InitiateCooldown()
        {
            _startTime = Time.time;
        }
    }
}

