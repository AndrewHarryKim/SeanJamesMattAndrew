using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public partial class GameManager : MonoBehaviour {
    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    void Awake()
    {
        instance = this;
        CreateGrid();
    }

    // Use this for initialization
    void Start()
    {
        foreach (ClassFactory cf in GlobalManager.instance.PartyFactories)
        {
            Unit u = cf.Generate();
            //unitShell.AddComponent<>
        }
    }

    [HideInInspector]
    public List<Task> tasks = new List<Task>();

    public bool HasTask
    {
        get
        {
            return tasks.Count > 0;
        }
    }

    public TempActionBarUI tempActionBar;
    public TempTurnQueueUI tempTurnQueueBar;

    // Update is called once per frame
    void Update()
    {
        if (tempTurnQueueBar.buttons.Count == 0)
            tempTurnQueueBar.Initialize(units);

        AnotherUpdate();
        if (HasTask)
        {
            if (tasks[0].update())
            {
                tasks[0].OnExit();
                tasks.RemoveAt(0);
            }
        }
        else if (activeUnit == null)
        {
            GetNextActiveUnit();
            tempActionBar.LoadUnit(activeUnit);
            tempTurnQueueBar.NewTurn(units);
            activeUnit.CalculateReachableTiles();
            SelectionParticle.GetComponent<ParticleSystem>().enableEmission = true;
            SelectionParticle.transform.position = activeUnit.transform.position;
            SelectionParticle.transform.parent = activeUnit.transform;
            activeUnit.RequestCommand();
        }
    }

    public void ProcessMoveCommand(Tile t)
    {
        if (activeUnit == null || activeUnit.hasMoved)
            return;
        tasks.Add(new Task_MoveToTile(activeUnit, t));
        activeUnit.hasMoved = true;
        //action();
        foreach (List<Tile> row in tiles)
        {
            foreach (Tile tile in row)
            {
                if (tile != null)
                    tile.GetComponent<Renderer>().material = defaultMat;
            }
        }
    }

    public void ProcessCommand(Action action)
    {
        if (activeUnit == null)
            return;
        activeUnit.processingCommand = false;
        action();
        foreach (List<Tile> row in tiles)
        {
            foreach (Tile tile in row)
            {
                if (tile != null)
                    tile.GetComponent<Renderer>().material = defaultMat;
            }
        }
        activeUnit.nextTurnTime += activeUnit.timeForActions;
        SelectionParticle.GetComponent<ParticleSystem>().enableEmission = false;
        foreach (EffectContainer e in activeUnit.effectContainers)
            e.effect.onTurnEnd(activeUnit);
        activeUnit = null;
    }

    float TurnTime = 0f;
    Unit activeUnit = null;
    public List<Unit> units;

    public GameObject SelectionParticle;


    private long turnTakenCtr = 0;
    private void GetNextActiveUnit()
    {
        if (units.Count == 0)
            return;
        units.Sort((a, b) => Unit.turnOrderComp(a, b));
        while (units[0].nextTurnTime > TurnTime)
        {
            TurnTime += 1f;
            foreach (Unit u in units)
                u.TurnTick();
        }
        activeUnit = units[0];
        activeUnit.turnTieBreaker = turnTakenCtr++;
    }


    public void RemoveUnit()
    {

    }

    //Tile Stuff Below Here
    [HideInInspector]
    public int MapH, MapW;
    public Dictionary<GameObject, Tile> tileMap = new Dictionary<GameObject, Tile>();
    public List<List<Tile>> tiles = new List<List<Tile>>();

    public void SetupList(int w, int h)
    {
        MapH = h;
        MapW = w;
        for (int i = 0; i < w; ++i)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < h; ++j)
                row.Add(null);
            tiles.Add(row);
        }
    }

    public bool inBounds(int x, int y)
    {
        return x >= 0 && x < MapW && y >= 0 && y < MapH;
    }

    public bool tileExists(int x, int y)
    {
        return inBounds(x, y) && tiles[x][y] != null;
    }

    public Tile getTile(int x, int y)
    {
        if (tileExists(x, y))
            return tiles[x][y];
        return null;
    }

    public Tile getTile(Vector2 pos)
    {
        return getTile((int)pos.x, (int)pos.y);
    }

    public Tile getTile(Tile.TilePos pos)
    {
        return getTile(pos.x, pos.y);
    }

    public Tile.TilePos[] directions = { new Tile.TilePos(-1, 0), new Tile.TilePos(1, 0), new Tile.TilePos(-1, -1), new Tile.TilePos(-1, 1), new Tile.TilePos(0, 1), new Tile.TilePos(0, -1) };
    public Tile.TilePos[] offsetDirections = { new Tile.TilePos(-1, 0), new Tile.TilePos(1, 0), new Tile.TilePos(0, 1), new Tile.TilePos(0, -1), new Tile.TilePos(1, 1), new Tile.TilePos(1, -1) };


    public List<Tile> getNeighbors(Tile tile)
    {
        List<Tile> list = new List<Tile>();
        bool offset = tile.IsOffset();
        foreach (Tile.TilePos dir in offset ? offsetDirections : directions)
        {
            Tile t = getTile(tile.gridPos + dir);
            if (t != null)
                list.Add(t);
        }

        return list;
    }

    public List<Tile.TilePos> getNeighborsNoBarrier(Tile.TilePos pos)
    {
        List<Tile.TilePos> list = new List<Tile.TilePos>();
        bool offset = pos.y % 2 == 1;
        foreach(Tile.TilePos dir in offset ? offsetDirections : directions)
        {
            list.Add(pos + dir);
        }
        return list;
    }



    public void RegisterTile(Tile tile)
    {
        tileMap[tile.gameObject] = tile;
        tiles[tile.gridX][tile.gridY] = tile;
    }



    private class PathNode
    {
        public PathNode prev = null;
        public Tile tile;

        public float g = 0;
        public float h = 0;
        public float f
        {
            get
            {
                return g + h;
            }
        }

        public PathNode(Tile t, Tile end, PathNode pred = null)
        {
            tile = t;
            h = t.gridPos.distance(end.gridPos);
            if (pred != null)
                setPrev(pred);
            else
                this.g = 0;
        }

        public void setPrev(PathNode prev)
        {
            this.prev = prev;
            g = prev.g + 1;
        }

        public bool isImprovement(PathNode pred)
        {
            return pred.g + 1 <= g;
        }
    }

    public List<Tile> FindPath(Tile start, Tile end)
    {
        if (start == null || end == null)
            return null;

        Dictionary<Tile, PathNode> nodeMap = new Dictionary<Tile, PathNode>();
        List<PathNode> openset = new List<PathNode>();
        HashSet<Tile> closedset = new HashSet<Tile>();


        openset.Add(new PathNode(start, end));
        nodeMap[start] = openset[0];

        while (openset.Count > 0)
        {
            openset.Sort(delegate (PathNode a, PathNode b)
            {
                if (a.f == b.f)
                    return 0;
                return a.f < b.f ? -1 : 1;
            });
            PathNode current = openset[0];
            closedset.Add(current.tile);
            openset.RemoveAt(0);
            if (current.tile == end)
            {
                List<Tile> list = new List<Tile>();
                Stack<Tile> stack = new Stack<Tile>();
                while (current != null)
                {
                    stack.Push(current.tile);
                    current = current.prev;
                }
                while (stack.Count > 0)
                {
                    list.Add(stack.Pop());
                }

                return list;
            }

            foreach (Tile tile in getNeighbors(current.tile))
            {
                //if (closedset.Contains(tile))
                //    continue;
                if (tile.unit != null && tile != end)
                    continue;


                if (nodeMap.ContainsKey(tile))
                {
                    if (nodeMap[tile].isImprovement(current)) {
                        nodeMap[tile].setPrev(current);
                        //Might be needed since I did not chech heuristic for constancy.
                        if (closedset.Contains(tile)) {
                            closedset.Remove(tile);
                            openset.Add(nodeMap[tile]);
                        }
                    }
                }
                else
                {
                    PathNode node = new PathNode(tile, end, current);
                    nodeMap[tile] = node;
                    openset.Add(node);
                }
            }

        }



        return null;
    }

    public HashSet<Tile> TilesInRange(Tile t, int range, Unit agent)
    {
        HashSet<Tile> set = new HashSet<Tile>();
        DLS(t, set, range, agent);

        //this isn't the actual place to put them
        foreach (List<Tile> row in tiles)
        {
            foreach (Tile tile in row)
            {
                if (tile != null)
                {
                    Material mat = defaultMat;
                    if (set.Contains(tile))
                        mat = tile.unit && tile.unit != activeUnit ? attackMat : walkSelectMat;
                    tile.GetComponent<Renderer>().material = mat;
                }
            }
        }

        return set;
    }

    public HashSet<Tile> TilesInRangeSkill(Tile t, int range, Unit agent, Skill skill)
    {
        HashSet<Tile> set = new HashSet<Tile>();
        if(t!=null)
            DLS_NoBarrier(t.gridPos, set, range, null);

        //this isn't the actual place to put them
        foreach (List<Tile> row in tiles)
        {
            foreach (Tile tile in row)
            {
                if (tile != null)
                {
                    Material mat = defaultMat;
                    if (set.Contains(tile))
                        mat = skill.ValidTile(agent, tile) ? (tile.unit != null && agent.IsAlly(tile.unit) ? allySelectMat : attackMat) : walkSelectMat;
                    tile.GetComponent<Renderer>().material = mat;
                }
            }
        }

        return set;
    }

    public Material walkSelectMat;
    public Material defaultMat;
    public Material attackMat;
    public Material allySelectMat;

    private void DLS(Tile t, HashSet<Tile> set, int depth, Unit agent)
    {
        if (depth < 0)
            return;
        if (t.unit && agent && t.unit != agent)
            return;
        set.Add(t);

        foreach (Tile tile in getNeighbors(t))
        {
            //if (set.Contains(tile))
            //    continue;
            DLS(tile, set, depth - 1, agent);
        }
    }


    private void DLS_NoBarrier(Tile.TilePos pos, HashSet<Tile> set, int depth, Unit agent)
    {
        if (depth < 0)
            return;
        if (inBounds(pos.x,pos.y) && tiles[pos.x][pos.y] != null)
            set.Add(tiles[pos.x][pos.y]);

        foreach (Tile.TilePos p in getNeighborsNoBarrier(pos))
            DLS_NoBarrier(p, set, depth-1, agent);
    }




    public delegate bool GoalTest(Tile tile);
    public Tile FindTileF(Tile start, GoalTest test)
    {
        return BFS(start, test);
    }

    private Tile BFS(Tile start, GoalTest test)
    {
        HashSet<Tile> closedset = new HashSet<Tile>();
        Queue<Tile> openqueue = new Queue<Tile>();

        openqueue.Enqueue(start);
        while(openqueue.Count > 0)
        {
            Tile current = openqueue.Dequeue();
            closedset.Add(current);
            if (test(current))
                return current;
            foreach (Tile t in getNeighbors(current))
                if (!closedset.Contains(t))
                    openqueue.Enqueue(t);
        }
        return null;
    }

    public Unit GetNearestEnemy(Unit unit)
    {
        Tile t = FindTileF(unit.tile, tile =>
        {
            return tile.unit != null && unit.IsEnemy(tile.unit);
        });
        return t == null ? null : t.unit;
    }

    public Unit GetNearestAlly(Unit unit)
    {
        Tile t = FindTileF(unit.tile, tile =>
        {
            return tile.unit != null && tile.unit != unit && unit.IsAlly(tile.unit);
        });
        return t == null ? null : t.unit;
    }

    public Tile selected;



    public HashSet<Tile> paintedTiles = new HashSet<Tile>();
    public void PaintTile(Tile t, Material mat)
    {
        t.GetComponent<Renderer>().material = mat;
        paintedTiles.Add(t);
    }
    public void clearPaintedTiles()
    {
        foreach (Tile t in paintedTiles)
            t.GetComponent<Renderer>().material = defaultMat;
        paintedTiles.Clear();
    }
    public void PaintMoveChoice()
    {
        if (activeUnit == null)
            return;
        //activeUnit.tile
    }

}
