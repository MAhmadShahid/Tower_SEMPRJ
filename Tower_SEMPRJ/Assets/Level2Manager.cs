using UnityEngine;

public class Level2Manager : MonoBehaviour
{
    public static Level2Manager Instance;

    public int collectedItemCount = 0;
    public int targetItemCount = 3;

    public GameObject doorPrefab;
    private bool doorDropped = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CollectItem()
    {
        collectedItemCount++;

        if (collectedItemCount >= targetItemCount && !doorDropped)
        {
            DropDoor();
            doorDropped = true;
        }
    }

    private void DropDoor()
    {
        
        Vector3 dropPosition = new Vector3(467.09f, 0f, 713.417847f);

        
        GameObject door = Instantiate(doorPrefab, dropPosition, Quaternion.identity);
        

        Debug.Log("Door dropped at position: " + dropPosition);
    }

}
