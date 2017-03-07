using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleObj : MonoBehaviour
{
    public float particleSize = 0.05f;

    ParticleSystem pe;
    ParticleSystem.Particle[] point;

    //パーティクル用インポート文字座標「ケ」1×1unit_size
    [HideInInspector]
    public Vector3[] fontPos = new Vector3[] { new Vector3(-0.16f, -0.43f, 0.00f), new Vector3(-0.24f, -0.36f, 0.00f), new Vector3(-0.10f, -0.31f, 0.00f), new Vector3(-0.02f, -0.37f, 0.00f), new Vector3(-0.01f, -0.26f, 0.00f), new Vector3(0.08f, -0.30f, 0.00f), new Vector3(0.05f, -0.19f, 0.00f), new Vector3(0.09f, -0.12f, 0.00f), new Vector3(0.14f, -0.22f, 0.00f), new Vector3(0.10f, -0.04f, 0.00f), new Vector3(0.10f, 0.03f, 0.00f), new Vector3(0.10f, 0.10f, 0.00f), new Vector3(0.10f, 0.17f, 0.00f), new Vector3(0.13f, 0.25f, 0.00f), new Vector3(0.21f, 0.25f, 0.00f), new Vector3(0.20f, 0.17f, 0.00f), new Vector3(0.19f, -0.13f, 0.00f), new Vector3(0.20f, -0.04f, 0.00f), new Vector3(0.20f, 0.03f, 0.00f), new Vector3(0.20f, 0.10f, 0.00f), new Vector3(0.29f, 0.17f, 0.00f), new Vector3(0.29f, 0.25f, 0.00f), new Vector3(0.37f, 0.25f, 0.00f), new Vector3(0.37f, 0.17f, 0.00f), new Vector3(0.45f, 0.25f, 0.00f), new Vector3(0.45f, 0.17f, 0.00f), new Vector3(0.05f, 0.25f, 0.00f), new Vector3(0.03f, 0.17f, 0.00f), new Vector3(-0.02f, 0.25f, 0.00f), new Vector3(-0.05f, 0.17f, 0.00f), new Vector3(-0.10f, 0.25f, 0.00f), new Vector3(-0.13f, 0.17f, 0.00f), new Vector3(-0.18f, 0.25f, 0.00f), new Vector3(-0.20f, 0.17f, 0.00f), new Vector3(-0.17f, 0.43f, 0.00f), new Vector3(-0.22f, 0.11f, 0.00f), new Vector3(-0.28f, 0.43f, 0.00f), new Vector3(-0.28f, 0.37f, 0.00f), new Vector3(-0.28f, 0.30f, 0.00f), new Vector3(-0.29f, 0.00f, 0.00f), new Vector3(-0.29f, 0.22f, 0.00f), new Vector3(-0.32f, 0.14f, 0.00f), new Vector3(-0.37f, -0.08f, 0.00f), new Vector3(-0.37f, 0.05f, 0.00f), new Vector3(-0.45f, -0.02f, 0.00f), new Vector3(-0.17f, 0.30f, 0.00f), new Vector3(-0.17f, 0.36f, 0.00f), new Vector3(0.50f, 0.52f, 0.00f), new Vector3(0.50f, -0.49f, 0.00f), new Vector3(-0.50f, -0.49f, 0.00f), new Vector3(-0.50f, 0.52f, 0.00f), };

    void Awake()
    {
        pe = gameObject.AddComponent<ParticleSystem>();
        ParticleSystem.MainModule peM = pe.main;
        peM.startSpeed = 0;
        peM.startLifetime = float.MaxValue;
        var em = pe.emission;
        em.enabled = false;     //パーティクルの追加噴出を止める

        var renderer = pe.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("GUI/Text Shader"));

        pe.Emit(fontPos.Length);
        point = new ParticleSystem.Particle[fontPos.Length + 1];
        pe.GetParticles(point);

        for (int n = 0; n < fontPos.Length; n++)
        {
            point[n].position = fontPos[n];
            point[n].startColor = Color.white;
            point[n].startSize = particleSize;
        }
        pe.SetParticles(point, fontPos.Length);
    }

    //外部から呼び出し行列計算で更新する
    public void UpdatePos(Matrix4x4 detA)
    {
        for (int i = 0; i < fontPos.Length; i++)
        {
            point[i].position = detA * fontPos[i];
        }
        pe.SetParticles(point, fontPos.Length);
    }
}
