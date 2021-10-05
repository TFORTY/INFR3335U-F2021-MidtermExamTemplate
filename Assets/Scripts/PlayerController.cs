using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    private float horizontalInput;
    private float verticalInput;

    public float moveSpeed = 6f;

    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelo;

    private bool isWalking;

    private Vector3 direction;

    public float animDelay = 0.1f;
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelo, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            controller.Move(direction * moveSpeed * Time.deltaTime);

            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        if (isWalking)
        {
            GetComponent<Animator>().SetInteger("AnimatorState", 1);
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= animDelay)
            {
                GetComponent<Animator>().SetInteger("AnimatorState", 0);
                timer = 0;
            }
            //GetComponent<Animator>().SetInteger("AnimatorState", 0);
        }
        
    }
}