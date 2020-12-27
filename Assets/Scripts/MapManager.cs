using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
public class MapManager : MonoBehaviour
{
    private Tilemap mapFront;
    private Tilemap mapMiddle;
    private Tilemap mapBackground;
    public int Height;
    public int Width;
    private Vector3Int[] back_positions;
    private TileBase[] tilesfront;
    private TileBase[] tilesmiddle;
    private TileBase tilebackground;
    private List<Vector3Int[]> GrassFieldPos;//存放每个GrassField的左下角和右上角
    private Dictionary<string, Vector3Int[]> ContinentPosDictionary = new Dictionary<string, Vector3Int[]>();//存放每个大陆的左下角和右上角坐标信息
    TileBase Ground1;
    TileBase Ground2;
    TileBase Grass1;
    TileBase Grass2;
    TileBase Grass3;
    TileBase Water0;
    TileBase[] GrassField = new TileBase[8];//存放grassfield的tiles
    private Dictionary<string, TileBase> itemDictionary = new Dictionary<string, TileBase>();
    MapInfo mapInfo;
    [SerializeField]
    public int night = 0;
    class MapInfo
    {
        //public Vector3Int[] Green_positions;
        public Dictionary<string, Vector3Int[]> ContinentVecs = new Dictionary<string, Vector3Int[]>();
    }
    private void Awake()
    {
        mapFront = transform.Find("../GameCanvas/FrontMap/Tilemap").gameObject.GetComponent<Tilemap>();
        mapMiddle = transform.Find("../GameCanvas/MiddleMap/Tilemap").GetComponent<Tilemap>();
        mapBackground = transform.Find("../GameCanvas/BackgroundMap/Tilemap").GetComponent<Tilemap>();
        back_positions = new Vector3Int[Height * Width];
        tilesfront = new TileBase[Height * Width];
        GrassFieldPos = new List<Vector3Int[]>();
        GrassField = new TileBase[8];
        Ground1 = Resources.Load<TileBase>("Blocks/ground0");
        Ground2 = Resources.Load<TileBase>("Blocks/ground1");
        Grass1 = Resources.Load<TileBase>("Blocks/grass0");
        Grass2 = Resources.Load<TileBase>("Blocks/grass1");
        Grass3 = Resources.Load<TileBase>("Blocks/grass2");
        Water0 = Resources.Load<TileBase>("Blocks/water0");
        mapInfo = new MapInfo();
        for (int i = 0; i < 8; i++)
        {
            GrassField[i] = Resources.Load<TileBase>("Blocks/gf" + Convert.ToString(i));
        }
        InitItemDictionary();
        mapFront.SetTile(new Vector3Int(15, 15, 0), itemDictionary["cucumber"]);


    }
    private void Start()
    {

        StartCoroutine(InitMap());
        Debug.Log("MapController Started!");


    }
    private void Update()
    {

    }
    public IEnumerator InitMap()
    {
        yield return InitBackground();
    }
    public void GenerateMap()
    {

    }
    public IEnumerator InitBackground()
    {
        //for(int i=0;i<Width;i++)
        //{
        //    for(int j=0;j<Height;j++)
        //    {
        //        back_positions[i*Width+j] = new Vector3Int(i, j, 0);
        //        mapBackground.SetTile(back_positions[i * Width + j ], Water0);
        //        yield return null;

        //    }
        //}
        Vector3Int vec = new Vector3Int(8, 10, 0);
        yield return StartCoroutine(GenerateGreenContinent(vec, 50, 40));
    }
    public IEnumerator GenerateGrassField(Vector3Int vec, int width, int height)
    {
        int x0 = vec.x;
        int y0 = vec.y;
        Vector3Int pos = new Vector3Int();
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                pos = new Vector3Int(x0 + j, y0 + i, 0);
                if (i == 0 && j == 0)//左下角
                {
                    mapBackground.SetTile(pos, GrassField[6]);
                }
                else if (i == height - 1 && j == 0)//左上角
                {
                    mapBackground.SetTile(pos, GrassField[0]);
                }
                else if (i == height - 1 && j == width - 1)//右上角
                {
                    mapBackground.SetTile(pos, GrassField[2]);
                }
                else if (i == 0 && j == width - 1)//右下角
                {
                    mapBackground.SetTile(pos, GrassField[4]);
                }
                else if ((i > 0 && i < height - 1) && j == 0)//左边
                {
                    mapBackground.SetTile(pos, GrassField[7]);
                }
                else if ((i > 0 && i < height - 1) && j == width - 1)//右边
                {
                    mapBackground.SetTile(pos, GrassField[3]);
                }
                else if (i == height - 1 && (j > 0 && j < width - 1))//上边
                {
                    mapBackground.SetTile(pos, GrassField[1]);
                }
                else if (i == 0 && (j > 0 && j < width - 1))//下边
                {
                    mapBackground.SetTile(pos, GrassField[5]);
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        mapBackground.SetTile(pos, Ground1);
                    }
                    else
                    {
                        mapBackground.SetTile(pos, Ground2);
                    }
                }
            }
            yield return null;
        }
        Vector3Int vec1 = new Vector3Int(x0 + width - 1, y0 + height - 1, 0);
        Vector3Int[] vecs = new Vector3Int[2];
        vecs[0] = vec;
        vecs[1] = vec1;
        GrassFieldPos.Add(vecs);
    }
    public IEnumerator GenerateGreenContinent(Vector3Int vec, int width, int height)//生成翠绿岛
    {
        int fieldNum = 0;
        fieldNum = (int)UnityEngine.Random.Range(4.0f, 10.0f);
        Vector3Int[] vecs = new Vector3Int[2];//将GreenContinent坐标数据存入字典
        vecs[0] = vec;
        vecs[1] = new Vector3Int(vec.x + width - 1, vec.y + height - 1, 0);
        mapInfo.ContinentVecs.Add("GreenContinent", vecs);
        Vector3Int firstvec = new Vector3Int(0, 0, 0);
        Vector3Int fieldvec = new Vector3Int(0, 0, 0);
        int fwidth = 0;
        int fheight = 0;
        int x = 0;
        int y = 0;
        for (int k = 0; k < height; k++)//GreenContinent中背景先统一填成草地
        {
            for (int s = 0; s < width; s++)
            {
                firstvec.x = vec.x + s;
                firstvec.y = vec.y + k;
                mapBackground.SetTile(firstvec, Grass2);
            }
        }
        yield return null;
        for (int i = 0; i < fieldNum; i++)//生成若干个GrassField
        {
            UnityEngine.Random.InitState(i + night);
            fwidth = width / fieldNum + (int)UnityEngine.Random.Range(2.0f, 5.0f);
            fheight = fwidth * 2 / 3;
            x = UnityEngine.Random.Range(vec.x, vec.x + width);
            y = UnityEngine.Random.Range(vec.y, vec.y + height);
            fieldvec.x = x;
            fieldvec.y = y;
            yield return StartCoroutine(OnGenerateGrassField(fieldvec, fwidth, fheight));
        }
    }
    public IEnumerator OnGenerateGrassField(Vector3Int vec, int width, int height)//验证GrassField坐标是否在地图内，是否重复等。
    {
        Vector3Int vec1 = new Vector3Int(0, 0, 0);//vec1 GrassField的右上角
        while (((vec.x + width - 1) >= mapInfo.ContinentVecs["GreenContinent"][1].x || (vec.y + height - 1) >= mapInfo.ContinentVecs["GreenContinent"][1].y))
        {

            if ((vec.x + width) >= mapInfo.ContinentVecs["GreenContinent"][1].x)
            {
                width = width - 3;
                height = width * 2 / 3;
            }
            if ((vec.y + height) >= mapInfo.ContinentVecs["GreenContinent"][1].y)
            {
                height = height - 2;
                width = height * 3 / 2;
            }
            yield return null;
        }
        vec1.x = vec.x + width - 1;
        vec1.y = vec.y + height - 1;
        if (!IsFieldDublicating(vec, vec1, GrassFieldPos) && width >= 3)
        {
            yield return StartCoroutine(GenerateGrassField(vec, width, height));
        }


    }

    private bool IsFieldDublicating(Vector3Int ld1, Vector3Int ru1, List<Vector3Int[]> poslist)//判断这个Field是否跟已生成的Field重合
    {
        Vector3Int ld2 = new Vector3Int();
        Vector3Int ru2 = new Vector3Int();
        bool iDub = false;
        for (int i = 0; i < poslist.Count; i++)
        {
            ld2 = poslist[i][0];
            ru2 = poslist[i][1];
            if (ru1.x >= ld2.x && ru2.x >= ld1.x && ru1.y >= ld2.y && ru2.y >= ld1.y)
                iDub = true;
            else
            {
            }
        }
        if (iDub)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void InitItemDictionary()
    {
        var tile = Resources.Load<TileBase>("Blocks/cucumber");
        itemDictionary.Add("cucumber", tile);
    }
            
   
}
