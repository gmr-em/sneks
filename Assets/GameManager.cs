using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject foodPrefab;

    // Borders
    public Transform borderTop;
    public Transform borderBottom;
    public Transform borderLeft;
    public Transform borderRight;

    public delegate void Move(int playerIndex);
    public static event Move MoveLeft;
    public static event Move MoveRight;

    public Button btnRightP1;
    public Button btnLeftP1;

    // offset from border so that food does not spawn inside the border object
    private float offset = 0.2f;

    void OnEnable()
    {
        Player.OnDeath += GoToMenu;
    }

    void OnDisable()
    {
        Player.OnDeath += GoToMenu;        
    }

    // Use this for initialization
    void Start () {
        // Generate food every 5s starting in 5s
        InvokeRepeating("GenerateFood", 5.0f, 5.0f);
    }

    public void MoveRightP1()
    {
        MoveRight(1);
    }

    public void MoveLeftP1()
    {
        MoveLeft(1);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight(1);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft(1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            MoveRight(2);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            MoveLeft(2);
        }
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

    void GoToMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }
}

