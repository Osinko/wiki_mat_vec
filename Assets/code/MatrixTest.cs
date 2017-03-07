using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixTest : MonoBehaviour
{
    public float intervalTime = 3f;
    public float speed = 0.5f;

    public Matrix4x4 detA;

    GridObj myGridObj;
    ParticleObj myParticleObj;

    void Start()
    {
        myGridObj = this.transform.GetComponentInChildren<GridObj>();
        myParticleObj = this.transform.GetComponentInChildren<ParticleObj>();

        detA.SetRow(0, new Vector4(1f, -0.3f, 0, 0));
        detA.SetRow(1, new Vector4(-0.7f, 0.6f, 0, 0));
        detA.SetRow(2, new Vector4(0, 0, 1, 0));
        detA.SetRow(3, new Vector4(0, 0, 0, 1));
    }

    void SetPos(Matrix4x4 pos)
    {
        myGridObj.UpdatePos(pos);
        myParticleObj.UpdatePos(pos);
    }

    //遷移制御
    int phase = 0;
    void Update()
    {
        switch (phase)
        {
            case 0:
                First(Time.deltaTime);
                break;
            case 1:
                Interval(Time.deltaTime);
                break;
            case 2:
                Second(Time.deltaTime);
                break;
            case 3:
                Interval(Time.deltaTime);
                break;
            default:
                phase = 0;
                break;
        }
    }

    float timer = 0;
    void First(float deltaTime)
    {
        timer += deltaTime * speed;
        SetPos(Matrix4x4.identity.Lerp(detA, timer));
        
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
        SetPos(detA.Lerp(Matrix4x4.identity, timer));

        if (timer >= 1f)
        {
            timer = 0;
            phase++;
        }
    }
}

public static partial class Matrix4x4Extensions
{
    static Matrix4x4 temp = new Matrix4x4();
    public static Matrix4x4 Lerp(this Matrix4x4 detA, Matrix4x4 detB, float t)
    {
        for (int i = 0; i < 4; i++)
        {
            temp.SetRow(i, Vector4.Lerp(detA.GetRow(i), detB.GetRow(i), t));
        }
        return temp;
    }
}

