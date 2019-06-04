using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class NewBehaviourScript : MonoBehaviour
{
    public Body bodyA = new Body() { position = new Vector2(0, 0), rotation = Mathf.PI / 6, width = new Vector2(2, 2) };
    public Body bodyB = new Body() { position = new Vector2(2.8f, 0), rotation = 0, width = new Vector2(2f, 4f) };
    public Text contact;

    void Update ()
    {
        Vector2 hA = 0.5f * bodyA.width;
        Vector2 hB = 0.5f * bodyB.width;

        Vector2 posA = bodyA.position;
        Vector2 posB = bodyB.position;

        Mat22 RotA = new Mat22(bodyA.rotation);
        Mat22 RotB = new Mat22(bodyB.rotation);

        Mat22 RotAT = RotA.Transpose();
        Mat22 RotBT = RotB.Transpose();

        Vector2 dp = posB - posA;          // 距离向量
        Vector2 dA = RotAT * dp;           // 距离向量，在A的局部坐标系里面表现
        Vector2 dB = RotBT * dp;           // 距离向量，在B的局部坐标里面的表现。

        Mat22 C = RotAT * RotB;
        Mat22 absC =  MathUtils.Abs(C);
        Mat22 absCT = absC.Transpose();

        // Box A faces
        Vector2 faceA = MathUtils.Abs(dA) - hA - absC * hB;
        if (faceA.x > 0.0f || faceA.y > 0.0f)
        {
            contact.text = "not contact";
            return;
        }

        // Box B faces
        Vector2 faceB = MathUtils.Abs(dB) - absCT * hA - hB;
        if (faceB.x > 0.0f || faceB.y > 0.0f)
        {
            contact.text = "not contact";
            return;
        }

        contact.text = "contacted";
    }

    void OnDrawGizmos()
    {
        // body
        DrawBody(bodyA);
        DrawBody(bodyB);
    }

    static void DrawBody(Body b)
    {
        Gizmos.color = Color.blue;
        Vector2[] points = 
        {
            b.position + new Vector2(b.width.x / 2, b.width.y / 2),
            b.position + new Vector2(b.width.x / 2, -b.width.y / 2),
            b.position + new Vector2(-b.width.x / 2, -b.width.y / 2),
            b.position + new Vector2(-b.width.x / 2, b.width.y / 2),
        };

        Vector2[] rotated_points = new Vector2[4];
        Mat22 rot = new Mat22(b.rotation);
        for (int i = 0; i < 4; i++)
        {
            points[i] = rot * (points[i] - b.position) + b.position;
        }

        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(points[i], points[(i+1) %4]);
        }
        
        // oobb
        Gizmos.color = Color.red;
        Mat22 abs_rot = MathUtils.Abs(rot);
        Vector2 v = abs_rot * (b.width);
        Vector2[] vec_aabb =
        {
            b.position + new Vector2(v.x / 2, v.y / 2),
            b.position + new Vector2(v.x / 2, -v.y / 2),
            b.position + new Vector2(-v.x / 2, -v.y / 2),
            b.position + new Vector2(-v.x / 2, v.y / 2),
        };

        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(vec_aabb[i], vec_aabb[(i + 1) % 4]);
        }
    }
}
