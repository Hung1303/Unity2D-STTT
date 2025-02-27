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

	public Transform attackPoint;
	public float attackRadius = 1f;
	public LayerMask attackLayer;

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
			animator.SetBool("Jumping", true);
		}

		if (Mathf.Abs(movement) > 0.1f)
		{
			animator.SetFloat("Moving", 1f);
		}
		else if (movement < 0.1f)
		{
			animator.SetFloat("Moving", 0f);
		}

		if (Input.GetKeyDown(KeyCode.J))
		{			
			animator.SetTrigger("Attacking");
			Attack();
			Debug.Log("Swings axe");
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

	void Attack()
	{
		if (attackPoint == null)
		{
			Debug.LogError("Attack Point is not assigned!");
			return;
		}

		Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
		if (collInfo != null) // Fixing incorrect condition
		{
			Debug.Log(collInfo.gameObject.name + " takes damage");
		}
	}

	private void OnDrawGizmosSelected()
	{
		if(attackPoint == null)
		{
			return;
		}
		Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Ground")
		{
			isGround = true;
			animator.SetBool("Jumping", false);
		}
	}
}
