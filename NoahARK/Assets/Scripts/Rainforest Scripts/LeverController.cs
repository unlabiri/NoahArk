using UnityEngine;

public class LeverController : MonoBehaviour
{
    [Header("References")]
    public LeverInteractable leverInteractable;
    public RainforestTempHumidController controller;

    [Header("Temperature Range")]
    public float tempMin = 50f;
    public float tempMax = 150f;

    [Header("Humidity Range")]
    public float humidMin = 0f;
    public float humidMax = 1f;

    void Update()
    {
        if (leverInteractable == null || controller == null) return;

        float normalized = leverInteractable.NormalizedValue;

        switch (controller.currentMode)
        {
            case RainforestTempHumidController.ControlMode.Temperature:
                controller.SetTemperature(Mathf.Lerp(tempMin, tempMax, normalized));
                break;
            case RainforestTempHumidController.ControlMode.Humidity:
                controller.SetHumidity(Mathf.Lerp(humidMin, humidMax, normalized));
                break;
        }
    }
}