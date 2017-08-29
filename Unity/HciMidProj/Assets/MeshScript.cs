using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshScript : MonoBehaviour
{
    Mesh mesh;

    public List<Vector3> _Vertex = new List<Vector3>();
    public List<int> _Tri = new List<int>();

    //int iSize = 0;
    
    // Use this for initialization
    void Start()
    {
        //mesh = GetComponent<MeshFilter>().mesh;
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if(GUI.Button(new Rect(100, 250, 100, 70), "tst"))
        {
            Mesh mesh = GetComponent<MeshFilter>().mesh;

            mesh.Clear();

            List<Vector3> newV = new List<Vector3>();

            newV.Add(new Vector3(0, 0, 0));
            newV.Add(new Vector3(1, 0, 0));
            newV.Add(new Vector3(2, 0, 0));
            newV.Add(new Vector3(3, 0, 0));
            newV.Add(new Vector3(4, 0, 0));
            newV.Add(new Vector3(5, 0, 0));
            newV.Add(new Vector3(6, 0, 0));
            newV.Add(new Vector3(7, 0, 0));
            newV.Add(new Vector3(8, 0, 0));
            newV.Add(new Vector3(9, 0, 0));
            newV.Add(new Vector3(10, 0, 0));
            newV.Add(new Vector3(11, 0, 0));
            newV.Add(new Vector3(12, 0, 0));
            newV.Add(new Vector3(13, 0, 0));
            newV.Add(new Vector3(14, 0, 0));
            newV.Add(new Vector3(15, 0, 0));
            newV.Add(new Vector3(16, 0, 0));
            newV.Add(new Vector3(17, 0, 0));
            newV.Add(new Vector3(18, 0, 0));
            newV.Add(new Vector3(19, 0, 0));

            newV.Add(new Vector3(0, 1, 0));
            newV.Add(new Vector3(1, 1, 0));
            newV.Add(new Vector3(2, 1, 0));
            newV.Add(new Vector3(3, 1, 0));
            newV.Add(new Vector3(4, 1, 0));
            newV.Add(new Vector3(5, 1, 0));
            newV.Add(new Vector3(6, 1, 0));
            newV.Add(new Vector3(7, 1, 0));
            newV.Add(new Vector3(8, 1, 0));
            newV.Add(new Vector3(9, 1, 0));
            newV.Add(new Vector3(10, 1, 0));
            newV.Add(new Vector3(11, 1, 0));
            newV.Add(new Vector3(12, 1, 0));
            newV.Add(new Vector3(13, 1, 0));
            newV.Add(new Vector3(14, 1, 0));
            newV.Add(new Vector3(15, 1, 0));
            newV.Add(new Vector3(16, 1, 0));
            newV.Add(new Vector3(17, 1, 0));
            newV.Add(new Vector3(18, 1, 0));
            newV.Add(new Vector3(19, 1, 0));

            newV.Add(new Vector3(0, 2, 0));
            newV.Add(new Vector3(1, 2, 0));
            newV.Add(new Vector3(2, 2, 0));
            newV.Add(new Vector3(3, 2, 0));
            newV.Add(new Vector3(4, 2, 0));
            newV.Add(new Vector3(5, 2, 0));
            newV.Add(new Vector3(6, 2, 0));
            newV.Add(new Vector3(7, 2, 0));
            newV.Add(new Vector3(8, 2, 0));
            newV.Add(new Vector3(9, 2, 0));
            newV.Add(new Vector3(10, 2, 0));
            newV.Add(new Vector3(11, 2, 0));
            newV.Add(new Vector3(12, 2, 0));
            newV.Add(new Vector3(13, 2, 0));
            newV.Add(new Vector3(14, 2, 0));
            newV.Add(new Vector3(15, 2, 0));
            newV.Add(new Vector3(16, 2, 0));
            newV.Add(new Vector3(17, 2, 0));
            newV.Add(new Vector3(18, 2, 0));
            newV.Add(new Vector3(19, 2, 0));

            //_Vertex.Add(newV);

            int MaxCol_ = 20;
            int height_ = 3;

            for (int h = 0; h < height_ - 1; h++)
            {
                for (int i = 0; i < MaxCol_ - 1; i++)
                {
                    int idx = MaxCol_ * h;
                    _Tri.Add(idx + i);
                    _Tri.Add(idx + i + MaxCol_);
                    _Tri.Add(idx + i + 1);

                    _Tri.Add(idx + i + 1);
                    _Tri.Add(idx + i + MaxCol_);
                    _Tri.Add(idx + i + 1 + MaxCol_);
                }
            }

            //메쉬를 청소해줍니다.
            mesh.Clear();
            //버텍스 데이터를 배열로 밀어 넣습니다.
            mesh.vertices = newV.ToArray();
            //인접한 버텍스 데이터를 배열로 밀어 넣습니다.
            mesh.triangles = _Tri.ToArray();
            //메쉬를 생성합니다.
            mesh.Optimize();
            mesh.RecalculateNormals();
        }

        if(GUI.Button(new Rect(200, 250, 100, 70), "make"))
        {
            Debug.LogError("butt");

            mesh = GetComponent<MeshFilter>().mesh;
            
            mesh.Clear();
            
            var size = GameObject.Find("GameObject").GetComponent<ArduinoScript>().verts.Count;
            
            //버텍스 데이터를 배열로 밀어 넣습니다.
            foreach (var i in GameObject.Find("GameObject").GetComponent<ArduinoScript>().verts)
            {
                //데이터 수 체크
                foreach (var j in i)
                {
                    _Vertex.Add(j);
                }
            }
            
            //////////////////////
            //인덱스 버퍼를 생성합니다.
            int MaxCol = 360 / 5;
            int height = size;
            
            for (int h = 0; h < height - 1; h++)
            {
                for (int i = 0; i < MaxCol - 1; i++)
                {
                    int idx = MaxCol * h;
                    _Tri.Add(idx + i);
                    _Tri.Add(idx + i + MaxCol);
                    _Tri.Add(idx + i + 1);
            
                    _Tri.Add(idx + i + 1);
                    _Tri.Add(idx + i + MaxCol);
                    _Tri.Add(idx + i + 1 + MaxCol);
                }
            }
            
            //mesh.triangles = _Tri.ToArray();

            //메쉬를 청소해줍니다.
            mesh.Clear();
            //버텍스 데이터를 배열로 밀어 넣습니다.
            mesh.vertices = _Vertex.ToArray();
            //인접한 버텍스 데이터를 배열로 밀어 넣습니다.
            mesh.triangles = _Tri.ToArray();
            //메쉬를 생성합니다.
            mesh.Optimize();
            mesh.RecalculateNormals();

        }
        if (GUI.Button(new Rect(300, 250, 100, 70), "clr"))
        {
            mesh = GetComponent<MeshFilter>().mesh;
            mesh.Clear();
        }
    }
}
