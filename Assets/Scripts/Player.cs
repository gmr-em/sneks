﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Player : MonoBehaviour {

    public float speed = 5; 
    public float rotationSpeed; // the angular speed with which the snake will turn

    public int playerIndex = 1;

    public GameObject head; // the head GO
    public GameObject tail; // the tail GO

    public Text scoreText;

    public float topMargin = 5;
    public float bottomMargin = -5;
    public float rightMargin = 5;
    public float leftMargin = -5;

    private List<GameObject> body; // array for all the body pieces; used to allow tail to follow leader
    public float maxDist = 0.3f;

    private bool ate = false;
    private bool rotateLeft = false;
    private bool rotateRight = false;

    public delegate void deathAction();
    public static event deathAction OnDeath;

    void OnEnable()
    {
        GameManager.MoveLeft += MoveLeft;
        GameManager.MoveRight += MoveRight;
    }

    void OnDisable()
    {
        GameManager.MoveLeft -= MoveLeft;
        GameManager.MoveRight -= MoveRight;
    }

    void MoveLeft(int player)
    {
        if (player == playerIndex)
        {
            rotateLeft = true;
        }
    }

    void MoveRight(int player)
    {
        if (player == playerIndex)
        {
            rotateRight = true;
        }
    }

    void Start()
    {
        body = new List<GameObject>();
        int i = 0;
        // add an initial number of body parts 
        while (i++ < 2)
        {
            GameObject instance = Instantiate(tail, head.GetComponent<Transform>().position, Quaternion.identity) as GameObject;
            // disable collider for all the parts at first (they are all together initially)
            instance.GetComponent<Collider>().enabled = false;
            body.Add(instance);
        }
        // move the snek every 300ms
        InvokeRepeating("Move", 0.1f, 0.1f);
    }

    // moves the head of the snake and teleports the last body piece in its previous position
    void Move()
    {
        // float rotate = Input.GetAxis("Horizontal");
        float rotate = rotateRight ? 1 : rotateLeft ? -1 : 0;
        rotateRight = false;
        rotateLeft = false;

        // get current position of the head
        Vector3 headPosition = head.GetComponent<Transform>().position;

        if (ate)
        {
            // if food has been eaten after last movement then we add a new body part instead of moving the last element
            GameObject newBody = Instantiate(tail, headPosition, Quaternion.identity) as GameObject;
            // disable collider for the closest part
            newBody.GetComponent<Collider>().enabled = false;
            body.Insert(0, newBody);
            ate = false;

            // increase Score if we eat
            scoreText.text = "Score: " + body.Count;
        }
        else
        {
            // Move last Tail Element to where the Head was            
            body.Last().GetComponent<Transform>().position = headPosition;
            // disable collider for the closest part
            body.Last().GetComponent<Collider>().enabled = false;

            // Add to front of list, remove from the back
            body.Insert(0, body.Last());
            body.RemoveAt(body.Count - 1);
        }

        // re-enable collider for the 3rd closest body part (if there are at least 3 parts)
        if (body.Count > 2)
        {
            body.ElementAt(2).GetComponent<Collider>().enabled = true;
        }
        
        // rotate head if key was pressed
        head.GetComponent<Transform>().Rotate(new Vector3(0, rotate * rotationSpeed, 0));
        // move head forwards (based on rotation)
        headPosition += head.GetComponent<Transform>().forward * speed;

        // teleport head to other side if it goes beyond the margins
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

    void OnTriggerEnter(Collider coll)
    {
        // Food?
        if (coll.gameObject.CompareTag("Food"))
        {
            // Get longer in next Move call
            ate = true;

            // Remove the Food
            Destroy(coll.gameObject);
        }
        // Collided with Tail or other Snakes
        else
        {
            Debug.Log("Dead?");
            // call the OnDeath event which will end the current game and go to the restart menu
            if (OnDeath != null)
            {
                OnDeath();
            }
        }
    }
}
