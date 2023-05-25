using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Tower
{
    public class PlayerCheckpointHandler : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LevelOneManager _levelManager;

        private void OnTriggerEnter(Collider collidedWith)
        {
            if(collidedWith.gameObject.name.StartsWith("Checkpoint"))
            {
                string platformName = collidedWith.transform.parent.name;
                int checkpointIndex = (int)platformName[platformName.Length - 1] - 48;

                _levelManager.UpdateCheckpoint(checkpointIndex);
                // Debug.Log($"Triggered checkpoint for Platform{checkpointIndex}");
            }
        }
    }
}

