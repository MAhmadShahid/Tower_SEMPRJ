using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimationScript : MonoBehaviour
{
    [SerializeField] GameObject _towerObject;
    [SerializeField] GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _towerObject = GameObject.FindWithTag("Tower");
        _player = GameObject.FindWithTag("Player");
        transform.position = _player.transform.position + new Vector3(0.0f, 2.0f, 0.0f);
        transform.LookAt(_towerObject.transform.position);
        // InvokeRepeating("PlayArrowAnimation", 0.0f, 2.25f);
        PlayArrowAnimation();
        StartCoroutine(DestroyAfterDelay());
    }

    private void Update()
    {
        transform.LookAt(_towerObject.transform.position); 
    }

    void PlayArrowAnimation()
    {
        Vector3 originalPosition = transform.position;
        Vector3 goalPosition = transform.forward * 5 + originalPosition;
        LeanTween.move(gameObject, goalPosition, 2f).setEaseOutElastic().callOnCompletes();

    }   


    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
