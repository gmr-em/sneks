using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject foodPrefab;

    // Borders
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;

    // offset from border so that food does not spawn inside the border object
    private float offset = 0.2f;

    // Use this for initialization
    void Start () {
        // Generate food every 5s starting in 5s
        InvokeRepeating("GenerateFood", 5.0f, 5.0f);
    }
	
	// Generate food at a random position on the board
	void GenerateFood() {
        // x position between left & right border
        int x = (int)Random.Range(borderLeft.position.x + offset,
                                  borderRight.position.x - offset);

        // y position between top & bottom border
        int z = (int)Random.Range(borderBottom.position.z + offset,
                                  borderTop.position.z - offset);

        // Instantiate the food at (x, 0.5, z)
        Instantiate(foodPrefab,
                    new Vector3(x, 0.5f, z),
                    Quaternion.identity); // default rotation
    }
}
