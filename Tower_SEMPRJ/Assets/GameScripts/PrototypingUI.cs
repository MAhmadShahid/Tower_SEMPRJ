using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrototypingUI : MonoBehaviour
{

    public Rigidbody _playerRigidBody;
    public TextMeshProUGUI x, y, z;
    public TextMeshProUGUI cx, cy, cz;

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = _playerRigidBody.velocity;
        x.text = "X: " + Mathf.Round(velocity.x).ToString();
        y.text = "Y: " + Mathf.Round(velocity.y).ToString();
        z.text = "Z: " + Mathf.Round(velocity.z).ToString();

        if(Input.GetKey(KeyCode.Mouse1))
        {
            cx.text = "X: " + Mathf.Round(velocity.x).ToString();
            cy.text = "Y: " + Mathf.Round(velocity.y).ToString();
            cz.text = "Z: " + Mathf.Round(velocity.z).ToString();
        }
    }
}
