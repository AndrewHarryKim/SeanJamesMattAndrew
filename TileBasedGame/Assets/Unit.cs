using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

    public Sprite icon;

    private static long idCtr = 0;

    public bool aiControlled = false;

    [HideInInspector]
    public float nextTurnTime = 0;
    [HideInInspector]
    public long turnTieBreaker = 0;
    private long _idNum = -1;
    public long ID
    {
        get
        {
            return _idNum;
        }
    }

    public HashSet<string> talentTags = new HashSet<string>();
    public bool TalentSelected(string str)
    {
        return talentTags.Contains(str);
    }

    private Dictionary<string, int> hardTags = new Dictionary<string, int>();
    public bool HasHardTag(string tag)
    {
        if (!hardTags.ContainsKey(tag))
            return false;
        return hardTags[tag] > 0;
    }
    public void addHardTag(string tag)
    {
        if (!hardTags.ContainsKey(tag))
            hardTags[tag] = 1;
        else
            ++hardTags[tag];
    }
    public void removeHardTag(string tag)
    {
        if (!hardTags.ContainsKey(tag))
            return;
        else if(hardTags[tag] > 0)
            --hardTags[tag];
    }


    public List<SkillContainer> skillContainers = new List<SkillContainer>();
    public void AddSkill(Skill skill)
    {
        skillContainers.Add(new SkillContainer(this, skill));
    }

    public List<EffectContainer> effectContainers = new List<EffectContainer>();
    //negative time for permanent effects.
    public void AddEffect(Effect effect, int time)
    {
        effectContainers.Add(new EffectContainer(this, effect, time));
    }


    [HideInInspector]
    public bool processingCommand = false;
    [HideInInspector]
    public bool hasMoved = false;

    public Animator anim;

    public int faction = 0;
    public bool IsEnemy(Unit other)
    {
        return faction != other.faction;
    }
    public bool IsAlly(Unit other)
    {
        return faction == other.faction;
    }

    public static int turnOrderComp(Unit a, Unit b)
    {
        if (a.nextTurnTime < b.nextTurnTime)
            return -1;
        if (a.nextTurnTime > b.nextTurnTime)
            return 1;
        if (a.turnTieBreaker < b.turnTieBreaker)
            return -1;
        if (a.turnTieBreaker > b.turnTieBreaker)
            return 1;
        if (a.ID < b.ID)
            return -1;
        if (a.ID > b.ID)
            return 1;
        return 0;
    }

    public void TurnTick()
    {
        curMP = Mathf.Clamp(curMP + maxMP / 10f, 0, maxMP);
        for(int i = 0; i < effectContainers.Count;)
        {
            if (effectContainers[i].Tick())
            { //cooldown ran out
                effectContainers[i].effect.Remove(this);
                effectContainers.RemoveAt(i);
            }
            else
                ++i;
        }
        foreach (SkillContainer s in skillContainers)
            s.Tick();
    }


    public int baseMoveRange = 3;
    public int MoveRange
    {
        get
        {
            int total = baseMoveRange;
            foreach(EffectContainer e in effectContainers)
            {
                total += e.effect.moveRangeBonus;
            }
            return System.Math.Max(0, total);
        }
    }


    public float timeForActions = 1f;


    //stats
    public float maxHP, maxMP;
    [HideInInspector]
    public float curHP, curMP;
    public float shield = 0;

    public float baseDamageMultiplier=1;
    public float baseArmor = 0;


    //sum bonsuses from effects here
    public float DamageMultiplier
    {
        get
        {
            float total = baseDamageMultiplier;
            foreach (EffectContainer e in effectContainers)
            {
                total += e.effect.damageBonus;
            }
            return total;
        }
    }

    //sum bonuses from effects here
    public float Armor
    {
        get
        {
            float total = baseArmor;
            foreach (EffectContainer e in effectContainers)
            {
                total += e.effect.armorBonus;
            }
            return Mathf.Min(0.7f,total);
        }
    }

    public delegate float FloatCalculation();

    //amt is a stub
    public void TakeDamage(float amt, Unit attacker)
    {
        attacker.OnAttackAnother(this, amt);
        OnGetHit(attacker, amt);

        if (amt > 0)
        {
            if (shield >= amt)
            {
                shield -= amt;
                return;
            }
            amt -= shield;
            shield = 0;
        }

        curHP = Mathf.Clamp(curHP - amt, 0, maxHP);
        //assuming you can't die during your turn;
        if (curHP <= 0)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
            live = false;
        }
    }

    [HideInInspector]
    public bool live = true;

    public void Death()
    {

    }


    // Use this for initialization
    void Awake () {
        if(_idNum < 0)
            _idNum = idCtr++;
	}

    AnimatorIKProxie ik;
    bool initialized = false;
    void Start()
    {
        initialized = true;
        GameManager.instance.units.Add(this);
        if (tile == null)
        {
            int i = 0;
            while (tile == null)
                tile = GameManager.instance.tiles[i++][(int)ID];
            tile.SetUnit(this);
            transform.position = tile.TopPosition;
            //reachableTiles = GameManager.instance.TilesInRange(tile, MoveRange);
        }
        if (anim == null)
            if (transform.childCount > 0)
            {
                anim = transform.FindChild("Model").GetComponent<Animator>();
                ik = transform.FindChild("Model").GetComponent<AnimatorIKProxie>();
            }

        curHP = maxHP;
        curMP = maxMP;
        explosion = (GameObject)Resources.Load("SpellVisuals/Explosion");

		//AddSkill (SkillFactory.GetWeakenOffense ());
		//AddSkill (SkillFactory.GetWeakenDefense());
		AddSkill(SkillFactory.GetBloodDonor());
		//AddSkill (SkillFactory.GetAoEHeal ());
        AddSkill(SkillFactory.GetSnipe());
        AddSkill(SkillFactory.GetSlam());
		//AddSkill(SkillFactory.GetRepair());

    }

    GameObject explosion;

    void OnDisable()
    {
        GameManager.instance.units.Remove(this);
    }

    void OnEnable()
    {
        if(initialized)
           GameManager.instance.units.Add(this);
    }



    public void RequestCommand()
    {
        if (processingCommand)
            return;
        processingCommand = true;
        hasMoved = false;

        foreach (EffectContainer e in effectContainers)
            e.effect.onTurnBegin(this);

       // if (aiControlled)
       //     calculateMove();

        //AI controlled units will use a coroutine to decide their moves

    }

    //AI ONLY
    public void calculateMovement()
    {
        Unit enemy = GameManager.instance.GetNearestEnemy(this);
        if (enemy == null)
        {
            GameManager.instance.ProcessCommand(() => { });
            return;
        }
        List<Tile> path = GameManager.instance.FindPath(this.tile, enemy.tile);
        if(path == null)
        {
            hasMoved = true;
            return;
        }


        int i = Mathf.Min(MoveRange, path.Count - 1);
        while (i > 0 && path[i].unit != this && path[i].unit != null)
            --i;
        Tile t = path[i];
        GameManager.instance.ProcessMoveCommand(t);
    }
    public void calculateAttack()
    {
        Unit enemy = GameManager.instance.GetNearestEnemy(this);
        if(enemy == null)
        {
            GameManager.instance.ProcessCommand(() => { });
            return;
        }

        List<SkillContainer> castableMoves = new List<SkillContainer>();
        foreach(SkillContainer sc in skillContainers)
            if (sc.IsCastable && sc.skill.targetType == Skill.TargetType.ENEMY && sc.skill.IsInRange(this, enemy.tile))
                castableMoves.Add(sc);

        if(castableMoves.Count <= 0)
        {
            GameManager.instance.ProcessCommand(() => { });
            return;
        }

        SkillContainer move = castableMoves[Random.Range(0, castableMoves.Count)];
        GameManager.instance.ProcessCommand(() => {
            move.skill.Perform(this, enemy.tile);
            move.cooldown = move.skill.cooldown;
        });


        /*
        if (SkillFactory.GetSnipe().IsInRange(this, enemy.tile))
            GameManager.instance.ProcessCommand(() =>
            {
                SkillFactory.GetSnipe().Perform(this, enemy.tile);
            });
        else
            GameManager.instance.ProcessCommand(() => { });
        */
    }



    [HideInInspector]
    public Tile tile;
    List<Tile> list = new List<Tile>();

    HashSet<Tile> reachableTiles = new HashSet<Tile>();

    public void CalculateReachableTiles()
    {
        reachableTiles = GameManager.instance.TilesInRange(tile, MoveRange,this);
    }

    void OnGetHit(Unit attacker, float amt)
    {
        foreach(EffectContainer e in effectContainers)
        {
            e.effect.OnHitDefending(attacker, this, amt);
        }
    }

    void OnAttackAnother(Unit defender, float amt)
    {
        foreach(EffectContainer e in effectContainers)
        {
            e.effect.OnHitAttacking(this, defender, amt);
        }
    }

    SkillContainer aimingSkill = null;
    public bool IsAimingSkill
    {
        get
        {
            return aimingSkill != null;
        }
    }
    public void StopAimingSkill()
    {
        GameManager.instance.cursor.transform.localScale = Vector3.one;
        if (!IsAimingSkill)
            return;
        aimingSkill = null;
        if (!hasMoved)
            CalculateReachableTiles();
        else
            GameManager.instance.TilesInRange(tile, 0, this);
    }


    void CommitSkillTarget()
    {
        GameManager.instance.cursor.transform.localScale = Vector3.one;
        if (!IsAimingSkill)
            return;
        if (GameManager.instance.selected == null ||
            !aimingSkill.skill.IsInRange(this, GameManager.instance.selected))
            return;
        if (aimingSkill.skill.aoe <= 0 && !aimingSkill.skill.ValidTile(this, GameManager.instance.selected))
            return;
        GameManager.instance.ProcessCommand(() =>
        {
            aimingSkill.skill.Perform(this, GameManager.instance.selected);
            aimingSkill.cooldown = aimingSkill.skill.cooldown;
            aimingSkill = null;
        });
    }

    public void SelectSkill(int index)
    {
        if (!processingCommand)
            return;
        if (GameManager.instance.tasks.Count > 0)
            return;
        if (IsAimingSkill)
            return;
        if (index < 0 || index >= skillContainers.Count)
            return;
        SkillContainer s = skillContainers[index];
        if (!s.IsCastable)
            return;
        aimingSkill = s;
        GameManager.instance.TilesInRangeSkill(tile, s.skill.range,this, s.skill);
        GameManager.instance.cursor.transform.localScale = new Vector3((1 + s.skill.aoe), 1, (1 + s.skill.aoe));
    }


	// Update is called once per frame
	void Update () {
     
        
        if (!processingCommand || GameManager.instance.tasks.Count > 0) {
            ik.StopLooking();
            return;
        }

        if (aiControlled)
        {
            if (!hasMoved)
                calculateMovement();
            else
                calculateAttack();
            return;
        }


        if (GameManager.instance.selected)
            ik.LookAt(GameManager.instance.selected.gameObject);
        else
            ik.StopLooking();

        if (Input.GetMouseButtonDown (0) && GameManager.instance.selected != null) {

            if (IsAimingSkill)
                CommitSkillTarget();

            else if (!hasMoved && reachableTiles.Contains(GameManager.instance.selected))
            {
                //if (GameManager.instance.selected.unit && GameManager.instance.selected.unit != this)
                //	attack (GameManager.instance.selected);
                //else
                //	move (GameManager.instance.selected);
                GameManager.instance.ProcessMoveCommand(GameManager.instance.selected);
            }
		}
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSkill(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSkill(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectSkill(2);
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && IsAimingSkill)
        {
            StopAimingSkill();
        }
        else if(!IsAimingSkill && Input.GetKeyDown(KeyCode.P)) //can pass turn
        {
            GameManager.instance.ProcessCommand(() => { });
        }


        /*
        else if (Input.GetKeyDown (KeyCode.Z) && GameManager.instance.selected != null) {
			Skill s = SkillFactory.GetBloodDonor();
			if(s.IsInRange(this,GameManager.instance.selected))
				GameManager.instance.ProcessCommand(()=>s.Perform(this,GameManager.instance.selected));
		}
        */
	}

    void move(Tile t)
    {
        //list = GameManager.instance.FindPath(tile, t);
        ik.StopLooking();
        GameManager.instance.ProcessCommand(() =>
        {
            GameManager.instance.tasks.Add(new Task_MoveToTile(this, t));
        });
    }

    void attack(Tile t)
    {
        ik.StopLooking();
        List<Tile> list = GameManager.instance.FindPath(tile, t);
        GameManager.instance.ProcessCommand(() =>
        {
            if(list.Count >= 2)
                GameManager.instance.tasks.Add(new Task_MoveToTile(this, list[list.Count - 2]));
            GameManager.instance.tasks.Add(new Task_ShowAttack(this, t.unit, "Punch"));
        });
    }
}
