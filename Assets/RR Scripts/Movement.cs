using UnityEngine;
using UnityEngine.UI;
//By: tyler ung 3/2/2025
//Description:
//this script is used for dashing as well as main movment
public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float dashLength = 0.1f;
    public float dashCooldown = 1f;
    public int dashMax = 2;

    private float activeMoveSpeed;
    private float dashCounter;
    private float dashCooldownTimer;
    private int dashCount;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    private Vector2 moveInput;
    private Vector2 dashDirection;

    public bool isDashing { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        activeMoveSpeed = moveSpeed;
        dashCount = dashMax;
    }

    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        moveVelocity = moveInput * activeMoveSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && dashCount > 0)
        {
            dashDirection = moveInput;
            activeMoveSpeed = dashSpeed;
            dashCounter = dashLength;
            dashCount--;
            isDashing = true;
            Debug.Log("Dash Charges: " + dashCount);
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            if (dashCounter <= 0)
            {
                activeMoveSpeed = moveSpeed;
                isDashing = false;
            }
        }

        if (dashCount < dashMax)
        {
            dashCooldownTimer += Time.deltaTime;
            if (dashCooldownTimer >= dashCooldown)
            {
                dashCooldownTimer = 0;
                dashCount++;
                Debug.Log("Dash recharged!");
                Debug.Log("Dash Charges: " + dashCount);
            }
        }

    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }


}
