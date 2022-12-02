using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Random = Unity.Mathematics.Random;

internal struct RandomStairs : IComponentData
{
    public Random Value;
}
