using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixTest : MonoBehaviour
{
    public float intervalTime = 3f;
    public float speed = 0.5f;
    public float anim;
     Vector4[] pos = new Vector4[4];
    Vector4[] identity = new Vector4[4];

    public Matrix4x4 detA;
    Vector4 x;

    Grid myGrid;
    ParticleObj myParticleObj;
    Vector3[] gridPos,particlePos;
    ParticleSystem.Particle[] point;

    Vector3[] gridTemp;
    Vector3[] fontTemp;

    void Start()
    {
        myGrid = this.transform.GetComponentInChildren<Grid>();
        myParticleObj=this.transform.GetComponentInChildren<ParticleObj>();
        point = myParticleObj.point;

        gridPos = myGrid.mesh.vertices;
        particlePos = myParticleObj.fontPos;

        gridTemp = new Vector3[gridPos.Length];
        fontTemp = new Vector3[particlePos.Length];

        Matrix4x4 temp = Matrix4x4.identity;
        for (int i = 0; i < 4; i++)
        {
            identity[i] = temp.GetRow(i);
        }

        detA = Matrix4x4.zero;
        detA.SetRow(0, new Vector4(1f, -0.3f, 0, 0));
        detA.SetRow(1, new Vector4(-0.7f, 0.6f, 0, 0));
        detA.SetRow(2, new Vector4(0, 0, 1, 0));
        detA.SetRow(3, new Vector4(0, 0, 0, 1));

        x = new Vector4(1, 2, 0, 0);
    }

    void SetPos(Matrix4x4 pos)
    {
        for (int i = 0; i < particlePos.Length; i++)
        {
            fontTemp[i] = pos * particlePos[i];
        }

        for (int i = 0; i < gridPos.Length; i++)
        {
            gridTemp[i] = pos * gridPos[i];
        }

        myGrid.mesh.vertices = gridTemp;
        for (int n = 0; n < fontTemp.Length; n++)
        {
            point[n].position = fontTemp[n];
        }
        myParticleObj.pe.SetParticles(point, particlePos.Length);
    }

    int phase = 0;
    void Update()
    {
        float t = Time.deltaTime;
        switch (phase)
        {
            case 0:
                First(t);
                break;
            case 1:
                Interval(t);
                break;
            case 2:
                Second(t);
                break;
            case 3:
                Interval(t);
                break;
            default:
                phase = 0;
                break;
        }
    }


    float timer = 0;
    Matrix4x4 mat = new Matrix4x4();
    void First(float deltaTime)
    {

        timer += deltaTime * speed;
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = Vector4.Lerp(identity[i], detA.GetRow(i), timer);
            mat.SetRow(i, pos[i]);
        }
        SetPos(mat);

        if (timer >= 1f)
        {
            timer = 0;
            phase++;
        }
    }

    void Interval(float deltaTime)
    {
        timer += deltaTime;
        if (timer >= intervalTime)
        {
            timer = 0;
            phase++;
        }
    }

    void Second(float deltaTime)
    {
        timer += deltaTime * speed;
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = Vector4.Lerp(identity[i], detA.GetRow(i), 1f - timer);
            mat.SetRow(i, pos[i]);
        }
        SetPos(mat);

        if (1f - timer <= 0f)
        {
            timer = 0;
            phase++;
        }
    }
}

