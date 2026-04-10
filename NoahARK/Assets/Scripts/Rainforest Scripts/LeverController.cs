using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{

    public Transform leverTransform;

    public RainforestTempHumidController controller;

    public float leverMinAngle = -45f;
    public float leverMaxAngle = 45f;

    // temp range
    public float tempMin = 50f;
    public float tempMax = 150f;

    // humidity range
    public float humidMin = 0;
    public float humidMax = 1;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {

        if (leverTransform == null)
        {
            return;
        }

        float angle = GetLeverAngle();
        float normalized = Mathf.InverseLerp(leverMinAngle, leverMaxAngle, angle);

        switch(controller.currentMode)
        {
            case RainforestTempHumidController.ControlMode.Temperature:
                controller.SetTemperature(Mathf.Lerp(tempMin, tempMax, normalized));
                break;
            case RainforestTempHumidController.ControlMode.Humidity:
                controller.SetHumidity(Mathf.Lerp(humidMin, humidMax, normalized));
                break;
        }
    }

    private float GetLeverAngle()
    {
       
        return 0;
    }
}
