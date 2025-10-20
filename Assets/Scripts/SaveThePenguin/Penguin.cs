using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour
{
    public float moveSpeed = 8f;      
    public float accel = 20f;         
    public float stopDistance = 0.1f; 

    public DOTweenAnimation jumpAnim;

    public EffectComplete impactEffect;
    public EffectComplete earnEffect;

    private Rigidbody2D rb;

    public float mouseMoveThreshold = 0.15f; // chuột phải di chuyển tối thiểu mới cho đổi hướng
    private int lastMoveDir = 0;
    private float lastMouseX;
    private bool mouseLocked = false;        // đã “khóa” hướng theo vị trí chuột hiện tại chưa

    public bool isDie;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDie) return;

        if (Input.GetMouseButton(0))
        {
            float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;

            // Lần đầu giữ chuột -> chốt hướng theo vị trí chuột lúc này
            if (!mouseLocked)
            {
                float dx0 = mouseX - transform.position.x;
                if (Mathf.Abs(dx0) > stopDistance)
                {
                    lastMoveDir = (dx0 > 0f) ? 1 : -1;
                    mouseLocked = true;
                    lastMouseX = mouseX;
                }
            }
            else
            {
                // Chỉ cho đổi hướng khi người chơi kéo chuột đủ xa
                if (Mathf.Abs(mouseX - lastMouseX) > mouseMoveThreshold)
                {
                    float dx = mouseX - transform.position.x;
                    if (Mathf.Abs(dx) > stopDistance)
                    {
                        lastMoveDir = (dx > 0f) ? 1 : -1;
                        lastMouseX = mouseX;
                    }
                }
            }

            // Nếu đã có hướng -> tiếp tục chạy đều (KHÔNG dừng khi tới gần chuột)
            if (lastMoveDir != 0)
            {
                float targetVx = lastMoveDir * moveSpeed;
                float diff = targetVx - rb.velocity.x;
                rb.AddForce(new Vector2(diff * accel, 0f));

                // hoạt ảnh nghiêng theo hướng
                jumpAnim.DOPlay();
                jumpAnim.transform.eulerAngles = (lastMoveDir > 0)
                    ? new Vector3(0, 0, 12f)
                    : new Vector3(0, 0, -12f);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Thả chuột -> dừng & reset (nếu muốn thả vẫn chạy, hãy bỏ 3 dòng đầu)
            rb.velocity = Vector2.zero;
            jumpAnim.transform.eulerAngles = new Vector3(0, 0, 0f);
            lastMoveDir = 0;
            mouseLocked = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HeartItem"))
        {
            UIController.instance.Levelplay.EarnItem();

            Vector3 closestPoint = GetComponent<Collider2D>().ClosestPoint(collision.transform.position);
            earnEffect.transform.position = closestPoint;
            earnEffect.PlayEffect();

            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("TrapItem"))
        {
            UIController.instance.Levelplay.CollisionWithTrap();
            Vector3 closestPoint = GetComponent<Collider2D>().ClosestPoint(collision.transform.position);
            impactEffect.transform.position = closestPoint;
            impactEffect.PlayEffect();

            Utility.Delay(this, delegate
            {
                isDie = true;
                DieEffect();
            }, 0.2f);

            Destroy(collision.gameObject);
        }
    }

    void DieEffect ()
    {
        rb.velocity = Vector2.zero;
        jumpAnim.transform.eulerAngles = new Vector3(0, 0, 0f);

        transform.DORotate(new Vector3(0, 0, -90f), 0.1f);

        Utility.Delay(this, delegate
        {            
            transform.DORotate(new Vector3(0, 0, 0f), 0.1f).OnComplete(() =>
            {
                isDie = false;
            });
        }, 2f);
    }
}
