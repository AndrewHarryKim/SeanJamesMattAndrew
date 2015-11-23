using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class EffectContainer
{

    public Effect effect;
    public Unit user;

    public int cooldown; // -1 if permanent.
    public bool Permanent
    {
        get
        {
            return cooldown < 0;
        }
    }


    public EffectContainer(Unit user, Effect effect, int turns)
    {
        this.user = user;
        this.effect = effect;
        cooldown = turns;

        effect.Initialize(user);
    }


    //return true if effect times out
    public bool Tick()
    {
        effect.onTimeTick(user);
        return !Permanent && --cooldown <= 0;
    }



}