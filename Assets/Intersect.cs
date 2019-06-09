using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public static class Intersect
    {
        public static bool Detect(SquareCollider bodyA, SquareCollider bodyB)
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
            Mat22 absC = MathUtils.Abs(C);
            Mat22 absCT = absC.Transpose();

            // Box A faces
            Vector2 faceA = MathUtils.Abs(dA) - hA - absC * hB;
            if (faceA.x > 0.0f || faceA.y > 0.0f)
            {
                return false;
            }

            // Box B faces
            Vector2 faceB = MathUtils.Abs(dB) - absCT * hA - hB;
            if (faceB.x > 0.0f || faceB.y > 0.0f)
            {
                return false;
            }

            return true;
        }

        public static bool Detect(CircleCollider bodyA, CircleCollider bodyBb)
        {
            int total_radius = bodyA.radius + bodyBb.radius;
            Vector2 vec_pos_diff = bodyA.position - bodyBb.position;
            return vec_pos_diff.sqrMagnitude > total_radius * total_radius;
        }

        public static bool Detect(SquareCollider bodyA, CircleCollider bodyB)
        {
            Vector2 hA = 0.5f * bodyA.width;
            Vector2 hB = new Vector2(bodyB.radius, bodyB.radius);

            Vector2 posA = bodyA.position;
            Vector2 posB = bodyB.position;

            Mat22 RotA = new Mat22(bodyA.rotation);

            Mat22 RotAT = RotA.Transpose();

            Vector2 dp = posB - posA;          // 距离向量
            Vector2 dA = RotAT * dp;           // 距离向量，在A的局部坐标系里面表现

            Vector2 v = MathUtils.Abs(dA);          // 转到第一象限
            Vector2 u = MathUtils.Max(v - hA, 0);                           // 求出最近的点
            return u.sqrMagnitude <= bodyB.radius * bodyB.radius;           // 计算距离
        }

        /*
         * 通过 二元一次方程来解决这个问题
         * axx+bx+c=0的根是(-b-sqrt(bb-4ac))/2a
         * 
         * x(t) = o + td    o是起始点，t=【0，1】，d是 p2 - p1 （参数函数）
         * |x - c_p|平方 = r平方   c是中点，r是半径
         * |td + (o-c_p)|平方 = r平方
         *  ddtt+ 2d(o-c_p)t + (o-c_p)(o-c_p) - rr = 0
         *  求出的t就是需要求的第一个相交点
         *  s = o - c_p
         *  b = sd  
         *  c = ss - rr
         *  带入方程
         *  ddtt + 2bt + c = 0
         *  sigma = sqrt(delta)/2 = sqrt(bb-ddc)
         *  t = -c - sigma        
         * 
         */
        public static bool Detect(CircleCollider body, LineCollider line, out Vector2 normal, out float fraction)
        {
            normal = Vector2.zero;
            fraction = 0;
            Vector2 s = line.p1 - body.position;
            float c = Vector2.Dot(s, s) - body.radius * body.radius; 

            // Solve quadratic equation.
            Vector2 d = line.p2 - line.p1;      
            float b = Vector2.Dot(s, d);        
            float dd = Vector2.Dot(d, d);       
            float sigma = b * b - dd * c;       

            // Check for negative discriminant and short segment.
            if (sigma < 0.0f || dd < float.MinValue)
            {
                return false;
            }

            // Find the point of intersection of the line with the circle.
            float a = -(b + Mathf.Sqrt(sigma));

            // Is the intersection point on the segment?
            if (0.0f <= a)
            {
                a /= dd;
                fraction = a;
                normal = s + a * d;
                normal.Normalize();
                return true;
            }

            return false;
        }
    }
}
