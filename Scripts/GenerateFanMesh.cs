using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 获取顶点的位置坐标
// 连接三角形的顶点绘制出多边形（顶点数组）

public class GenerateFanMesh : MonoBehaviour 
{
    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;
    private ScoutingLaser scoutingLaser;

    [SerializeField]
    private float pieceAngle = 5;

    [SerializeField]
    private float radius = 10;

    public void Start()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();

        MakeFanShape(30, 120);
    }

    public void MakeFanShape(float startAngle, float endAngle)
    {
        // 夹角大于180，但是其实是角度横跨了12点方向的0度，其实是锐角
        if (Mathf.Abs(endAngle - startAngle) > 180f) 
        {
            // 如果出现了这种横跨的情况，就 +360 
            if (startAngle < 180f)
            {
                startAngle += 360f;
            }
            if (endAngle < 180f)
            {
                endAngle += 360f;
            }
        }

        int triangleNum =(int)Mathf.Ceil (Mathf.Abs(endAngle - startAngle) / pieceAngle);

        Vector3[] vertices = new Vector3[triangleNum+1+1];
        // 三角形顶点序号
        int[] triangles = new int[triangleNum * 3];

        // 扇形原点
        vertices[0] = Vector3.zero;

        // 计算每个三角形的顶点
        for (int i = 0; i < triangleNum+1 ; ++i)
        {
            float currentAngle = startAngle + pieceAngle * i;

            // Quaternion.AngleAxis(currentAngle, Vector3.up) 这个算出来是一个 Quaternion ，四元数，表示一个绕 up 旋转 currentAngle 的旋转
            // Quaternion.AngleAxis(currentAngle, Vector3.up) * Vector3.forward ，上面那个结果然后乘  Vector3.forward ，得到的是个 Vector3 ，一个顺时针方向的旋转之后的向量
            // 当然我们也可以乘以 Vector3.back ,就成了逆时针方向的旋转了
            vertices[i + 1] = Quaternion.AngleAxis(currentAngle, Vector3.up) * Vector3.forward * radius;

        }

        // 缠绕三角形 0,1,2   0,2,3  0,3,4 ... 
        for (int i = 0; i < triangleNum; ++i)
        {
            triangles[i * 3 + 0] = 0;
            triangles[i * 3 + 1] = i+1;
            triangles[i * 3 + 2] = i+2;
        }


        // 生成网格
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // 重新计算边界和法线
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        // mesh 修改之后，必须将 enable 从 false 改为 true
        meshCollider.enabled = false;
        meshCollider.enabled = true;
    }


}
