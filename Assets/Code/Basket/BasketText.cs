using UnityEngine;
using DG.Tweening;

public class BasketText : MonoBehaviour
{
    private async void Awake()
    {
        await transform.DOMoveY(transform.position.y + 0.1f, 1).AsyncWaitForCompletion();
        await transform.DOScale(Vector3.zero, 0.35f).SetEase(Ease.InBack).AsyncWaitForCompletion();
        Destroy(gameObject);
    }
}