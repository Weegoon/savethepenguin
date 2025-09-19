using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseButton : Button
{
    [Range(10f, 200f)] public float percentScale = 105f;

    bool isAnimRun = false;

    public bool isSound = true;

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!IsActive() || !IsInteractable())
            return;

        ButtonClick(eventData);
    }

    void ButtonClick(PointerEventData eventData)
    {
        if (isAnimRun)
        {
            return;
        }
        if (isSound)
        {
            //SoundManager.Instance.PlayAudioClip(SoundManager.Instance.mainButton);
        }

        isAnimRun = true;
        Vector3 target = transform.localScale * percentScale / 100f;
        Vector3 originalScale = transform.localScale;
        target.z = 0f;
        transform.DOScale(target, 0.05f).OnComplete(delegate
        {
            transform.DOScale(originalScale, 0.05f).OnComplete(delegate
            {
                isAnimRun = false;
                base.OnPointerClick(eventData);
            }).SetUpdate(true);
        }).SetUpdate(true);
    }
}
