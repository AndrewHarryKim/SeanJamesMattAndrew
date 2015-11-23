using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Effect
{
	public HashSet<string> TriggerTags = new HashSet<string> ();

    public float damageBonus = 0;
    public float armorBonus = 0;
    public int moveRangeBonus = 0;

	public string animBool = "";
	//public float dot = 0;
	public GameObject particlePrefab;




	public delegate void OnHitDelegate(Unit attacker, Unit defender, float amt);
	public OnHitDelegate OnHitDefending = (attacker,defender,amt) => {};
	public OnHitDelegate OnHitAttacking = (attacker,defender,amt) => {};

	public delegate void SingleUnitDelegate(Unit user);
	public SingleUnitDelegate OnEnter = user => {};
	public SingleUnitDelegate OnExit = user => {};

	public SingleUnitDelegate onTurnBegin = user => {};
	public SingleUnitDelegate onTimeTick = user => {};
	public SingleUnitDelegate onTurnEnd = user => {};
	public SingleUnitDelegate onDeath = user => {};

	public Dictionary<Unit,GameObject> particleEffectMap = new Dictionary<Unit, GameObject>();

	public void Initialize(Unit user){
		if(particlePrefab != null){
			GameObject particle = (GameObject)GameObject.Instantiate (particlePrefab, user.transform.position,user.transform.rotation);
			particle.transform.parent = user.transform;
			particleEffectMap [user] = particle;
		}
		if (animBool.Length > 0)
			user.anim.SetBool (animBool, true);
		OnEnter (user);
	}

	public void Remove(Unit user){
		if (particleEffectMap.ContainsKey (user)) {
			GameObject.Destroy(particleEffectMap[user]);
		}
		if (animBool.Length > 0)
			user.anim.SetBool (animBool, false);
		OnExit (user);
	}

}
