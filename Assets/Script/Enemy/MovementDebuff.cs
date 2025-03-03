using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementDebuff
{
    public float slowPercentage;
    public float slowTimer;

    public float stunTimer;

    public MovementDebuff(float slowPercentage, float slowTimer, float stunTimer)
    {
        this.slowPercentage = slowPercentage;
        this.slowTimer = slowTimer;
        this.stunTimer = stunTimer;
    }

    public bool Timer()
    {
        slowTimer -= Time.deltaTime;
        stunTimer -= Time.deltaTime;

        return slowTimer < 0 && stunTimer < 0;
    }
}
