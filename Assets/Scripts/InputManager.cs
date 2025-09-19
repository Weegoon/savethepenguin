using System;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public float swipeThreshold = 10f;
    public float timeThreshold = 0.3f;

    public UnityEvent OnSwipeLeft;
    public UnityEvent OnSwipeRight;
    public UnityEvent OnSwipeUp;
    public UnityEvent OnSwipeDown;

    public UnityEvent OnMouseDown;
    public UnityEvent OnMouseEnter;
    public UnityEvent OnMouseUp;

    private Vector3 fingerDown;
    private DateTime fingerDownTime;
    private Vector2 fingerUp;
    private DateTime fingerUpTime;
    Vector3 initialWorldFingerDown;
    Vector3 runtimeWorldFingerDown;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 vec1 = Input.mousePosition;
            Vector3 vec2 = new Vector3(vec1.x, vec1.y, 0f);
            this.initialWorldFingerDown = Camera.main.ScreenToWorldPoint(vec2);            

            this.fingerDown = Input.mousePosition;
            this.fingerUp = Input.mousePosition;
            this.fingerDownTime = DateTime.Now;

            this.OnMouseDown.Invoke();
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 vec1 = Input.mousePosition;
            Vector3 vec2 = new Vector3(vec1.x, vec1.y, 0f);
            this.runtimeWorldFingerDown = Camera.main.ScreenToWorldPoint(vec2);

            this.fingerDown = Input.mousePosition;
            this.fingerUpTime = DateTime.Now;
            this.OnMouseEnter.Invoke();
            this.CheckSwipe();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            this.OnMouseUp.Invoke();
        }
    }

    private void CheckSwipe()
    {
        float duration = (float)this.fingerUpTime.Subtract(this.fingerDownTime).TotalSeconds;
        if (duration > this.timeThreshold) return;

        float deltaX = this.fingerDown.x - this.fingerUp.x;
        if (Mathf.Abs(deltaX) > this.swipeThreshold)
        {
            if (deltaX > 0)
            {
                this.OnSwipeRight.Invoke();
            }
            else if (deltaX < 0)
            {
                this.OnSwipeLeft.Invoke();
            }
        }

        float deltaY = fingerDown.y - fingerUp.y;
        if (Mathf.Abs(deltaY) > this.swipeThreshold)
        {
            if (deltaY > 0)
            {
                this.OnSwipeUp.Invoke();
            }
            else if (deltaY < 0)
            {
                this.OnSwipeDown.Invoke();
            }
        }

        this.fingerUp = this.fingerDown;
    }

    public Vector3 MousePosToWorld()
    {
        Vector3 vec1 = Input.mousePosition;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(vec1);
        mousePos.z = 0f;

        return mousePos;
    }

    public Vector3 MousePosToWorld(Camera cam)
    {
        Vector3 vec1 = Input.mousePosition;
        Vector3 mousePos = cam.ScreenToWorldPoint(vec1);
        mousePos.z = 0f;

        return mousePos;
    }

    // Get touch object with mouse pos
    public T GetTouchObject<T>()
    {
        Ray pos = Camera.main.ScreenPointToRay(Input.mousePosition);

        var hit = Physics2D.RaycastAll(pos.origin, pos.direction);

        foreach (var item in hit)
        {
            if (item.collider != null)
            {
                T obj = item.collider.gameObject.GetComponent<T>();
                if (obj != null)
                {
                    return obj;
                }
            }
        }

        return default(T);
    }

    public T GetTouchObject<T>(Camera cam)
    {
        Ray pos = cam.ScreenPointToRay(Input.mousePosition);

        var hit = Physics2D.RaycastAll(pos.origin, pos.direction);

        foreach (var item in hit)
        {
            if (item.collider != null)
            {
                T obj = item.collider.gameObject.GetComponent<T>();
                if (obj != null)
                {
                    return obj;
                }
            }
        }

        return default(T);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 v = new Vector3(initialWorldFingerDown.x, initialWorldFingerDown.y, -100);
        Gizmos.DrawLine(initialWorldFingerDown, v);
    }

    // Get touch object with custom Pos
    public T GetTouchObject<T>(Vector3 worldPos)
    {
        var hit = Physics2D.LinecastAll(worldPos, new Vector3(worldPos.x, worldPos.y, 100));

        foreach (var item in hit)
        {
            if (item.collider != null)
            {
                T obj = item.collider.gameObject.GetComponent<T>();
                if (obj != null)
                {
                    return obj;
                }
            }
        }

        return default(T);
    }
}
