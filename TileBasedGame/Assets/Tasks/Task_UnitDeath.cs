using UnityEngine;
using System.Collections;
using System.Collections.Generic;


class Task_UnitDeath : Task
{
    Unit unit;

    public Task_UnitDeath(Unit unit)
    {
        this.unit = unit;
    }

    public override bool OnUpdate()
    {
        if (unit)
            GameObject.Destroy(unit.gameObject);
        return true;
    }

}