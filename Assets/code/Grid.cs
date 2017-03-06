using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public Rect gridSize = new Rect(-0.5f, -0.5f, 1, 1);
    public Color gridColor = new Color(0f, 0f, 0f);
    public int gridDivid = 20;                        //グリッドの分割数。偶数に設定する事

    public Mesh mesh;

    void Awake()
    {
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        GetComponent<MeshRenderer>().material = new Material(Shader.Find("GUI/Text Shader"));
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        RespownPolyGrid(gridSize, gridColor, gridDivid);
    }

    //グリッドとして扱うポリゴンメッシュを生成
    void RespownPolyGrid(Rect rect, Color col, int divide)
    {
        Vector2[] uv;
        Color[] colors;
        int[] lines;

        List<Vector3> temp = new List<Vector3>();
        float dx = (rect.xMax - rect.xMin) / (divide);
        for (int i = 0; i <= divide; i++)
        {
            temp.Add(new Vector3(rect.xMin + dx * i, -rect.yMin, 0.0f));
            temp.Add(new Vector3(rect.xMin + dx * i, -rect.yMax, 0.0f));
        }
        float dy = (rect.yMax - rect.yMin) / (divide);
        for (int i = 0; i <= divide; i++)
        {
            temp.Add(new Vector3(rect.xMin, -rect.yMin - dy * i, 0.0f));
            temp.Add(new Vector3(rect.xMax, -rect.yMin - dy * i, 0.0f));
        }

        Vector3[] pos = temp.ToArray();
        uv = Enumerable.Range(1, pos.Length).Select(n => Vector2.zero).ToArray();
        colors = Enumerable.Range(1, pos.Length).Select(n => col).ToArray();

        for (int i = 0; i < 2; i++)
        {
            colors[(colors.Length * 1 / 4) - i] = Color.red;
            colors[(colors.Length * 3 / 4) - i] = Color.green;
        }

        lines = InitLine(pos, MeshTopology.Lines);

        mesh.vertices = pos;
        mesh.uv = uv;
        mesh.colors = colors;
        mesh.SetIndices(lines, MeshTopology.Lines, 0);
    }

    //メッシュの一筆書き型ラインレンダリング用
    int[] InitLine(Vector3[] pos, MeshTopology mt)
    {
        switch (mt)
        {
            case MeshTopology.LineStrip:
                List<int> temp = new List<int>();
                for (int i = 0; i < pos.Length; i++)
                {
                    temp.Add(i);
                    temp.Add(i + 1);
                }
                temp[temp.Count - 1] = 0;
                return temp.ToArray();
            case MeshTopology.Lines:
                return Enumerable.Range(0, pos.Length).Select(n => n).ToArray();
            default:
                return null;
        }
    }
}
