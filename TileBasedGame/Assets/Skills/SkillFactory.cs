using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillFactory
{
    private static Skill bloodDonor = null;
    public static Skill GetBloodDonor()
    {
        if (bloodDonor == null)
        {
            bloodDonor = new Skill();
            bloodDonor.name = "Blood\nDonor";
			bloodDonor.icon = Resources.Load<Sprite>("SpellIcons/bloodDonor");

            bloodDonor.basePower = 3;
            bloodDonor.aoe = 0;
            bloodDonor.range = 4;
			bloodDonor.manaCost = user => 3;
            bloodDonor.cooldown = 3;
			bloodDonor.damageType = Skill.DamageType.DAMAGE;
			bloodDonor.targetType = Skill.TargetType.ENEMY;
            bloodDonor.OnTarget = (user, target, args) =>
            {
                float amt = user.DamageMultiplier * bloodDonor.basePower * (1f - target.Armor);
                target.TakeDamage(amt,user);
                user.TakeDamage(-amt,user);
            };
			bloodDonor.GenerateTasks = (user,tile,args)=>
			{
				List<Unit> list = bloodDonor.gatherTargets(user,tile);
				foreach(Unit target in list){
					GameManager.instance.tasks.Add(new Task_Face_Eachother(user, target));
					GameManager.instance.tasks.Add(new Task_Trigger_Animation(user,"Punch"));
					GameManager.instance.tasks.Add(new Task_Wait(0.3f));
					GameManager.instance.tasks.Add(new Task_Fire_Projectile(user.transform.position+Vector3.up,target.tile.transform.position+Vector3.up,(GameObject)Resources.Load("SpellVisuals/BulletCube")));
					GameManager.instance.tasks.Add(new Task_Trigger_Animation(target,"Hit"));
				}
				bloodDonor.EnqueueExecuteTask(user,tile,args);
			};
        }
        return bloodDonor;
    }

	//MEDIC
	private static Skill heal = null;
	public static Skill GetHeal() {
		if (heal == null) {
			heal = new Skill();
			heal.name = "Heal";
			heal.icon = Resources.Load<Sprite>("SkillIcons/heal");

			heal.basePower = 2;
			heal.aoe = 0;
			heal.range = 6;
			heal.manaCost = user => 1;
			heal.cooldown = 1;
			heal.damageType = Skill.DamageType.HEAL;
			heal.targetType = Skill.TargetType.ALLY;
			heal.OnTarget = (user, target, args) => {
				target.TakeDamage((-1*aoeHeal.basePower), user);
			};
			heal.GenerateTasks = (user, tile, args) => {
				foreach(Unit target in heal.gatherTargets (user, tile)){
					GameManager.instance.tasks.Add(new Task_Face_Eachother(user, target));
					GameManager.instance.tasks.Add(new Task_Trigger_Animation(user,"Punch"));
					GameManager.instance.tasks.Add(new Task_Wait(0.3f));
					GameManager.instance.tasks.Add(new Task_Fire_Projectile(user.transform.position+Vector3.up,target.tile.transform.position+Vector3.up,(GameObject)Resources.Load("SpellVisuals/BulletCube")));
					GameManager.instance.tasks.Add(new Task_Trigger_Animation(target,"Hit"));
				}
				heal.EnqueueExecuteTask(user,tile,args);
			};
		}
		return heal;
	}

	private static Skill aoeHeal = null;
	public static Skill GetAoEHeal() {
		if (aoeHeal == null) {
			aoeHeal = new Skill();
			aoeHeal.name = "AoE\nHeal";
			aoeHeal.icon = Resources.Load<Sprite>("SpellIcons/aoeHeal");

			aoeHeal.basePower = 3;
			aoeHeal.aoe = 3;
			aoeHeal.range = 4;
			aoeHeal.manaCost = user => 6;
			aoeHeal.cooldown = 2;
			aoeHeal.damageType = Skill.DamageType.HEAL;
			aoeHeal.targetType = Skill.TargetType.ALLY;
			aoeHeal.OnTarget = (user, target, args) => {
				target.TakeDamage((-1*aoeHeal.basePower), user);
			};
			aoeHeal.GenerateTasks = (user, title, args) => {
				foreach(Unit target in aoeHeal.gatherTargets (user, title)) {
					GameManager.instance.tasks.Add(new Task_Face_Eachother(user, target));
					GameManager.instance.tasks.Add(new Task_Trigger_Animation(user,"Punch"));
					GameManager.instance.tasks.Add(new Task_Wait(0.3f));
					GameManager.instance.tasks.Add(new Task_Fire_Projectile(user.transform.position+Vector3.up,target.tile.transform.position+Vector3.up,(GameObject)Resources.Load("SpellVisuals/BulletCube")));
					GameManager.instance.tasks.Add(new Task_Trigger_Animation(target,"Hit"));
				}
				aoeHeal.EnqueueExecuteTask(user, title, args);
			};
		}
		return aoeHeal;
	}
	
	private static Skill weakenOffense = null;
	public static Skill GetWeakenOffense() {
		if (weakenOffense == null) {
			weakenOffense = new Skill ();
			weakenOffense.name = "Weaken\nOffense";
			weakenOffense.icon = Resources.Load<Sprite> ("SpellIcons/weakenOffense");

			weakenOffense.basePower = 1;
			weakenOffense.aoe = 0;
			weakenOffense.range = 5;
			weakenOffense.manaCost = user => 1;
			weakenOffense.cooldown = 0;
			weakenOffense.damageType = Skill.DamageType.DAMAGE; 
			weakenOffense.targetType = Skill.TargetType.ENEMY;
			weakenOffense.OnTarget = (user, target, args) => {
				//TODO: wait for Sean
				target.AddEffect (EffectFactory.GetWeakenOffenseEffect (), 3); 
			};
			weakenOffense.GenerateTasks = (user, title, args) => {
				foreach (Unit target in weakenOffense.gatherTargets(user, title)) {
					GameManager.instance.tasks.Add (new Task_Face_Eachother (user, target));
					GameManager.instance.tasks.Add (new Task_Trigger_Animation (user, "Punch"));
					GameManager.instance.tasks.Add (new Task_Wait (0.3f));
					GameManager.instance.tasks.Add (new Task_Fire_Projectile (user.transform.position + Vector3.up, target.tile.transform.position + Vector3.up, (GameObject)Resources.Load ("SpellVisuals/BulletCube")));
					GameManager.instance.tasks.Add (new Task_Trigger_Animation (target, "Hit"));
				}
				weakenOffense.EnqueueExecuteTask (user, title, args);
			};
		}
		return weakenOffense;
	}


	//TANK
    private static Skill slam = null;
    public static Skill GetSlam()
    {
        if(slam == null)
        {
            slam = new Skill();
            slam.name = "Slam";
            slam.icon = Resources.Load<Sprite>("SpellIcons/slam");

            slam.basePower = 4;
            slam.manaCost = user => 4;
            slam.aoe = 1;
            slam.range = 0;
            slam.cooldown = 4;
            slam.damageType = Skill.DamageType.DAMAGE;
            slam.targetType = Skill.TargetType.ENEMY;
            slam.OnTarget = (user, target, args) => {
                bool flag = user.talentTags.Contains("Slam Upgrade");

                float amt = user.DamageMultiplier * bloodDonor.basePower * (1f - target.Armor);
                target.TakeDamage(amt*(flag?2:1), user);
                target.AddEffect(EffectFactory.getStunEffect(), flag?1:2);
            };
            slam.GenerateTasks = (user, tile, args) =>
             {
                 GameManager.instance.tasks.Add(new Task_Trigger_Animation(user, "Punch"));
                 GameManager.instance.tasks.Add(new Task_Wait(0.3f));
                 foreach(Unit target in slam.gatherTargets(user, tile))
                 {
                     GameManager.instance.tasks.Add(new Task_Trigger_Animation(target, "Hit"));
                 }
                 slam.EnqueueExecuteTask(user, tile, args);
             };
        }
        return slam;
    }


	private static Skill weakenDefense = null;
	public static Skill GetWeakenDefense() {
		if (weakenDefense == null) {
			weakenDefense = new Skill();
			weakenDefense.name = "Weakend\nDefense";
			weakenDefense.icon = Resources.Load<Sprite>("SpellIcons/weakenDefense");
			
			weakenDefense.basePower = 2;
			weakenDefense.manaCost = user => 1;
			weakenDefense.aoe = 3;
			weakenDefense.range = 4;
			weakenDefense.cooldown = 0;
			weakenDefense.damageType = Skill.DamageType.DAMAGE;
			weakenDefense.targetType = Skill.TargetType.ENEMY;
			weakenDefense.OnTarget = (user, target, args) => {
				target.AddEffect(EffectFactory.GetWeakenDefenseEffect(), -1);
				
			};
			weakenDefense.GenerateTasks = (user, tile, args) => {				
				foreach(Unit target in weakenDefense.gatherTargets(user, tile)) {
					GameManager.instance.tasks.Add(new Task_Face_Eachother(user, target));
					GameManager.instance.tasks.Add(new Task_Trigger_Animation(user,"Punch"));
					GameManager.instance.tasks.Add(new Task_Wait(0.3f));
					GameManager.instance.tasks.Add(new Task_Fire_Projectile(user.transform.position+Vector3.up,target.tile.transform.position+Vector3.up,(GameObject)Resources.Load("SpellVisuals/BulletCube")));
					GameManager.instance.tasks.Add(new Task_Trigger_Animation(target,"Hit"));
				}
				weakenDefense.EnqueueExecuteTask(user, tile, args);
			};
		}
		return weakenDefense;
	}
	
	private static Skill taunt = null;
	public static Skill GetTaunt() {
		if (taunt == null) {
			taunt = new Skill();
			taunt.name = "Taunt";
			taunt.icon = Resources.Load<Sprite>("SpellIcons/taunt");

			taunt.basePower = 0;
			taunt.aoe = 0;
			taunt.range = 0;
			taunt.manaCost = user => 2;
			taunt.cooldown = 2;
			taunt.damageType = Skill.DamageType.CONDITIONAL;
			taunt.targetType = Skill.TargetType.NONE; 
			taunt.OnTarget = (user, target, args) => {

			};
			taunt.GenerateTasks = (user, title, args) => {
				GameManager.instance.tasks.Add(new Task_Trigger_Animation(user,"Punch"));
				taunt.EnqueueExecuteTask(user, title, args);
			};
		}
		return taunt;
	}

	//---------------//
	//Assassin Skills//
	//---------------//

	private static Skill shiv = null;
	public static Skill GetShiv()
	{
		if (shiv == null)
		{
			shiv = new Skill();
			shiv.name = "Shiv";
			shiv.icon = Resources.Load<Sprite>("SpellIcons/shiv");
			
			shiv.basePower = 1;
			shiv.aoe = 0;
			shiv.range = 1;
			shiv.manaCost = user => 1;
			shiv.cooldown = 0;
			shiv.damageType = Skill.DamageType.DAMAGE;
			shiv.targetType = Skill.TargetType.ENEMY;
			shiv.OnTarget = (user, target, args) =>
			{
				float amt = user.DamageMultiplier * shiv.basePower * (1f - target.Armor);
				target.TakeDamage(amt,user);

			};
			shiv.GenerateTasks = (user,tile,args)=>
			{
				List<Unit> list = shiv.gatherTargets(user,tile);
				foreach(Unit target in list){
					GameManager.instance.tasks.Add(new Task_Face_Eachother(user, target));
					GameManager.instance.tasks.Add(new Task_Trigger_Animation(user,"Punch"));
					GameManager.instance.tasks.Add(new Task_Wait(0.3f));
					GameManager.instance.tasks.Add(new Task_Fire_Projectile(user.transform.position+Vector3.up,target.tile.transform.position+Vector3.up,(GameObject)Resources.Load("SpellVisuals/BulletCube")));
					GameManager.instance.tasks.Add(new Task_Trigger_Animation(target,"Hit"));
				}
				shiv.EnqueueExecuteTask(user,tile,args);
			};
		}
		return shiv;
	}


	private static Skill fade = null;
	public static Skill GetFade()
	{
		if(fade == null)
		{
			fade = new Skill();
			fade.name = "Fade";
			fade.icon = Resources.Load<Sprite>("SpellIcons/fade");
			
			fade.basePower = 0;
			fade.manaCost = user => 1;
			fade.aoe = 0;
			fade.range = 0;
			fade.cooldown = 6;
			fade.damageType = Skill.DamageType.HEAL;
			fade.targetType = Skill.TargetType.SELF;
			fade.OnTarget = (user, target, args) => {
				//bool flag = user.talentTags.Contains("Fade Upgrade");
				target.AddEffect(EffectFactory.getInvincibleEffect(),3);

			};
			fade.GenerateTasks = (user, tile, args) =>
			{
				GameManager.instance.tasks.Add(new Task_Trigger_Animation(user, "Punch"));
				GameManager.instance.tasks.Add(new Task_Wait(0.3f));
			
				fade.EnqueueExecuteTask(user, tile, args);
			};
		}
		return fade;
	}


	private static Skill cripple = null;
	public static Skill GetCripple()
	{
		if (cripple == null)
		{
			cripple = new Skill();
			cripple.name = "Cripple";
			cripple.icon = Resources.Load<Sprite>("SpellIcons/cripple");
			
			cripple.basePower = 1;
			cripple.aoe = 0;
			cripple.range = 4;
			cripple.manaCost = user => 3;
			cripple.cooldown = 0;
			cripple.damageType = Skill.DamageType.DAMAGE;
			cripple.targetType = Skill.TargetType.ENEMY;
			cripple.OnTarget = (user, target, args) =>
			{
				float amt = user.DamageMultiplier * cripple.basePower * (1f - target.Armor);
				target.TakeDamage(amt,user);
				target.AddEffect(EffectFactory.getCrippleEffect(), 3);
			};
			cripple.GenerateTasks = (user,tile,args)=>
			{
				List<Unit> list = cripple.gatherTargets(user,tile);
				foreach(Unit target in list){
					GameManager.instance.tasks.Add(new Task_Face_Eachother(user, target));
					GameManager.instance.tasks.Add(new Task_Trigger_Animation(user,"Punch"));
					GameManager.instance.tasks.Add(new Task_Wait(0.3f));
					GameManager.instance.tasks.Add(new Task_Fire_Projectile(user.transform.position+Vector3.up,target.tile.transform.position+Vector3.up,(GameObject)Resources.Load("SpellVisuals/BulletCube")));
					GameManager.instance.tasks.Add(new Task_Trigger_Animation(target,"Hit"));
				}
				cripple.EnqueueExecuteTask(user,tile,args);
			};
		}
		return cripple;
	}
	//------------//
	// Tech Skills//
	//------------//
	private static Skill snipe = null;
	public static Skill GetSnipe()
	{
		if (snipe == null)
		{
			snipe = new Skill();
			snipe.name = "Snipe";
			snipe.icon = Resources.Load<Sprite>("SpellIcons/snipe");
			
			snipe.basePower = 6;
			snipe.aoe = 0;
			snipe.range = 4;
			snipe.manaCost = user => 6;
			snipe.cooldown = 0;
			snipe.damageType = Skill.DamageType.DAMAGE;
			snipe.targetType = Skill.TargetType.ENEMY;
			snipe.OnTarget = (user, target, args) =>
			{
				float amt = user.DamageMultiplier * snipe.basePower * (1f - target.Armor);
				target.TakeDamage(amt,user);
				
			};
			snipe.GenerateTasks = (user,tile,args)=>
			{
				List<Unit> list = snipe.gatherTargets(user,tile);
				foreach(Unit target in list){
					GameManager.instance.tasks.Add(new Task_Face_Eachother(user, target));
					GameManager.instance.tasks.Add(new Task_Trigger_Animation(user,"Punch"));
					GameManager.instance.tasks.Add(new Task_Wait(0.3f));
					GameManager.instance.tasks.Add(new Task_Fire_Projectile(user.transform.position+Vector3.up,target.tile.transform.position+Vector3.up,(GameObject)Resources.Load("SpellVisuals/BulletCube")));
					GameManager.instance.tasks.Add(new Task_Trigger_Animation(target,"Hit"));
				}
				snipe.EnqueueExecuteTask(user,tile,args);
			};
		}
		return snipe;
	}

	private static Skill repair = null;
	public static Skill GetRepair()
	{
		if (repair == null)
		{
			repair = new Skill();
			repair.name = "Repair";
			repair.icon = Resources.Load<Sprite>("SpellIcons/repair");
			
			repair.basePower = 2;
			repair.aoe = 0;
			repair.range = 4;
			repair.manaCost = user => 2;
			repair.cooldown = 0;
			repair.damageType = Skill.DamageType.HEAL;
			repair.targetType = Skill.TargetType.ALLY;
			repair.OnTarget = (user, target, args) =>
			{
				float amt = -user.DamageMultiplier * repair.basePower ;
				target.TakeDamage(amt,user);
				target.AddEffect(EffectFactory.getDamageMultiplierEffect(),3);
			};
			repair.GenerateTasks = (user,tile,args)=>
			{
				List<Unit> list = repair.gatherTargets(user,tile);
				foreach(Unit target in list){
					GameManager.instance.tasks.Add(new Task_Face_Eachother(user, target));
					GameManager.instance.tasks.Add(new Task_Trigger_Animation(user,"Punch"));
					GameManager.instance.tasks.Add(new Task_Wait(0.3f));
					GameManager.instance.tasks.Add(new Task_Fire_Projectile(user.transform.position+Vector3.up,target.tile.transform.position+Vector3.up,(GameObject)Resources.Load("SpellVisuals/BulletCube")));
					GameManager.instance.tasks.Add(new Task_Trigger_Animation(target,"Hit"));
				}
				repair.EnqueueExecuteTask(user,tile,args);
			};
		}
		return repair;
	}


	
	
	
}