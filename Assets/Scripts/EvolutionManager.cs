using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionManager : MonoBehaviour
{
    public static EvolutionManager Instance;
    public LayerMask FoodMask;
    public LayerMask WaterMask;
    public LayerMask EntityMask;

    public void Start()
    {
        Instance = this;
    }
}
