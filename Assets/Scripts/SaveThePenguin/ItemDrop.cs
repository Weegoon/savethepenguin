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
            if (UIController.instance.Levelplay.itemDrops.Contains(gameObject))
                UIController.instance.Levelplay.itemDrops.Remove(gameObject);
            Destroy(gameObject);
        }, 10f);

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        SoundManager.instance.PlayAudioClip(SoundManager.instance.trapFall);

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
        
        if (UIController.instance.Levelplay.CurrentScore < 100f)
            rb.gravityScale = 0.5f;
        else if (UIController.instance.Levelplay.CurrentScore < 300f)
            rb.gravityScale = 0.7f;
        else if (UIController.instance.Levelplay.CurrentScore < 500f)
            rb.gravityScale = 1f;
        else if (UIController.instance.Levelplay.CurrentScore < 700f)
            rb.gravityScale = 1.5f;
        else rb.gravityScale = 2f;
    }
}
