using UnityEngine;

public class PlayerControl : MonoBehaviour
{
	public Rigidbody2D rb;
	public Animator animator;

	public float jumpHeight = 5f;
	private bool isGround = true;

	public float movement;
	public float speed = 5f;
	private bool facingRight = true;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}

	void Update()
    {
		movement = Input.GetAxis("Horizontal");

		if (movement < 0f && facingRight)
		{
			transform.eulerAngles = new Vector3(0f, -180f, 0f);
			facingRight = false;
		}
		else if (movement > 0f && facingRight == false)
		{
			transform.eulerAngles = new Vector3(0f, 0f, 0f);
			facingRight = true;
		}

		if (Input.GetKeyDown(KeyCode.Space) && isGround)
		{
			Jump();
			isGround = false;
		}

		if (Mathf.Abs(movement) > 0.1f)
		{
			animator.SetFloat("Moving", 1f);
		}
		else if (movement < 0.1f)
		{
			animator.SetFloat("Moving", 0f);
		}
	}

	private void FixedUpdate()
	{
		transform.position += new Vector3(movement, 0f, 0f) * Time.deltaTime * speed;
	}

	void Jump()
	{
		rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Ground")
		{
			isGround = true;
		}
	}
}
