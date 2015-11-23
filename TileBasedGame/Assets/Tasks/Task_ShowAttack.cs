using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Task_ShowAttack : Task
{
    float timer = 0.6f;
    Unit user, target;
    string trigger;

    public Task_ShowAttack(Unit user, Unit target, string trigger)
    {
        this.user = user;
        this.target = target;
        this.trigger = string.Copy(trigger);
    }

    public override void OnEnter()
    {
        Vector3 delta = target.transform.position - user.transform.position;
        delta.y = 0;
        user.transform.rotation = Quaternion.LookRotation(delta);
        target.transform.rotation = Quaternion.LookRotation(-delta);
        if (user.anim)
            user.anim.SetTrigger(trigger);
        target.anim.SetTrigger("Hit");
    }

    public override bool OnUpdate()
    {
        timer -= Time.deltaTime;
        return timer <= 0;
    }

    public override void OnExit()
    {
        target.TakeDamage(3,user);
    }



}

