using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Talent
{
    public bool toggle = false;
    public string name;
    public string description;
    public Sprite icon;

    public Talent()
    {
        icon = Resources.Load<Sprite>("SpellIcons/llama");
    }

    public delegate void AlterCharacterDelegate(Unit unit);
    public AlterCharacterDelegate IfChosen = unit => { };

}

public abstract class ClassFactory
{

    public string name;
    public string description;
    public List<Talent> talentOptions = new List<Talent>();

    public List<Skill> baseSkills = new List<Skill>();

    public GameObject baseModel;

    public float attackPower, armor, maxHP, maxMP;
    public Sprite image;

    public ClassFactory()
    {
        baseModel = Resources.Load <GameObject> ("basemodel");
    }

    public Unit Generate()
    {
        GameObject model = (GameObject)GameObject.Instantiate(baseModel);
        Unit u = model.AddComponent<Unit>();
        u.curHP = u.maxHP = maxHP;
        u.curMP = u.maxMP = maxMP;
        u.baseDamageMultiplier = attackPower;
        u.baseArmor = armor;
        u.icon = image;

        foreach (Skill s in baseSkills)
            u.AddSkill(s);

        foreach (Talent t in talentOptions)
            if (t.toggle)
                t.IfChosen(u);

        return u;
    }


}

public class TankFactory : ClassFactory
{

    public TankFactory()
    {
        baseModel = Resources.Load<GameObject>("TankModel");

        attackPower = 0.9f;
        armor = 0.4f;
        maxHP = 7;
        maxMP = 6;


        baseSkills.Add(SkillFactory.GetSlam());
        baseSkills.Add(SkillFactory.GetWeakenDefense());
        baseSkills.Add(SkillFactory.GetTaunt());

        Talent t = new Talent();
        t.name = "Add Blood Donor";
        t.description = "Adds Blood Donor to Class";
        t.IfChosen = unit => {
            unit.AddSkill(SkillFactory.GetBloodDonor());
        };
        talentOptions.Add(t);

        t = new Talent();
        t.name = "Add Repair";
        t.description = "Adds Repair to Class";
        t.IfChosen = unit =>
        {
            unit.AddSkill(SkillFactory.GetRepair());
        };
		talentOptions.Add(t);

        t = new Talent();
        t.name = "Improve Skill X";
        t.description = "Makes Skill X better. Skill X is scripted to check if the performing agent has the string 'SkillXImprove' in its talentTags";
        t.IfChosen = unit =>
        {
            unit.talentTags.Add("SkillXImprove");
        };
		talentOptions.Add(t);

        t = new Talent();
        t.name = "Vampiric Attacks";
        t.description = "Your attacks restore your health";
        t.IfChosen = unit =>
        {
            unit.AddEffect(EffectFactory.getVampiricEffect(),-1); //-1 is permanent
        };
		talentOptions.Add(t);

        t = new Talent();
        t.name = "Useless 1";
        t.IfChosen = unit =>
        {

        };
        talentOptions.Add(t);

        t = new Talent();
        t.name = "Useless 2";
        t.IfChosen = Unit =>
        {

        };
		talentOptions.Add(t);

        name = "Tank";
        description = "A tank. Deals and takes little damage. Hinders enemies to protect his team.";
        image = Resources.Load<Sprite>("CharacterIcons/TankIcon");
    }

}

public class AssassinFactory : ClassFactory
{

    public AssassinFactory()
    {
        baseModel = Resources.Load<GameObject>("AssassinModel");

        attackPower = 1.2f;
        armor = 0.1f;
        maxHP = 6;
        maxMP = 6;


        baseSkills.Add(SkillFactory.GetShiv());
        baseSkills.Add(SkillFactory.GetFade());
        baseSkills.Add(SkillFactory.GetCripple());

        Talent t = new Talent();
        t.name = "Add Blood Donor";
        t.description = "Adds Blood Donor to Class";
        t.IfChosen = unit => {
            unit.AddSkill(SkillFactory.GetBloodDonor());
        };
        talentOptions.Add(t);

        t = new Talent();
        t.name = "Add Repair";
        t.description = "Adds Repair to Class";
        t.IfChosen = unit =>
        {
            unit.AddSkill(SkillFactory.GetRepair());
        };
		talentOptions.Add(t);

        t = new Talent();
        t.name = "Improve Skill X";
        t.description = "Makes Skill X better. Skill X is scripted to check if the performing agent has the string 'SkillXImprove' in its talentTags";
        t.IfChosen = unit =>
        {
            unit.talentTags.Add("SkillXImprove");
        };
		talentOptions.Add(t);

        t = new Talent();
        t.name = "Vampiric Attacks";
        t.description = "Your attacks restore your health";
        t.IfChosen = unit =>
        {
            unit.AddEffect(EffectFactory.getVampiricEffect(), -1); //-1 is permanent
        };
		talentOptions.Add(t);

        t = new Talent();
        t.name = "Useless 1";
        t.IfChosen = unit =>
        {

        };
        talentOptions.Add(t);

        t = new Talent();
        t.name = "Useless 2";
        t.IfChosen = Unit =>
        {

        };
		talentOptions.Add(t);

        name = "Assassin";
        description = "A spooky dangerous assassin, who deals a lot of damage";
        image = Resources.Load<Sprite>("CharacterIcons/AssassinIcon");
    }

}

public class MedicFactory : ClassFactory
{

    public MedicFactory()
    {
        baseModel = Resources.Load<GameObject>("MedicModel");

        attackPower = 0.9f;
        armor = 0.4f;
        maxHP = 4;
        maxMP = 6;


        baseSkills.Add(SkillFactory.GetHeal());
        baseSkills.Add(SkillFactory.GetAoEHeal());
        baseSkills.Add(SkillFactory.GetWeakenOffense());

        Talent t = new Talent();
        t.name = "Add Blood Donor";
        t.description = "Adds Blood Donor to Class";
        t.IfChosen = unit => {
            unit.AddSkill(SkillFactory.GetBloodDonor());
        };
        talentOptions.Add(t);

        t = new Talent();
        t.name = "Add Repair";
        t.description = "Adds Repair to Class";
        t.IfChosen = unit =>
        {
            unit.AddSkill(SkillFactory.GetRepair());
        };
		talentOptions.Add(t);

        t = new Talent();
        t.name = "Improve Skill X";
        t.description = "Makes Skill X better. Skill X is scripted to check if the performing agent has the string 'SkillXImprove' in its talentTags";
        t.IfChosen = unit =>
        {
            unit.talentTags.Add("SkillXImprove");
        };
		talentOptions.Add(t);

        t = new Talent();
        t.name = "Vampiric Attacks";
        t.description = "Your attacks restore your health";
        t.IfChosen = unit =>
        {
            unit.AddEffect(EffectFactory.getVampiricEffect(), -1); //-1 is permanent
        };
		talentOptions.Add(t);

        t = new Talent();
        t.name = "Useless 1";
        t.IfChosen = unit =>
        {

        };
        talentOptions.Add(t);

        t = new Talent();
        t.name = "Useless 2";
        t.IfChosen = Unit =>
        {

        };
		talentOptions.Add(t);

        name = "Medic";
        description = "A doctor who supports his team with his healing powers.";
        image = Resources.Load<Sprite>("CharacterIcons/MedicIcon");
    }

}

public class TechFactory : ClassFactory
{

    public TechFactory()
    {
        baseModel = Resources.Load<GameObject>("TechModel");

        attackPower = 0.9f;
        armor = 0.4f;
        maxHP = 4;
        maxMP = 6;


        baseSkills.Add(SkillFactory.GetSnipe());
        baseSkills.Add(SkillFactory.GetRepair());
        baseSkills.Add(SkillFactory.GetMakeSentry());

        Talent t = new Talent();
        t.name = "Add Blood Donor";
        t.description = "Adds Blood Donor to Class";
        t.IfChosen = unit => {
            unit.AddSkill(SkillFactory.GetBloodDonor());
        };
        talentOptions.Add(t);

        t = new Talent();
        t.name = "Add Repair";
        t.description = "Adds Repair to Class";
        t.IfChosen = unit =>
        {
            unit.AddSkill(SkillFactory.GetRepair());
        };
		talentOptions.Add(t);

        t = new Talent();
        t.name = "Improve Skill X";
        t.description = "Makes Skill X better. Skill X is scripted to check if the performing agent has the string 'SkillXImprove' in its talentTags";
        t.IfChosen = unit =>
        {
            unit.talentTags.Add("SkillXImprove");
        };
		talentOptions.Add(t);

        t = new Talent();
        t.name = "Vampiric Attacks";
        t.description = "Your attacks restore your health";
        t.IfChosen = unit =>
        {
            unit.AddEffect(EffectFactory.getVampiricEffect(), -1); //-1 is permanent
        };
		talentOptions.Add(t);

        t = new Talent();
        t.name = "Useless 1";
        t.IfChosen = unit =>
        {

        };
        talentOptions.Add(t);

        t = new Talent();
        t.name = "Useless 2";
        t.IfChosen = Unit =>
        {

        };
		talentOptions.Add(t);

        name = "Technician";
        description = "A repairman who can build robots";
        image = Resources.Load<Sprite>("CharacterIcons/TechIcon");
    }

}