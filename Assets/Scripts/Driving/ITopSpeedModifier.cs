using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface for all modules that can modify the top speed of the racer
// Provides a scalar modifier and a bool to indicate if the modifier is active
public interface ITopSpeedModifier
{
    float modifier { get; }
    bool applyModifier { get; }
}
