using UnityEngine;
using System.Collections;

public static class MeshGenerator {

    public static MeshData GenerateterrainMesh(float[,] heightMap,float meshHeight,AnimationCurve heightCurve,int LOD)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;
        MeshData meshData = new MeshData(width,height);
        int vertexIndex = 0;
        int detailscale = (LOD==0)?1:LOD * 2;
        int vertexPerLine = (width-1)/detailscale + 1;
        for(int y =0; y < height; y+= detailscale)
        {
            for (int x = 0;x<width; x+=detailscale)
            {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y])* meshHeight, topLeftZ - y);
                meshData.uv[vertexIndex] = new Vector2(x/(float)width,y/(float)height);
                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangles(vertexIndex, vertexIndex + vertexPerLine + 1, vertexIndex + vertexPerLine);
                    meshData.AddTriangles(vertexIndex+ vertexPerLine + 1, vertexIndex , vertexIndex + 1);                    
                }
                vertexIndex++;
            }
        }
        return meshData;
    }

}

public class MeshData{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uv;
    public int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshHeight*meshWidth];
        triangles = new int[(meshWidth-1)*(meshHeight-1)*6];
        uv = new Vector2[meshHeight*meshWidth];
    }

    public void AddTriangles(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex+1] = b;
        triangles[triangleIndex+2] = c;

        triangleIndex += 3;
    }
    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }
}
