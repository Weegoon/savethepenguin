using UnityEngine;

public class Penguin : MonoBehaviour
{
    public float moveSpeed = 8f;      // tốc độ mong muốn
    public float accel = 20f;         // lực tăng tốc khi đổi hướng
    public float stopDistance = 0.1f; // khoảng cách gần chuột thì dừng lại

    private Rigidbody2D rb;

    Vector2 velocity = Vector2.zero;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float dx = mousePos.x - transform.position.x;

            if (Mathf.Abs(dx) > stopDistance)
            {
                // Hướng mục tiêu
                float dir = Mathf.Sign(dx);
                float targetVx = dir * moveSpeed;

                // Tạo lực để vận tốc tiệm cận targetVx
                float diff = targetVx - rb.velocity.x;
                rb.AddForce(new Vector2(diff * accel, 0f));
            }
            else
            {
                // Gần vị trí chuột -> dừng lại
                rb.velocity = Vector2.zero;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            rb.velocity = Vector2.zero;
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HeartItem"))
        {
            UIController.instance.Levelplay.EarnItem();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("TrapItem"))
        {
            UIController.instance.Levelplay.CollisionWithTrap();
            Destroy(collision.gameObject);
        }
    }
}
