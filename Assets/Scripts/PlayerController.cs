using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    private float horizontalInput;
    private float verticalInput;

    public float moveSpeed = 6f;

    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelo;

    private bool isWalking;

    private Vector3 direction;

    public float animDelay = 0.1f;
    private float timer = 0.0f;

    private int coinCount;

    public Text coinsLeft;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        coinsLeft.text = coinCount.ToString("0");
    }

    // Update is called once per frame
    void Update()
    {
        // Gets the direction of the player based in their input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Rotates the player to face the direction they are moving in
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelo, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir * moveSpeed * Time.deltaTime);

            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        // Changes to the walking animation
        if (isWalking)
        {
            GetComponent<Animator>().SetInteger("AnimatorState", 1);
            timer = 0;
        }
        // Changes to the idle animation
        else
        {
            timer += Time.deltaTime;
            if (timer >= animDelay)
            {
                GetComponent<Animator>().SetInteger("AnimatorState", 0);
                timer = 0;
            }
        } 

        // Checks if the player has collected all coins. 
        // If so, switches to the end screen
        if (coinCount == 10)
        {
            SceneManager.LoadScene("End");
        }

        CoinsNeeded();
    }

    // Checks if the player has collided with a coin and adds to their total coin count
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
            coinCount++;
        }
    }

    // Updates text to let player know how many coins they have collected
    void CoinsNeeded()
    {
        coinsLeft.text = coinCount.ToString("");
    }
}