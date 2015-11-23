using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//To be added: Visuals for the skill
//Should be in the form of adding Tasks to the GameManager task queue
//Will likely be a delegate handled by class in the factory
public class Skill
{
    public string name = "Nameless Skill";
    public Sprite icon = null;

    public enum TargetType { SELF, ALLY, ENEMY, NONE, CONDITIONAL };
    public TargetType targetType = TargetType.ENEMY;
    public TargetTypeDelegate targetTypeFunction = user => TargetType.ENEMY; //only gets called if targetType is CONDITIONAL

    public enum DamageType { DAMAGE, HEAL, CONDITIONAL }
    public DamageType damageType = DamageType.DAMAGE;
    public DamageTypeDelegate damageTypeFunction = user => DamageType.DAMAGE; //only gets called if damageType is CONDITIONAL

    public delegate bool SkillBoolDelegate(Unit user, Unit target, object[] args);
    public delegate TargetType TargetTypeDelegate(Unit user);
    public delegate DamageType DamageTypeDelegate(Unit user);

    public delegate float CostDelegate(Unit user);
    public CostDelegate manaCost = (user) => 0;

    public int cooldown;

    public float basePower = 1; //base healing/damage

    public int aoe = 0; //dls with depth limiter equal to aoe: single target spells have aoe of 0.
    public int range = 0; //range 0 only hits the tile you're on, range 1 is melee, and so on.

    public bool requiresEmptyTile = false; //moves that create new units shouldn't be castable on occupied tiles

    public delegate void SkillEffectDelegate(Unit user, Unit target, object[] args);
    public SkillEffectDelegate OnTarget;

    public delegate void TaskGenDelegate(Unit user, Tile epicenter, object[] args);
    public TaskGenDelegate GenerateTasks;

    public delegate void EpicenterDelegate(Unit user, Tile epicenter, object[] args);
    public EpicenterDelegate OnTilePostEffects = (user, epicenter, args) => {};

    public void DefaultOnTarget(Unit user, Unit target)
    {
        DamageType dt = damageType == DamageType.CONDITIONAL ? damageTypeFunction(user) : damageType;

        if (dt == DamageType.DAMAGE)
            target.TakeDamage(basePower * user.DamageMultiplier * (1f - target.Armor),user);
    }

    public List<Unit> gatherTargets(Unit user, Tile t)//gather targets from center of tile extending out aoe tiles. The targettype affects it.
    {
        TargetType tt = targetType == TargetType.CONDITIONAL ? targetTypeFunction(user) : targetType;
        return gatherTargets(user, t, tt);
    }

    public List<Unit> gatherTargets(Unit user, Tile t, TargetType tt)//gather targets from center of tile extending out aoe tiles. The targettype affects it.
    {
        List<Unit> list = new List<Unit>();
        foreach (Tile tile in GameManager.instance.TilesInRangeSkill(t, aoe, user,this))
        {
            if(tt == TargetType.ENEMY)
            {
                if (tile.unit != null && user.IsEnemy(tile.unit))
                    list.Add(tile.unit);
            }
            else if (tt == TargetType.ALLY)
            {
                if (tile.unit != null && user.IsAlly(tile.unit))
                    list.Add(tile.unit);
            }
            else if (tt == TargetType.SELF)
            {
                if (tile.unit != null && user == tile.unit)
                    list.Add(tile.unit);
            }
        }
        return list;
    }

    public enum TargetTileRestriction { ANY, UNIT, EMPTY};
    public TargetTileRestriction tileRestriction = TargetTileRestriction.UNIT;
    public bool ValidTile(Unit user, Tile tile)
    {
        if (tileRestriction == TargetTileRestriction.ANY)
            return true;
        if (tileRestriction == TargetTileRestriction.UNIT)
        {
            if (targetType == TargetType.ENEMY)
            {
                if (tile.unit != null && user.IsEnemy(tile.unit))
                    return true;
            }
            else if (targetType == TargetType.ALLY)
            {
                if (tile.unit != null && user.IsAlly(tile.unit))
                    return true;
            }
            else
            { 
                if (tile.unit != null && user == tile.unit)
                    return true;
            }
            return false;
        }
        return tile.unit == null;
    }

	public bool CanCast(Unit user){
		return user.curMP >= manaCost(user);
	}


    public void Execute(Unit user, Tile t)
    {
        Execute(user, t, null);
    }

    public void Execute(Unit user, Tile t, object[] args)
    {
		//user.curMP -= manaCost(user);
        if (!IsInRange(user,t))
            return;
        List<Unit> targets = gatherTargets(user, t);
        foreach(Unit target in targets)
        {
            OnTarget(user, target, args);
        }
        OnTilePostEffects(user,t,args);
    }

	public void Perform(Unit user, Tile t){
		Perform (user, t, null);
	}

	public void Perform(Unit user, Tile t, object[] args){
        user.curMP -= manaCost(user);
        GenerateTasks (user, t, args);
	}

    public HashSet<string> triggers = new HashSet<string>(); //triggers will be used to see if certain talents are active

    public Skill()
    {
        OnTarget = (user, target, args) => DefaultOnTarget(user, target);
		GenerateTasks = (user,epicenter,args) => EnqueueExecuteTask(user,epicenter,args);
    }

	public void EnqueueExecuteTask(Unit user, Tile epicenter, object[] args){
		GameManager.instance.tasks.Add (new Task_Execute_Skill (this, user, epicenter, args));
	}

	public void EnqueueWait(float t){
		GameManager.instance.tasks.Add (new Task_Wait (t));
	}

    public bool IsInRange(Unit user, Tile t)
    {
        return GameManager.instance.TilesInRangeSkill(user.tile, range, user,this).Contains(t);
    }


}