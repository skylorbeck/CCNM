using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class SafeAnimator : MonoBehaviour
{
    [field: SerializeField] private Transform safeHandle;
    [field: SerializeField] private Transform safeDoor;
    [field: SerializeField] private Transform safeBottom;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    public async Task Open()
    {
        // DOTween.To(() => safeHandle.localRotation, (x => safeHandle.localRotation = x), Quaternion.Euler(0, 0, 360), 1f);
        SoundManager.Instance.PlayEffect("SafeHandleSpin");
        safeHandle.DORotate(new Vector3(0, 0, 360), 1f,RotateMode.FastBeyond360);
        await Task.Delay(1000);
        SoundManager.Instance.PlayEffect("SafeClick");
        await Task.Delay(250);
        SoundManager.Instance.PlayEffect("SafeOpen");

        safeDoor.DOLocalMove(new Vector3(-1.5f,0,0), 1f).SetEase(Ease.OutSine);
        // safeDoor.DORotate(new Vector3(0, 0, 360), 1f,RotateMode.FastBeyond360);
        await Task.Delay(1000);
    }

    public async void Close()
    {
        safeDoor.DOLocalMove(Vector3.zero, 0.5f);
        SoundManager.Instance.PlayEffect("SafeOpen");
        await Task.Delay(500);
        SoundManager.Instance.PlayEffect("SafeClick");
        safeHandle.DORotate(new Vector3(0, 0, -360), 1f,RotateMode.FastBeyond360);
        SoundManager.Instance.PlayEffect("SafeHandleSpin");
    }
}
