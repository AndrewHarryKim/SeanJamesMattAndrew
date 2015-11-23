using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Task_MoveToTile : Task
{
    private Unit unit;
    private Tile tile;
    private List<Tile> path;


    public Task_MoveToTile(Unit unit, Tile tile)
    {
        this.unit = unit;
        this.tile = tile;
    }

    public override void OnEnter()
    {
        path = GameManager.instance.FindPath(unit.tile, tile);
        if(path != null && path.Count > 0)
            unit.transform.position = new Vector3(unit.transform.position.x, path[0].TopPosition.y, unit.transform.position.z);
        if (unit.anim)
            unit.anim.SetBool("Walking",true);
    }
    public override bool OnUpdate()
    {
        if (path == null || path.Count == 0)
            return true;
        Vector3 delta = path[0].TopPosition - unit.transform.position + Vector3.up;
        delta.y = 0;
        unit.transform.rotation = Quaternion.Slerp(unit.transform.rotation,Quaternion.LookRotation(delta),Time.deltaTime*10);
        if (delta.magnitude < 0.01)
        {
            unit.transform.position = path[0].TopPosition;
            path.RemoveAt(0);
            if (path.Count == 0)
            {
                tile.SetUnit(unit);
                return true;
            }
            else
            {
                unit.transform.position = new Vector3(unit.transform.position.x, path[0].TopPosition.y, unit.transform.position.z);
            }
        }
        else
        {
            unit.transform.position += delta.normalized * Mathf.Clamp(0f,Time.deltaTime,delta.magnitude);
        }

        return false;
    }
    public override void OnExit()
    {
        if (unit.anim)
            unit.anim.SetBool("Walking", false);
    }
}