using UnityEngine;
using UnityEngine.Events;

public class SimpleLock : MonoBehaviour
{
    public GameObject correctKey;
    public UnityEvent onFaultFixed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == correctKey)
        {
            correctKey.SetActive(false);
            onFaultFixed.Invoke(); // This tells the Randomizer the slot is free again
        }
    }
}