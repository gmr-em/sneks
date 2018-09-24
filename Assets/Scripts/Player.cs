using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour {

    public float speed = 5; 
    public float rotationSpeed; // the angular speed with which the snake will turn

    public GameObject head; // the head GO
    public GameObject tail; // the tail GO

    public float topMargin = 5;
    public float bottomMargin = -5;
    public float rightMargin = 5;
    public float leftMargin = -5;

    private List<GameObject> body; // array for all the body pieces; used to allow tail to follow leader
    public float maxDist = 0.3f;

    void Start()
    {
        body = new List<GameObject>();
        int i = 0;
        // add an initial number of body parts 
        while (i++ < 15)
        {
            GameObject instance = Instantiate(tail, head.GetComponent<Transform>().position, Quaternion.identity) as GameObject;
            body.Add(instance);
        }
        // move the snek every 300ms
        InvokeRepeating("Move", 0.1f, 0.1f);
    }

    // moves the head of the snake and teleports the last body piece in its previous position
    void Move()
    {
        float rotate = Input.GetAxis("Horizontal");      

        // Move last Tail Element to where the Head was
        Vector3 headPosition = head.GetComponent<Transform>().position;
        body.Last().GetComponent<Transform>().position = headPosition;

        // Add to front of list, remove from the back
        body.Insert(0, body.Last());
        body.RemoveAt(body.Count - 1);

        head.GetComponent<Transform>().Rotate(new Vector3(0, rotate * rotationSpeed, 0));
        headPosition += head.GetComponent<Transform>().forward * speed;

        if (headPosition.z > topMargin)
        {
            headPosition.z = bottomMargin;
        }
        else if (headPosition.z < bottomMargin)
        {
            headPosition.z = topMargin;
        }

        if (headPosition.x > rightMargin)
        {
            headPosition.x = leftMargin;
        }
        else if (headPosition.x < leftMargin)
        {
            headPosition.x = rightMargin;
        }

        head.GetComponent<Transform>().position = headPosition;
    }    
}