using UnityEngine;

public class CollectibleItems : MonoBehaviour
{
    public float timeIncrease = 10f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.StartsWith("Player"))
        {
            Level2Manager.Instance.CollectItem();
            Timer.Instance.IncreaseTime(timeIncrease);
            Destroy(gameObject);
        }
    }
}
