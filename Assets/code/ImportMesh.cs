using UnityEngine;
using System.Linq;
using System.IO;

//メッシュを読み込んで頂点位置をvector3[]でテキストファイルとして出力
//モデリングした頂点位置をコードとして埋め込んでパーティクル位置として利用する為の下準備に利用する
public class ImportMesh : MonoBehaviour
{

    [SerializeField]
    GameObject meshTarget;

    void Start()
    {
        string first = "Vector3[] pos = new Vector3[] { ";
        string left = "new Vector3(";
        string right = "), ";
        string end = "};";

        Vector3[] vertices = meshTarget.GetComponent<MeshFilter>().sharedMesh.vertices.Distinct().ToArray();    //重複する頂点はひとつにする
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.Append(first);
        for (int i = 0; i < vertices.Length; i++)
        {
            sb.Append(left + vertices[i].x.ToString("F") + "f, " + vertices[i].y.ToString("F") + "f, " + vertices[i].z.ToString("F") + "f" + right);
        }
        sb.Append(end);

        print(sb);

        string[] st =  { sb.ToString() };
        string folder = Application.dataPath;                   //これだけでunityの実行ファイルがあるフォルダがわかる
        SaveText(folder, @"\vec.txt", st);
    }

    //資料：StreamWriter クラス (System.IO)
    //http://msdn.microsoft.com/ja-jp/library/system.io.streamwriter(v=vs.110).aspx

    //テキストファイルとしてセーブ
    public void SaveText(string fileFolder, string filename, string[] dataStr)
    {
        using (StreamWriter w = new StreamWriter(fileFolder + filename, false, System.Text.Encoding.GetEncoding("shift_jis")))
        {
            foreach (var item in dataStr)
            {
                w.WriteLine(item);
            }
        }
    }
}
