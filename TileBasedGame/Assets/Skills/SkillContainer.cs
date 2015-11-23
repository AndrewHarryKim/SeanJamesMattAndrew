using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class SkillContainer
{
    public Skill skill;
    public Unit user;

    public int cooldown = 0;
    public bool OffCooldown
    {
        get
        {
            return cooldown <= 0;
        }
    }

    public bool IsCastable
    {
        get
        {
            return OffCooldown && skill.CanCast(user);
        }
    }

    public float CooldownProportion
    {
        get
        {
            return OffCooldown ? 1 : 1f - (float)cooldown / (float)skill.cooldown;
        }
    }


    public SkillContainer(Unit user, Skill skill)
    {
        this.user = user;
        this.skill = skill;
    }

    //returns true if skill is or goes off cooldown.
    public bool Tick()
    {
        return OffCooldown || --cooldown <= 0;
    }



}
