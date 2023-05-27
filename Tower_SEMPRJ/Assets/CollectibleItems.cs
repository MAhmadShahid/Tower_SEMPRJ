using UnityEngine;

public class CollectibleItems : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.StartsWith("Player"))
        {
            Level2Manager.Instance.CollectItem();
            Destroy(gameObject);
        }
    }
}
