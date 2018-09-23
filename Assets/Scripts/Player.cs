using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour {

    public float speed;

    public GameObject head; // the head GO
    public GameObject tail; // the tail GO

    private List<GameObject> body; // array for all the body pieces; used to allow tail to follow leader
    public float maxDist = 0.3f;

    void Start()
    {
        body = new List<GameObject>();
        // rb = GetComponent<Rigidbody>();
        int i = 0;
        while (i++ < 15)
        {
            GameObject instance = Instantiate(tail, head.GetComponent<Transform>().position, Quaternion.identity) as GameObject;
            body.Add(instance);
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        head.GetComponent<Transform>().Translate (movement * speed * Time.deltaTime);
        Vector3 previousPart = head.GetComponent<Transform>().position;

        foreach (GameObject tail in body)
        {
            Vector3 position = tail.GetComponent<Transform>().position;
            
            if (Vector3.Distance(previousPart, position) > maxDist)
            {
                tail.GetComponent<Transform>().position = previousPart - Vector3.Normalize(previousPart - position) * maxDist;
            }

            previousPart = tail.GetComponent<Transform>().position;
        }
    }
}