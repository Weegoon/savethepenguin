using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Utility
{
    #region Misc
    public static Coroutine DelayToNextFrame(MonoBehaviour mon, Action action)
    {
        return mon.StartCoroutine(IEDelayToNextFrame(action));
    }

    public static Coroutine Delay(MonoBehaviour mon, Action action, float time)
    {
        return mon.StartCoroutine(IEDelay(time, action));
    }

    public static IEnumerator IEDelay(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        if (action != null)
        {
            action.Invoke();
        }
    }

    public static IEnumerator IEDelayToNextFrame(Action action)
    {
        yield return new WaitForEndOfFrame();
        if (action != null)
        {
            action.Invoke();
        }
    }

    public static bool IsLayerInMask(LayerMask mask, int layer)
    {
        return (mask.value & (1 << layer)) > 0;
    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }
    #endregion

    #region UI

    public static bool CheckTouchUI()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public static bool IsPointerOverLayerElement(string layerName)
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults(), layerName);
    }
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults, string layerName)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer(layerName))
                return true;
        }
        return false;
    }
    ///Gets all event systen raycast results of current mouse or touch position.
    public static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
#if UNITY_EDITOR
        eventData.position = Input.mousePosition;
#endif
        if (Input.touchCount == 1)
        {
            eventData.position = Input.touches[0].position;
        }

        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    public static void ScrollRectCenterOnItem(ScrollRect mScrollRect, RectTransform target)
    {
        RectTransform mScrollTransform = mScrollRect.GetComponent<RectTransform>();
        RectTransform maskTransform = mScrollRect.viewport;
        RectTransform mContent = mScrollRect.content;
        // Item is here
        var itemCenterPositionInScroll = GetWorldPointInWidget(mScrollTransform, GetWidgetWorldPoint(target));
        //itemCenterPositionInScroll.y -= 275f;
        // But must be here
        var targetPositionInScroll = GetWorldPointInWidget(mScrollTransform, GetWidgetWorldPoint(maskTransform));
        // So it has to move this distance
        var difference = targetPositionInScroll - itemCenterPositionInScroll;
        difference.z = 0f;
        //clear axis data that is not enabled in the scrollrect
        if (!mScrollRect.horizontal)
        {
            difference.x = 0f;
        }
        if (!mScrollRect.vertical)
        {
            difference.y = 0f;
        }

        var normalizedDifference = new Vector2(
                                       difference.x / (mContent.rect.size.x - mScrollTransform.rect.size.x),
                                       difference.y / (mContent.rect.size.y - mScrollTransform.rect.size.y));

        var newNormalizedPosition = mScrollRect.normalizedPosition - normalizedDifference;
        if (mScrollRect.movementType != ScrollRect.MovementType.Unrestricted)
        {
            newNormalizedPosition.x = Mathf.Clamp01(newNormalizedPosition.x);
            newNormalizedPosition.y = Mathf.Clamp01(newNormalizedPosition.y);
        }

        //mScrollRect.DONormalizedPos(newNormalizedPosition, 0.5f);
        mScrollRect.normalizedPosition = newNormalizedPosition;
    }

    private static Vector3 GetWidgetWorldPoint(RectTransform target)
    {
        var pivotOffset = new Vector3(
                              (0.5f - target.pivot.x) * target.rect.size.x,
                              (0.5f - target.pivot.y) * target.rect.size.y,
                              0f);
        var localPosition = target.localPosition + pivotOffset;
        return target.parent.TransformPoint(localPosition);
    }

    private static Vector3 GetWorldPointInWidget(RectTransform target, Vector3 worldPoint)
    {
        return target.InverseTransformPoint(worldPoint);
    }

    #endregion
}

[System.Serializable]
public class Wrapper<T>
{
    public List<T> list;
    public Wrapper(List<T> list) => this.list = list;
}