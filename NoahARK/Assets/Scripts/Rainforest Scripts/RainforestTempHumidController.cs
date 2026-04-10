using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainforestTempHumidController : MonoBehaviour
{
    public RainforestBiomeController biomeController;
    public RainforestBiomeState biomeState;
    // Start is called before the first frame update
    public ControlMode currentMode = ControlMode.Temperature;
 

    void Start()
    {
        biomeState = biomeController.State;
    }

    public void SetMode(ControlMode mode)
    {
        currentMode = mode;
    }

    public void SetTemperature(float temp)
    {
        biomeState.temperature = temp;
    }

    public void SetHumidity(float hum)
    {
        biomeState.humidity = hum;
    }

    public enum ControlMode
    {
        Temperature,
        Humidity
    }
}
