using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainforestTempHumidController : MonoBehaviour
{
    // Start is called before the first frame update
    public ControlMode currentMode = ControlMode.Temperature;
    public float temperature;
    public float humidity;


    public void SetMode(ControlMode mode)
    {
        currentMode = mode;
    }

    public enum ControlMode
    {
        Temperature,
        Humidity
    }
}
