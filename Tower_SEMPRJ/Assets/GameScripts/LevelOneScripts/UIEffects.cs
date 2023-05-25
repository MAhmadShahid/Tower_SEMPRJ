using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEffects : MonoBehaviour
{
    //[Header("References")]
    //[SerializeField] private Canvas _worldSpaceCanvas;

    private Transform _playerTransform;

    private List<TextMeshProUGUI> _worldCanvasTextElements;
    private List<Image> _worldCanvasImageElements;

   [SerializeField] private float _fadeDistance;

    [SerializeField] private TextMeshProUGUI _testText;

    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = GameObject.FindWithTag("Player").transform;

        _worldCanvasTextElements = new List<TextMeshProUGUI>();  
        _worldCanvasImageElements = new List<Image>();   

        GetComponentsInChildren<TextMeshProUGUI>(_worldCanvasTextElements);
        GetComponentsInChildren<Image>(_worldCanvasImageElements);
    }

    // Update is called once per frame
    void Update()
    {
        HandleUITransparency();
        // TestFunction();
    }

    // only render the ui that is close to the player
    void HandleUITransparency()
    {
        Vector3 playerPosition = _playerTransform.position;

        foreach(TextMeshProUGUI tmpElement in _worldCanvasTextElements)
        {
            float distanceOfElementFromPlayer = Vector3.Distance(tmpElement.transform.position, playerPosition);
            distanceOfElementFromPlayer = Mathf.Clamp(distanceOfElementFromPlayer, -_fadeDistance, _fadeDistance);

            tmpElement.alpha = 1 - distanceOfElementFromPlayer/_fadeDistance;
        }

        foreach(Image imageElement in _worldCanvasImageElements)
        {
            float distanceOfElementFromPlayer = Vector3.Distance(imageElement.transform.position, playerPosition);
            distanceOfElementFromPlayer = Mathf.Clamp(distanceOfElementFromPlayer, -_fadeDistance, _fadeDistance);

            Color imageColor = imageElement.color;
            imageElement.color = new Color(imageColor.r, imageColor.g, imageColor.b, 1 - distanceOfElementFromPlayer / _fadeDistance);
        }
    }

    void TestFunction()
    {

        if(_worldCanvasImageElements.Count == 0) { Debug.Log("This shit null af"); }
        //Vector3 playerPosition = _playerTransform.position;
        //Vector3 testTextPosition = _testText.transform.position;

        //Debug.Log($"Player: {playerPosition}, Text: {testTextPosition}\nDistance: {Vector3.Distance(playerPosition, testTextPosition)}");
    }
}
