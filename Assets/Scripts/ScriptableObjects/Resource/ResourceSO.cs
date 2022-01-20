using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/Resource", order = 1)]
public class ResourceSO : ScriptableObject
{
    public EResourceType ResourceType;
    public float MaxValue;
}

public enum EResourceType
{
    Food,
    Water
}
