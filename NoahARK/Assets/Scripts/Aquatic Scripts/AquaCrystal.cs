using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Throwable))]
public class AquaCrystal : MonoBehaviour
{
    [SerializeField] private AudioSource chimeSound;

    private bool used = false;

    public void Consume()
    {
        if (used) return;
        used = true;

        if (chimeSound != null)
            chimeSound.Play();

        Hand holdingHand = GetComponent<Interactable>()?.attachedToHand;
        if (holdingHand != null)
            holdingHand.DetachObject(gameObject);

        Destroy(gameObject, chimeSound != null ? chimeSound.clip.length : 0f);
    }
}