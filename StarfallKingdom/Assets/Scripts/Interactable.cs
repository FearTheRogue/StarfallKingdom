using Unity.VisualScripting;
using UnityEngine;

public enum InteractionTypes { Enemy, Item }
public class Interactable : MonoBehaviour
{
    public Actor myActor { get; private set; }

    public InteractionTypes interactionType;

    private void Awake()
    {
        if (interactionType == InteractionTypes.Enemy)
            myActor = GetComponent<Actor>(); ;
    }

    public void InteractWithItem()
    {
        Destroy(gameObject);
    }
}
