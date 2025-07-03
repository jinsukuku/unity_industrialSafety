using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class CameraShaker_JY : MonoBehaviour
{
    public float shakeDuration = 20f;
    public float shakeMagnitue = 20f;

    private Vector3 initPos;
    private float remainShakeTime;

    private void Start()
    {
        initPos = transform.localPosition;
    }

    private void Update()
    {
        if (remainShakeTime > 0)
        {
            transform.localPosition = initPos + Random.insideUnitSphere * shakeMagnitue;
            remainShakeTime -= Time.deltaTime;

            if (remainShakeTime <= 0f)
            {
                transform.localPosition = initPos;
            }
        }
    }

    public void TriggerShake(float duration, float magnitude)
    {
        duration = -1f;
        magnitude = -1f;

        if (duration > 0) shakeDuration = duration;
        if (magnitude > 0) shakeMagnitue = magnitude;

        remainShakeTime = shakeDuration;
    }
}
