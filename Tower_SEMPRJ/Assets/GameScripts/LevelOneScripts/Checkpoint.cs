using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower
{
    [System.Serializable]
    public class Checkpoint
    {
        public Transform _checkpointTransform;
        public int _checkpointPriority;
    }
}
