using UnityEngine;

[CreateAssetMenu(fileName = "EntityProperties", menuName = "ScriptableObjects/EntityProperties", order = 1)]
public class EntityProperties : ScriptableObject
{
    public float HungerMax;
    public float HungerDecrease;
    public float ThirstMax;
    public float ThirstDecrease;
    public float InteractRange;
    public float InteractTime;
}
