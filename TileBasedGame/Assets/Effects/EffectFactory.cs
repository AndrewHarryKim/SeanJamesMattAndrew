using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectFactory{

    private static Effect rootEffect;
    public static Effect getRootEffect()
    {
        if(rootEffect == null)
        {
            rootEffect = new Effect();
            rootEffect.onTurnBegin = user =>
            {
                user.hasMoved = true;
            };
            //rootEffect.animBool = "Dizzy"; If we had a dizzy animaiton and wanted rooted characters to look like that.
        }
        return rootEffect;
    }

    private static Effect stunEffect;
    public static Effect getStunEffect(){

        if (stunEffect == null)
        {
            stunEffect = new Effect();
            stunEffect.onTurnBegin = user =>
            {
                GameManager.instance.ProcessCommand(() => { });
            };
            //stunEffect.animBool = "Dizzy"; character will look whoozy while stunned, if we put in an animation like this.
            //stunEffect.particlePrefab = Resources.Load<GameObject>("EffectParticles/StunParticle");
        }
        return stunEffect;
    }

    private static Effect vampiricEffect;
    public static Effect getVampiricEffect()
    {
        if (vampiricEffect == null)
        {
            vampiricEffect = new Effect();
            vampiricEffect.OnHitAttacking = (attacker, defender, amt) =>
            {
                if(attacker != defender && amt>0)
                    attacker.TakeDamage(-amt,attacker);
            };
        }
        return vampiricEffect;
    }

	private static Effect invincibleEffect;
	public static Effect getInvincibleEffect(){
		
		if (invincibleEffect == null)
		{
			invincibleEffect = new Effect();

		
			invincibleEffect.armorBonus=999;
		}
		return invincibleEffect;
	}

	private static Effect crippleEffect;
	public static Effect getCrippleEffect(){
		
		if (crippleEffect == null)
		{
			crippleEffect = new Effect();
			crippleEffect.moveRangeBonus= -2;
			
			//stunEffect.animBool = "Dizzy"; character will look whoozy while stunned, if we put in an animation like this.
			//stunEffect.particlePrefab = Resources.Load<GameObject>("EffectParticles/StunParticle");
		}
		return crippleEffect;
	}



	private static Effect damageMultiplierEffect;
	public static Effect getDamageMultiplierEffect(){
		
		if (damageMultiplierEffect == null)
		{
			damageMultiplierEffect = new Effect();
			damageMultiplierEffect.damageBonus= 2;
			
			//stunEffect.animBool = "Dizzy"; character will look whoozy while stunned, if we put in an animation like this.
			//stunEffect.particlePrefab = Resources.Load<GameObject>("EffectParticles/StunParticle");
		}
		return damageMultiplierEffect;
	}

	private static Effect weakenOffenseEffect;
	public static Effect GetWeakenOffenseEffect() {
		if (weakenOffenseEffect == null) {
			weakenOffenseEffect = new Effect();
			weakenOffenseEffect.damageBonus = -2;
		}
		return weakenOffenseEffect;
	}

	private static Effect weakenDefenseEffect;
	public static Effect GetWeakenDefenseEffect() {
		if (weakenDefenseEffect == null) {
			weakenDefenseEffect = new Effect();
			weakenDefenseEffect.armorBonus = -1;
		}
		return weakenDefenseEffect;
	}

	private static Effect tauntEffect;
	public static Effect GetTauntEffect() {
		if (tauntEffect == null) {
			tauntEffect = new Effect();
			//TODO: finish dis shiiiit
		}
		return tauntEffect;
	}
}