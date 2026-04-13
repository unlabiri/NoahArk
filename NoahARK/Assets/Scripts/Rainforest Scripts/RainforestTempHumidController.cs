using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RainforestTempHumidController : MonoBehaviour
{
    public RainforestBiomeController biomeController;
    public RainforestBiomeState biomeState;

    [SerializeField] private TMP_Text humidityText;
    [SerializeField] private TMP_Text temperatureText;
    // Start is called before the first frame update
    public ControlMode currentMode = ControlMode.Temperature;
 

    void Start()
    {
        biomeState = biomeController.State;
    }

    private void Update()
    {
        humidityText.text = biomeState.humidity.ToString("F2");
        temperatureText.text = biomeState.temperature.ToString("F2");
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
