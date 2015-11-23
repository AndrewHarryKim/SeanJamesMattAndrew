using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour {

    public class TilePos
    {
        public int x, y;
        public float distance(TilePos other)
        {
            return (new Vector2(x - other.x, y - other.y)).magnitude;
        }
        public static TilePos operator +(TilePos a, TilePos b)
        {
            return new TilePos(a.x + b.x, a.y + b.y);
        }

        public TilePos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public TilePos()
        {

        }
    }

    private const float tileH = 0.1039212f;

    public Vector3 TopPosition
    {
        get
        {
            return transform.position + Vector3.up * tileH * transform.localScale.y;
        }
    }

    public TilePos gridPos = new TilePos();

    public int gridX
    {
        get
        {
            return gridPos.x;
        }
        
    }

    public int gridY
    {
        get
        {
            return gridPos.y;
        }
    }

    private Unit _unit = null;
    public Unit unit
    {
        get
        {
            return _unit;
        }
    }


    /*
    public Vector2 gridPos = new Vector2();
    public int gridX
    {
        get
        {
            return (int)gridPos.x;
        }
        set
        {
            gridPos.x = (int)value;
        }
    }
    public int gridY
    {
        get
        {
            return (int)gridPos.y;
        }
        set
        {
            gridPos.y = (int)value;
        }
    }
    */

    public bool IsOffset()
    {
        return gridY % 2 == 1;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void SetUnit(Unit unit)
    {
        //if (unit.tile == this)
        //    return;
        if (unit.tile != null)
            unit.tile._unit = null;
        unit.tile = this;
        this._unit = unit;
    }


    public HashSet<Aura> auras = new HashSet<Aura>();

}
