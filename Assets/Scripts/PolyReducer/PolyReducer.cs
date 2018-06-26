using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
namespace PolyReduction
{
    public class PolyReducer : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            //init the mesh? [TODO]
            if (meshToGenerate == null)
                meshToGenerate = GetComponent<MeshFilter>().mesh;
            meshToGenerate = Object.Instantiate<Mesh>(meshToGenerate);
            DeleteDup();
            Generate();
        }

        //Update is called once per frame
        void Update()
        {
            //reductionData.Clear();
            //collapse vertex with the least 'collapsePerFrame' cost 
            for (int zx = 0; zx < collapsePerFrame; zx++)
            {
                PRVertex mn = MinimunCostEdge();
                Collapse(mn, mn.collapse);
                vertexNum--;
            }

            //what dose ReductionData do? [TODO]
            for (; deleteIndex < reductionData.Count; deleteIndex++)
            {
                ApplyData(reductionData[deleteIndex]);
            }
            //update the mesh
            meshToGenerate.vertices = vertices;
            meshToGenerate.triangles = triangles;
            GetComponent<MeshFilter>().mesh = meshToGenerate;

        }

        public Mesh meshToGenerate;
        private Vector3[] vertices;
        private int[] triangles;
        private Vector3[] normals;
        private PRVertex[] prVertices;
        private PRTriangle[] prTriangle;
        private int vertexNum = 0;
        private int deleteIndex = 0;
        private List<ReductionData> reductionData = new List<ReductionData>();

        public int collapsePerFrame = 60;

        
        void DeleteDup()
        {
            vertices = meshToGenerate.vertices;
            triangles = meshToGenerate.triangles;
            vertexNum = vertices.Length;
            //vector3 - 
            Hashtable pos_point_map = new Hashtable();
            Vector3[] point_pos_map = new Vector3[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                if(!pos_point_map.Contains(vertices[i]))
                    pos_point_map.Add(vertices[i], i);
                point_pos_map[i] = vertices[i];
                
            }

            for(int i = 0; i < triangles.Length; i++)
            {
                triangles[i] = (int)pos_point_map[point_pos_map[triangles[i]]];
            }
            //Vector3[] vertices_distinct;
            //for (int i = point_pos_map.Length - 1; i >= 0; i--)
            //{
            //    if((int)pos_point_map[point_pos_map[i]] != i)
            //        vertices.
            //}
            meshToGenerate.triangles = triangles;
            GetComponent<MeshFilter>().mesh = meshToGenerate;
        }

        //process the mesh
        void Generate()
        { 
            vertices = meshToGenerate.vertices;
            triangles = meshToGenerate.triangles;
            normals = meshToGenerate.normals;
            vertexNum = vertices.Length;
            Debug.Log(triangles.Length);
            prVertices = new PRVertex[vertices.Length];
            prTriangle = new PRTriangle[triangles.Length / 3];
            int i;
            int j;
            Hashtable pointMap = new Hashtable();
            //init the vertexes
            for (i = 0; i < vertices.Length; i++)
            {
                //if (pointMap.Contains(vertices[i]))
                //    prVertices[i] = prVertices[(int)pointMap[vertices[i]]];
                //else
                //{
                //    prVertices[i] = new PRVertex(i, vertices[i]);
                //    pointMap.Add(vertices[i], i);
                //}
                prVertices[i] = new PRVertex(i, vertices[i]);
            }
            //init the faces
            for (i = 0, j = 0; i < triangles.Length; i += 3, j += 1)
            {
                prTriangle[j] = new PRTriangle(i, prVertices[triangles[i]], prVertices[triangles[i + 1]], prVertices[triangles[i + 2]]);
                //Debug.Log(triangles[i] + " " + triangles[i + 1] + " " + triangles[i + 2]);
            }
            //update the neighbour faces of 3 vertex of a triangle 
            for (i = 0; i < prTriangle.Length; i++)
            {
                prTriangle[i].vertex[0].face.Add(prTriangle[i]);
                prTriangle[i].vertex[1].face.Add(prTriangle[i]);
                prTriangle[i].vertex[2].face.Add(prTriangle[i]);
                //update the neighbour of a point with the other two points in the triangle
                for (j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        if (j == k)
                            continue;
                        if (!prTriangle[i].vertex[j].neighbor.Contains(prTriangle[i].vertex[k]))
                        {
                            prTriangle[i].vertex[j].neighbor.Add(prTriangle[i].vertex[k]);
                        }
                    }
                }
            }
            //calculate all the costs of the vertexes
            for (i = 0; i < prVertices.Length; i++)
            {
                ComputeEdgeCostAtVertex(prVertices[i]);
            }
            //collapse vertex with the least 'collapsePerFrame' cost 
            for (int zx = 0; zx < collapsePerFrame; zx++)
            {
                PRVertex mn = MinimunCostEdge();
                Collapse(mn, mn.collapse);
                vertexNum--;
            }

            //what dose ReductionData do? [TODO]
            for (; deleteIndex < reductionData.Count; deleteIndex++)
            {
                ApplyData(reductionData[deleteIndex]);
            }
            //update the mesh
            meshToGenerate.vertices = vertices;
            meshToGenerate.triangles = triangles;
            GetComponent<MeshFilter>().mesh = meshToGenerate;

        }
        //tranverse all the data to find the vertex with the least cost [TODO] [can be optimized]
        public PRVertex MinimunCostEdge()
        {
            PRVertex t = prVertices[0];
            for (int i = 0; i < prVertices.Length; i++)
            {
                if (prVertices[i].cost < t.cost)
                    t = prVertices[i];
            }
            return t;
        }
        //compute the cost of u collapse to v ( has direction)
        //the formation can be seen on wehsite
        public float ComputeEdgeCollapseCost(PRVertex u, PRVertex v)
        {
            //float edgeLength = MathExtra.GetV3L(u.pos - v.pos);
            float edgeLength = Vector3.Distance(u.pos, v.pos);
            float curvature = 0f;
            List<PRTriangle> sides = new List<PRTriangle>();
            for (int i = 0; i < u.face.Count; i++)
            {
                if (u.face[i].HasVertex(v))
                    sides.Add(u.face[i]);
            }
            for (int i = 0; i < u.face.Count; i++)
            {
                float mincurv = 1f;
                for (int j = 0; j < sides.Count; j++)
                {
                    float dotprod = Vector3.Dot(u.face[i].normal, sides[j].normal);
                    mincurv = Mathf.Min(mincurv, (1f - dotprod) * 0.5f);
                }
                curvature = Mathf.Max(curvature, mincurv);
            }
            return edgeLength * curvature;
        }
        //calculate the cost of a single vertex
        //the cost has direction 
        public void ComputeEdgeCostAtVertex(PRVertex v)
        {
            if (v.neighbor.Count == 0)
            {
                v.collapse = null;
                v.cost = 1000000f;
                return;
            }
            v.cost = 1000000f;
            v.collapse = null;
            //tranverse all the neighbours of a vertex
            //use the min valur of the costs of the edges containing that vertex as the cost of the vertex
            for (int i = 0; i < v.neighbor.Count; i++)
            {
                float c;
                c = ComputeEdgeCollapseCost(v, v.neighbor[i]);
                if (c < v.cost)
                {
                    v.collapse = v.neighbor[i];
                    v.cost = c;
                }

            }
        }
        //collapse u to v
        //update the data structure
        //record the process
        public void Collapse(PRVertex u, PRVertex v)
        {
            if (v == null)
            {
                Debug.Log("!!!");//prVertices [u.id] = null;
                return;
            }
            //Debug.Log (u.id.ToString()+"  "+v.id.ToString()+"  "+u.cost.ToString());
            int i;
            List<PRVertex> tmp = new List<PRVertex>();
            for (i = 0; i < u.neighbor.Count; i++)
            {
                tmp.Add(u.neighbor[i]);
            }
            ReductionData rd = new ReductionData();
            rd.vertexU = u.id;
            rd.vertexV = v.id;
            v.neighbor.Remove(u);
            for (i = u.face.Count - 1; i >= 0; i--)
            {
                u.face[i].ReplaceVertex(u, v);
            }
            for (int j = 0; j < u.face.Count; j++)
            {
                rd.triangleID.Add(u.face[j].id);
            }
            reductionData.Add(rd);
            ComputeEdgeCostAtVertex(v);
            //prVertices [u.id] = null;
            for (i = 0; i < tmp.Count; i++)
            {
                ComputeEdgeCostAtVertex(tmp[i]);
            }
            //prVertices [u.id] = null;
            u.cost = 10000000f;
        }

        public void ApplyData(ReductionData rd)
        {

            for (int i = 0; i < rd.triangleID.Count; i++)
            {
                if (triangles[rd.triangleID[i]] == rd.vertexV || triangles[rd.triangleID[i] + 1] == rd.vertexV || triangles[rd.triangleID[i] + 2] == rd.vertexV)
                {
                    triangles[rd.triangleID[i]] = triangles[rd.triangleID[i] + 1] = triangles[rd.triangleID[i] + 2] = 0;
                }
                else
                {
                    if (triangles[rd.triangleID[i]] == rd.vertexU)
                    {
                        triangles[rd.triangleID[i]] = rd.vertexV;
                        continue;
                    }
                    if (triangles[rd.triangleID[i] + 1] == rd.vertexU)
                    {
                        triangles[rd.triangleID[i] + 1] = rd.vertexV;
                        continue;
                    }
                    if (triangles[rd.triangleID[i] + 2] == rd.vertexU)
                    {
                        triangles[rd.triangleID[i] + 2] = rd.vertexV;
                        continue;
                    }
                }
            }
        }
        public void BackData(ReductionData rd)
        {

        }

    }
}
