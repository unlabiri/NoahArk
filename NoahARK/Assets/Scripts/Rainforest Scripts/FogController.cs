using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    [SerializeField] public RainforestBiomeController biomeController;
    public RainforestBiomeState biomeState;
    private float maxSafeHumidity = .88f;
    private float minSafeHumidity = .77f;

    [SerializeField] private GameObject fogModel;
    // Start is called before the first frame update
    void Start()
    {
        biomeState = biomeController.State;
        Debug.Log("here");

    }

    // Update is called once per frame
    void Update()
    {
        bool humidityUnsafe = biomeState.humidity > maxSafeHumidity || biomeState.humidity < minSafeHumidity;
        if (humidityUnsafe)
        {
            fogModel.SetActive(true);
            
        } else
        {
            fogModel.SetActive(false);
        }
    }
}
