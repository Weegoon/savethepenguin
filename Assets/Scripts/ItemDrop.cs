using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        Utility.Delay(this, delegate
        {
            Destroy(gameObject);
        }, 10f);

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        StartCoroutine(IEShowHide());
    }

    IEnumerator IEShowHide()
    {
        SpriteRenderer img = GetComponent<SpriteRenderer>();
        for (int i = 0; i <= 8; i++)
        {
            if (i % 2 == 0)
                img.color = new Color32(255, 255, 255, 255);
            else
                img.color = new Color32(0, 0, 0, 0);
            yield return new WaitForSeconds(0.1f);
        }
        rb.gravityScale = 0.5f;
    }
}
