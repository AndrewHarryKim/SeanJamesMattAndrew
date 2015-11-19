using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class GameManager : MonoBehaviour {

    public GameObject tile;

    private float tileH = 0.1039212f;
    private float HexMaxCirc = 2 / Mathf.Sqrt(3);
    private Vector3 NE,NW,SE,SW,E,W;

    //public Material redmat;

    public GameObject cursor;

    public int width=10, height=20;

	// Use this for initialization
	void CreateGrid () {
        NE = new Vector3(1f/2f, 0, 1.5f/Mathf.Sqrt(3));
        NW = new Vector3(-1f / 2f, 0, 1.5f / Mathf.Sqrt(3));
        SE = new Vector3(1f / 2f, 0, -1.5f / Mathf.Sqrt(3));
        SW = new Vector3(-1f / 2f, 0, -1.5f / Mathf.Sqrt(3));
        E = new Vector3(1f, 0, 0);
        W = new Vector3(-1f, 0, 0);
        //NE.Normalize();

        /*
        for (int i = 0; i < 5; ++i)
        {
            Instantiate(tile, Vector3.right * i, Quaternion.identity);
        }
        for (int i = 0; i < 5; ++i)
        {
            Instantiate(tile, Vector3.right * i+NE, Quaternion.identity);
        }
        for (int i = 0; i < 5; ++i)
        {
            Instantiate(tile, Vector3.right * i + NE+NW, Quaternion.identity);
        }
        */
        GameManager.instance.SetupList(width+1, height);

        Vector3 row = new Vector3();
        for(int i = 0; i < height;)
        {
            for(int j = 0; j < width; ++j)
            {
                if (Random.value <= 0.2f)
                    continue;
                GameObject go = (GameObject)Instantiate(tile, row + Vector3.right * j, Quaternion.identity);
                if(Random.value <= 0.3f)
                {
                    go.GetComponent<MeshRenderer>().material = walkSelectMat;
                }
                //Debug.Log("I: " + i + " J: " + j);
                Tile t = go.GetComponent<Tile>();
                t.gridPos = new Tile.TilePos(j, i);
                GameManager.instance.RegisterTile(t);
                
                if(Random.value <= 0.1f)
                {
                    go.transform.localScale = new Vector3(1, 2, 1);
                    go.transform.position += Vector3.up*tileH/2;
                }
                /*
                if(Random.value <= 0.1f)
                {
                    GameObject g = (GameObject)Instantiate(tile, row + Vector3.right * j + Vector3.up * tileH, Quaternion.identity);
                    if (Random.value <= 0.3f)
                    {
                        g.GetComponent<MeshRenderer>().material = redmat;
                    }
                }
                */
            }
            row += NE;
            if (++i >= height)
                break;
            for (int j = 0; j < width; ++j)
            {
                if (Random.value <= 0.2f)
                    continue;
                GameObject go = (GameObject)Instantiate(tile, row + Vector3.right * j, Quaternion.identity);
                if (Random.value <= 0.3f)
                {
                    go.GetComponent<MeshRenderer>().material = walkSelectMat;
                }
                //Debug.Log("I: " + i + " J: " + j);
                Tile t = go.GetComponent<Tile>();
                t.gridPos = new Tile.TilePos(j, i);
                GameManager.instance.RegisterTile(t);
                if (Random.value <= 0.1f)
                {
                    go.transform.localScale = new Vector3(1, 2, 1);
                    go.transform.position += Vector3.up * tileH/2;
                }
                /*
                if (Random.value <= 0.1f)
                {
                    GameObject g = (GameObject)Instantiate(tile, row + Vector3.right * j + Vector3.up * tileH, Quaternion.identity);
                    if (Random.value <= 0.3f)
                    {
                        g.GetComponent<MeshRenderer>().material = redmat;
                    }
                }
                */
                
            }
            row += NW;
            ++i;
        }

        /*
        List<Tile> list = GameManager.instance.FindPath(GameManager.instance.tiles[4][4], GameManager.instance.tiles[7][18]);
        if(list != null)
        {
            int i = 0;
            foreach(Tile t in list)
            {
                t.gameObject.GetComponent<MeshRenderer>().material = redmat;
                //t.text.text = (i++).ToString();
            }
        }
        */
        
    }

    int tileLayer = 1 << 8;
	// Update is called once per frame
	void AnotherUpdate () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayer))
        {
            cursor.SetActive(true);
            //hit.collider.gameObject.GetComponent<MeshRenderer>().material = redmat;
            cursor.transform.position = hit.collider.gameObject.transform.position + Vector3.up * 0.1f * hit.collider.gameObject.transform.localScale.y;
            GameManager.instance.selected = hit.collider.gameObject.GetComponent<Tile>();
        }
        else
        {
            cursor.SetActive(false);
            GameManager.instance.selected = null;
        }
	}

}
