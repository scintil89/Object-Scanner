using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

public struct point
{
    public int rotate;
    public int distance;
    public int yValue;

    public point(int a, int b, int c) { rotate = a; distance = b; yValue = c; }
};

public class ArduinoScript : MonoBehaviour
{
    Mesh mesh;
    public GameObject obj;
    public Material mat;

    //포인트 자료구조
    public List<List<Vector3>> verts = new List<List<Vector3>>();
    public List<point> vert1 = new List<point>();
    public List<point> vert2 = new List<point>();

    //매쉬 버텍스버퍼, 인덱스버퍼
    public List<Vector3> _VertexBuffer = new List<Vector3>();
    public List<int> _IndexBuffer = new List<int>();

    [Range(0, 360)]
    public int rotateAngle = 45;

    int basicDist = 600;
    int cnt = 0;
    int r = 0;
    int maxH = 0;

    //360 / 5개
    //public List<int> verts = new List<int>();
    /*
    int[] vert = new int[360/5] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                  -1, -1
                                 };*/

    SerialPort sp = new SerialPort("COM3", 9600); //데스크탑 com3, 맥북 com5

    Vector3 point2vector(point p)
    {
        Vector3 vec3 = new Vector3(p.distance * Mathf.Cos(p.rotate * Mathf.Deg2Rad) / 10, p.yValue * 2, p.distance * Mathf.Sin(p.rotate * Mathf.Deg2Rad) / 10);

        return vec3;
    }

    public void arduinoStart()
    {
        sp.WriteLine("1");
    }

    // Use this for initialization
    void Start()
    {
        //serial port init
        sp.Open();
        sp.ReadTimeout = 100;

        //mesh init
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        gameObject.GetComponent<MeshRenderer>().material = mat;
    }

    // Update is called once per frame
    void Update()
    {
        if (sp.IsOpen)
        {
            try
            {
                string _read = sp.ReadLine();

                Debug.Log(_read);

                if (_read.Contains("~"))
                {
                    //
                    string[] str = _read.Split('~');

                    maxH = int.Parse(str[0]);
                }
                else if (_read.Contains("$$"))
                {
                    //Start
                }
                else if (_read.Contains("@@"))
                {
                    //한번 끝남.
                    //배열 다 밀어넣음
                    Debug.LogError("push push");

                    point tmp_point = new point();

                    List<Vector3> tmpList = new List<Vector3>();

                    foreach (var v in vert1)
                    {
                        r = 0;

                        //if(v.rotate == r)
                        //{
                        //값이 정확하므로 그냥 넣는다
                        tmpList.Add(point2vector(v));
                        tmp_point = v;
                        //}
                        // else
                        // {
                        //값이 없는 경우 회전각 , 이전 dist를 이용하여 새로운 점 생성

                        //       point np = new point(r, tmp_point.distance, tmp_point.yValue);

                        //       tmpList.Add( point2vector(np) );
                        //        tmpList.Add( point2vector(v) );
                        //        tmp_point = v;
                        // }

                        //r += rotateAngle;
                    }

                    //r = 180;

                    foreach (var v in vert2)
                    {
                        //if (v.rotate == r)
                        //{
                        tmpList.Add(point2vector(v));
                        r += rotateAngle;
                        //}
                        // else
                        //{
                        //    //이전 값이라도 넣는다
                        //     tmpList.Add(point2vector(tmp_point));
                        // }
                    }

                    ////end

                    verts.Add(tmpList);

                    cnt++;
                    vert1.Clear();
                    vert2.Clear();

                }

                else if (_read.Contains("!!"))
                {
                    //스캐닝 끝
                    sp.Close();
                }

                else if (_read.Contains("&"))
                {
                    string[] result = _read.Split('&');

                    point newPoint = new point();

                    newPoint.rotate = int.Parse(result[0]);
                    newPoint.distance = basicDist - int.Parse(result[1]);
                    newPoint.yValue = int.Parse(result[2]);

                    GameObject newObj = Instantiate<GameObject>(obj);

                    newObj.transform.position = point2vector(newPoint);

                    //180도 보다 작으면 1 크면 2
                    if (newPoint.rotate < 180 && newPoint.rotate >= 0)
                    {
                        //vert1.Add(newObj.transform.position);
                        vert1.Add(newPoint);
                    }
                    else if (newPoint.rotate >= 180 && newPoint.rotate < 360)
                    {
                        //vert2.Add(newObj.transform.position);
                        vert2.Add(newPoint);
                    }

                }
            }

            catch (System.Exception)
            {
                //Debug.Log("aaaa");
            }
        }
    }




    void OnGUI()
    {
        if (GUI.Button(new Rect(100, 250, 100, 70), "tst"))
        {
            Mesh mesh = GetComponent<MeshFilter>().mesh;

            mesh.Clear();

            //List<Vector3> newV = new List<Vector3>();

            _VertexBuffer.Add(new Vector3(0, 0, 0));
            _VertexBuffer.Add(new Vector3(1, 0, 0));
            _VertexBuffer.Add(new Vector3(2, 0, 0));
            _VertexBuffer.Add(new Vector3(3, 0, 0));
            _VertexBuffer.Add(new Vector3(4, 0, 0));
            _VertexBuffer.Add(new Vector3(5, 0, 0));
            _VertexBuffer.Add(new Vector3(6, 0, 0));
            _VertexBuffer.Add(new Vector3(7, 0, 0));
            _VertexBuffer.Add(new Vector3(8, 0, 0));
            _VertexBuffer.Add(new Vector3(9, 0, 0));
            _VertexBuffer.Add(new Vector3(10, 0, 0));
            _VertexBuffer.Add(new Vector3(11, 0, 0));
            _VertexBuffer.Add(new Vector3(12, 0, 0));
            _VertexBuffer.Add(new Vector3(13, 0, 0));
            _VertexBuffer.Add(new Vector3(14, 0, 0));
            _VertexBuffer.Add(new Vector3(15, 0, 0));
            _VertexBuffer.Add(new Vector3(16, 0, 0));
            _VertexBuffer.Add(new Vector3(17, 0, 0));
            _VertexBuffer.Add(new Vector3(18, 0, 0));
            _VertexBuffer.Add(new Vector3(10, 0, 2));

            _VertexBuffer.Add(new Vector3(0, 1, 0));
            _VertexBuffer.Add(new Vector3(1, 1, 0));
            _VertexBuffer.Add(new Vector3(2, 1, 0));
            _VertexBuffer.Add(new Vector3(3, 1, 0));
            _VertexBuffer.Add(new Vector3(4, 1, 0));
            _VertexBuffer.Add(new Vector3(5, 1, 0));
            _VertexBuffer.Add(new Vector3(6, 1, 0));
            _VertexBuffer.Add(new Vector3(7, 1, 0));
            _VertexBuffer.Add(new Vector3(8, 1, 0));
            _VertexBuffer.Add(new Vector3(9, 1, 0));
            _VertexBuffer.Add(new Vector3(10, 1, 0));
            _VertexBuffer.Add(new Vector3(11, 1, 0));
            _VertexBuffer.Add(new Vector3(12, 1, 0));
            _VertexBuffer.Add(new Vector3(13, 1, 0));
            _VertexBuffer.Add(new Vector3(14, 1, 0));
            _VertexBuffer.Add(new Vector3(15, 1, 0));
            _VertexBuffer.Add(new Vector3(16, 1, 0));
            _VertexBuffer.Add(new Vector3(17, 1, 0));
            _VertexBuffer.Add(new Vector3(18, 1, 0));
            _VertexBuffer.Add(new Vector3(10, 1, 2));

            _VertexBuffer.Add(new Vector3(0, 2, 0));
            _VertexBuffer.Add(new Vector3(1, 2, 0));
            _VertexBuffer.Add(new Vector3(2, 2, 0));
            _VertexBuffer.Add(new Vector3(3, 2, 0));
            _VertexBuffer.Add(new Vector3(4, 2, 0));
            _VertexBuffer.Add(new Vector3(5, 2, 0));
            _VertexBuffer.Add(new Vector3(6, 2, 0));
            _VertexBuffer.Add(new Vector3(7, 2, 0));
            _VertexBuffer.Add(new Vector3(8, 2, 0));
            _VertexBuffer.Add(new Vector3(9, 2, 0));
            _VertexBuffer.Add(new Vector3(10, 2, 0));
            _VertexBuffer.Add(new Vector3(11, 2, 0));
            _VertexBuffer.Add(new Vector3(12, 2, 0));
            _VertexBuffer.Add(new Vector3(13, 2, 0));
            _VertexBuffer.Add(new Vector3(14, 2, 0));
            _VertexBuffer.Add(new Vector3(15, 2, 0));
            _VertexBuffer.Add(new Vector3(16, 2, 0));
            _VertexBuffer.Add(new Vector3(17, 2, 0));
            _VertexBuffer.Add(new Vector3(18, 2, 0));
            _VertexBuffer.Add(new Vector3(10, 2, 2));

            _VertexBuffer.Add(new Vector3(0, 3, 0));
            _VertexBuffer.Add(new Vector3(1, 3, 0));
            _VertexBuffer.Add(new Vector3(2, 3, 0));
            _VertexBuffer.Add(new Vector3(3, 3, 0));
            _VertexBuffer.Add(new Vector3(4, 3, 0));
            _VertexBuffer.Add(new Vector3(5, 3, 0));
            _VertexBuffer.Add(new Vector3(6, 3, 0));
            _VertexBuffer.Add(new Vector3(7, 3, 0));
            _VertexBuffer.Add(new Vector3(8, 3, 0));
            _VertexBuffer.Add(new Vector3(9, 3, 0));
            _VertexBuffer.Add(new Vector3(10, 3, 0));
            _VertexBuffer.Add(new Vector3(11, 3, 0));
            _VertexBuffer.Add(new Vector3(12, 3, 0));
            _VertexBuffer.Add(new Vector3(13, 3, 0));
            _VertexBuffer.Add(new Vector3(14, 3, 0));
            _VertexBuffer.Add(new Vector3(15, 3, 0));
            _VertexBuffer.Add(new Vector3(16, 3, 0));
            _VertexBuffer.Add(new Vector3(17, 3, 0));
            _VertexBuffer.Add(new Vector3(18, 3, 0));
            _VertexBuffer.Add(new Vector3(10, 3, 2));

            int MaxCol_ = 20;
            int height_ = 3;

            for (int h = 0; h < height_; h++)
            {
                for (int i = 0; i < MaxCol_ - 1; i++)
                {
                    int idx = MaxCol_ * h;
                    _IndexBuffer.Add(idx + i);
                    _IndexBuffer.Add(idx + i + MaxCol_);
                    _IndexBuffer.Add(idx + i + 1);

                    _IndexBuffer.Add(idx + i + 1);
                    _IndexBuffer.Add(idx + i + MaxCol_);
                    _IndexBuffer.Add(idx + i + 1 + MaxCol_);

                    if (i == MaxCol_ - 2)
                    {
                        _IndexBuffer.Add(idx + i + 1);
                        _IndexBuffer.Add(idx + i + 1 + MaxCol_);
                        _IndexBuffer.Add(idx);

                        _IndexBuffer.Add(idx);
                        _IndexBuffer.Add(idx + i + 1 + MaxCol_);
                        _IndexBuffer.Add(idx + MaxCol_);
                        //_IndexBuffer.Add(MaxCol_ * (h + 1));
                    }
                }
            }

            //메쉬를 청소해줍니다.
            mesh.Clear();
            //버텍스 데이터를 배열로 밀어 넣습니다.
            mesh.vertices = _VertexBuffer.ToArray();
            //인접한 버텍스 데이터를 배열로 밀어 넣습니다.
            mesh.triangles = _IndexBuffer.ToArray();
            //메쉬를 생성합니다.
            ;
            mesh.RecalculateNormals();
        }

        if (GUI.Button(new Rect(200, 250, 100, 70), "make"))
        {
            Debug.LogError("butt");

            mesh = GetComponent<MeshFilter>().mesh;

            mesh.Clear();

            var size = verts.Count;

            Debug.Log("verts count : " + size);

            //버텍스 데이터를 배열로 밀어 넣습니다.
            foreach (var i in verts)
            {
                //데이터 수 체크
                foreach (var j in i)
                {
                    _VertexBuffer.Add(new Vector3(j.x, j.y, j.z));
                }
            }
            /*
            for(int i = 0; i < verts.Count; i++)
            {
                for (int j = 0; j < verts[i].Count; j++)
                {
                    _VertexBuffer.Add( verts[i][j] );
                }
            }
            */
            //////////////////////
            //인덱스 버퍼를 생성합니다.
            int MaxCol = 360 / rotateAngle;
            int height = size;

            Debug.Log("index col : " + MaxCol + " height" + size);

            for (int h = 0; h < height - 1; h++)
            {
                int idx = MaxCol * h;

                //Bottom 
                if (h == 0)
                {
                    for (int i = 0; i < MaxCol - 1; i++)
                    {
                        _IndexBuffer.Add(0);
                        _IndexBuffer.Add(i);
                        _IndexBuffer.Add(i+1);
                    }
                }

                //Side
                for (int i = 0; i < MaxCol - 1; i++)
                {
                    _IndexBuffer.Add(idx + i);
                    _IndexBuffer.Add(idx + i + 1);
                    _IndexBuffer.Add(idx + i + MaxCol);

                    _IndexBuffer.Add(idx + i + MaxCol);
                    _IndexBuffer.Add(idx + i + 1);
                    _IndexBuffer.Add(idx + i + 1 + MaxCol);

                    if (i == MaxCol - 2)
                    {
                        _IndexBuffer.Add(idx + i + 1);
                        _IndexBuffer.Add(idx + 0);
                        _IndexBuffer.Add(idx + i + MaxCol + 1);

                        _IndexBuffer.Add(idx + i + MaxCol + 1);
                        _IndexBuffer.Add(idx + 0);
                        _IndexBuffer.Add(idx + 0 + MaxCol);
                    }

                }

                //Top
                if (h == height - 2)
                {
                    idx = MaxCol * (h + 1);
                    for (int i = 0; i < MaxCol - 1; i++)
                    {
                        _IndexBuffer.Add(idx + 0);
                        _IndexBuffer.Add(idx + i);
                        _IndexBuffer.Add(idx + i + 1);
                    }
                }
                
            }
            
            //메쉬를 청소해줍니다.
            mesh.Clear();
            //버텍스 데이터를 배열로 밀어 넣습니다.
            mesh.vertices = _VertexBuffer.ToArray();
            //인접한 버텍스 데이터를 배열로 밀어 넣습니다.
            mesh.triangles = _IndexBuffer.ToArray();
            //메쉬를 생성합니다.
            mesh.Optimize();
            mesh.RecalculateNormals();
        }

        if (GUI.Button(new Rect(300, 250, 100, 70), "clr"))
        {
            mesh = GetComponent<MeshFilter>().mesh;
            mesh.Clear();

            _VertexBuffer.Clear();
            _IndexBuffer.Clear();
        }
    }
}