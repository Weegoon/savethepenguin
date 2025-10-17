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

    public float dirSwitchThreshold = 0.15f; // ngưỡng đổi hướng để tránh rung
    private int lastMoveDir = 0;

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
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float dx = mousePos.x - transform.position.x;

            // Cập nhật hướng khi chuột đủ lệch về 1 phía
            if (Mathf.Abs(dx) > dirSwitchThreshold)
                lastMoveDir = (dx > 0f) ? 1 : -1;

            // Nếu chưa có hướng (mới nhấn chuột mà gần), mặc định không làm gì
            if (lastMoveDir != 0)
            {
                float targetVx = lastMoveDir * moveSpeed;

                // Tiệm cận vận tốc đều bằng AddForce -> có quán tính mềm khi đổi hướng
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
            rb.velocity = Vector2.zero;
            jumpAnim.transform.eulerAngles = new Vector3(0, 0, 0f);
            lastMoveDir = 0;
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

            DieEffect();

            Destroy(collision.gameObject);
        }
    }

    void DieEffect ()
    {
        isDie = true;
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
