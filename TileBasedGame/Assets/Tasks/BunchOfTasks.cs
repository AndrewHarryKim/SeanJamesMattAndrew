using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Task_Wait : Task{

	private float time;

	public Task_Wait(float time){
		this.time = time;
	}

	public override bool OnUpdate(){
		time -= Time.deltaTime;
		return time <= 0;
	}

}

public class Task_Face : Task{

	private Unit user, target;

	public Task_Face(Unit user, Unit target){
		this.user = user;
		this.target = target;
	}

	public override bool OnUpdate(){
		Vector3 delta = target.transform.position - user.transform.position;
		delta.y = 0;
		user.transform.rotation = Quaternion.LookRotation (delta);
		return true;
	}

}

public class Task_Face_Eachother : Task
{
	private Unit user, target;
	
	public Task_Face_Eachother(Unit user, Unit target){
		this.user = user;
		this.target = target;
	}
	
	public override bool OnUpdate(){
		Vector3 delta = target.transform.position - user.transform.position;
		delta.y = 0;
		user.transform.rotation = Quaternion.LookRotation (delta);
		target.transform.rotation = Quaternion.LookRotation (-delta);
		return true;
	}

}

public class Task_Trigger_Animation : Task
{
	private Unit user;
	private string trigger;
	
	public Task_Trigger_Animation(Unit user, string trigger)
	{
		this.user = user;
		this.trigger = trigger;
	}

	public override bool OnUpdate(){
		if (user)
			user.anim.SetTrigger (trigger);
		return true;
	}
}

public class Task_Fire_Projectile : Task
{
	private Unit user, target;
	private GameObject prefab;
	private float speed, accel;
	private Vector3 start, end;
	private Vector3 dir;
	private float dist;

	public Task_Fire_Projectile(Unit user, Unit target, GameObject prefab, float speed=1, float accel=0){
		this.user = user;
		this.target = target;
		this.prefab = prefab;
		this.speed = speed;
		this.accel = accel;
	}

	public Task_Fire_Projectile(Vector3 start, Unit target, GameObject prefab, float speed=1, float accel=0){
		this.start = start;
		this.target = target;
		this.prefab = prefab;
		this.speed = speed;
		this.accel = accel;
	}

	public Task_Fire_Projectile(Unit user, Vector3 end, GameObject prefab, float speed=1, float accel=0){
		this.user = user;
		this.end = end;
		this.prefab = prefab;
		this.speed = speed;
		this.accel = accel;
	}

	public Task_Fire_Projectile(Vector3 start, Vector3 end, GameObject prefab, float speed=1, float accel=0){
		this.start = start;
		this.end = end;
		this.prefab = prefab;
		this.speed = speed;
		this.accel = accel;
	}

	public override void OnEnter(){
		prefab = (GameObject)GameObject.Instantiate (prefab, StartPos, Rotation);
		dir = (EndPos - StartPos).normalized;
		dist = Vector3.SqrMagnitude (EndPos - StartPos);
	}

	public override bool OnUpdate(){
		speed += accel * Time.deltaTime;
		prefab.transform.position += speed * Time.deltaTime*dir;
		return Vector3.SqrMagnitude (StartPos - prefab.transform.position) >= dist;
	}

	public override void OnExit(){
		GameObject.Destroy (prefab);
	}

	private Vector3 StartPos{
		get{
			return user ? user.transform.position : start;
		}
	}

	private Vector3 EndPos{
		get{
			return target ? user.transform.position : end;
		}
	}

	private Quaternion Rotation{
		get{
			return Quaternion.LookRotation(EndPos-StartPos);
		}
	}
}

public class Task_Execute_Skill : Task
{
	private Unit user;
	private Tile epicenter;
	private object[] args;
	private Skill skill;

	public Task_Execute_Skill(Skill skill, Unit user, Tile epicenter, object[] args){
		this.skill = skill;
		this.user = user;
		this.epicenter = epicenter;
		this.args = args;
	}

	public override bool OnUpdate(){
		skill.Execute (user, epicenter,args);
		return true;
	}

}

public class Task_ShowParticleAnimation : Task
{
    private Vector3 position;
    private Transform transPos = null;
    private GameObject prefab;
    private float destroyTime;

    public Task_ShowParticleAnimation(GameObject effect, Vector3 pos, float time)
    {
        position = pos;
        prefab = effect;
        destroyTime = time;
    }

    public Task_ShowParticleAnimation(GameObject effect, Transform pos, float time)
    {
        transPos = pos;
        prefab = effect;
        destroyTime = time;
    }

    public override bool OnUpdate()
    {
        if (transPos != null)
            position = transPos.position;
        GameObject go = (GameObject)GameObject.Instantiate(prefab, position, Quaternion.identity);
        GameObject.Destroy(go, destroyTime);
        return true;
    }

}