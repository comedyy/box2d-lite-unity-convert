using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

[ExecuteInEditMode]
public class NewBehaviourScript : MonoBehaviour
{
    public SquareCollider square1 = new SquareCollider() { position = new Vector2(0, 0), rotation = Mathf.PI / 6, width = new Vector2(2, 2) };
    public SquareCollider square2 = new SquareCollider() { position = new Vector2(2.8f, 0), rotation = 0, width = new Vector2(2f, 4f) };
    public CircleCollider circle = new CircleCollider() { position = new Vector2(1, 2), radius = 1};
    public LineCollider line = new LineCollider() { p1 = new Vector2(-1, 1), p2 = new Vector2(2, 4) };
    public LineCollider line_1 = new LineCollider() { p1 = new Vector2(1, -1), p2 = new Vector2(5, -1.5f)};

    void OnDrawGizmos()
    {
        // rotate item
        square1.rotation += 0.1f;
        square2.rotation += -0.1f;

        bool intersect_square_1_2 = Intersect.Detect(square1, square2);
        bool intersect_square1_circle = Intersect.Detect(square1, circle);
        bool intersect_square2_circle = Intersect.Detect(square2, circle);

        Vector2 normal = Vector2.zero;
        float fratction = 0;
        bool intersect_line_circle = Intersect.Detect(circle, line, out normal, out fratction);

        bool intersect_line_square = Intersect.Detect(square2, line_1, out normal, out fratction);

        // body
        DrawSquare(square1, intersect_square_1_2 || intersect_square1_circle);
        DrawSquare(square2, intersect_square_1_2 || intersect_square2_circle);
        DrawCicle(circle, intersect_square2_circle || intersect_square1_circle);
        DrawLine(line, intersect_line_circle, normal, fratction);
        DrawLine(line_1, intersect_line_square, normal, fratction);
    }

    private void DrawLine(LineCollider line, bool intersect_line_circle, Vector2 normal, float fraction)
    {
        Gizmos.color = intersect_line_circle ? Color.green : Color.blue;
        Gizmos.DrawLine(line.p1, line.p2);
        if (intersect_line_circle)
        {
            Gizmos.color = Color.yellow;
            Vector2 pos_intersect = line.p1 + fraction * (line.p2 - line.p1);
            Gizmos.DrawLine(pos_intersect, pos_intersect + normal);
        }
    }

    private void DrawCicle(CircleCollider circle, bool is_intersect)
    {
        Gizmos.color = is_intersect ? Color.green : Color.blue;
        Gizmos.DrawWireSphere(circle.position, circle.radius);

        Gizmos.color = Color.red;
        Vector2 v = new Vector2(circle.radius * 2, circle.radius * 2);
        Vector2[] vec_aabb =
        {
            circle.position + new Vector2(v.x / 2, v.y / 2),
            circle.position + new Vector2(v.x / 2, -v.y / 2),
            circle.position + new Vector2(-v.x / 2, -v.y / 2),
            circle.position + new Vector2(-v.x / 2, v.y / 2),
        };
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(vec_aabb[i], vec_aabb[(i + 1) % 4]);
        }
    }

    static void DrawSquare(SquareCollider b, bool is_intersect)
    {
        Gizmos.color = is_intersect ? Color.green : Color.blue;
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

        // normal(通过rotate来取出normal) 原理是：我们把初始法线值（0，1）旋转角度之后，得到的是col2，初始法线（1，0）旋转角度之后是col1
        //Gizmos.color = Color.green;
        //Gizmos.DrawLine(b.position, rot.col1+ b.position);
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawLine(b.position, -rot.col1+ b.position);
        //Gizmos.DrawLine(b.position, rot.col2+ b.position);
        //Gizmos.DrawLine(b.position, -rot.col2+ b.position);
    }
}
