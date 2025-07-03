using UnityEngine;

public class Vibration_JY : MonoBehaviour
{
    public CameraShaker_JY cameraShaker;

    void Start()
    {
        Invoke(nameof(ShakeIt), 4f);
    }

    void ShakeIt()
    {
        cameraShaker.TriggerShake(10f, 0.15f);
    }
}
