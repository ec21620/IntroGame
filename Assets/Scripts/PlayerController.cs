using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{

    public Vector2 moveValue;
    public float speed;

    private int count;
    private int numPickups = 8;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI playerDistance;
    public TextMeshProUGUI playerPosition;
    public TextMeshProUGUI playerVelocity;

    private LineRenderer lineRenderer;

    void Start()
    {
        count = 0;
        winText.text = "";
        SetCountText();

        lineRenderer = gameObject.AddComponent<LineRenderer>();
    }

    void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveValue.x, 0.0f, moveValue.y);

        GetComponent<Rigidbody>().AddForce(movement * speed * Time.fixedDeltaTime);

        playerPosition.text = "Player position: " + transform.position.x.ToString("0.00") + "," + transform.position.z.ToString("0.00");

        playerVelocity.text = "Player velocity: " + GetComponent<Rigidbody>().velocity.magnitude.ToString("0.00");

        GameObject[] pickups = GameObject.FindGameObjectsWithTag("PickUp");
        if(pickups.Length > 0)
        {
            float closestDistance = Vector3.Distance(transform.position, pickups[0].transform.position);
            GameObject closestPickup = pickups[0];

            foreach(GameObject pickup in pickups)
            {
                pickup.GetComponent<Renderer>().material.color = new Color(1f, 0.5f, 0f);
                float checkDistance = Vector3.Distance(transform.position, pickup.transform.position);

                if  (checkDistance < closestDistance) {
                    closestDistance = checkDistance;
                    closestPickup = pickup;
                } 
            }

            closestPickup.GetComponent<Renderer>().material.color = new Color(0f, 0.8f, 1f);
            playerDistance.text = "Player distance: " + closestDistance.ToString("0.00");

            lineRenderer.SetPosition (0, transform.position);
            lineRenderer.SetPosition(1, closestPickup.transform.position);
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
        } else
        {
            lineRenderer.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    private void SetCountText()
    {
        scoreText.text = "Score: " + count.ToString();
        if (count >= numPickups)
        {
            winText.text = "You Win!";
        }
    }
}