using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SimpleIntersectDetect
{
    class MathUtils
    {

        public static Vector2 Max(Vector2 a, float max)
        {
            return new Vector2(Mathf.Max(a.x, max), Mathf.Max(a.y, max));
        }

        public static Vector2 Abs(Vector2 a)
        {
            return new Vector2(Mathf.Abs(a.x), Mathf.Abs(a.y));
        }

        public static Mat22 Abs(Mat22 A)
        {
            return new Mat22(Abs(A.col1), Abs(A.col2));
        }
    }

    public struct Mat22
    {
        public Mat22(float angle)
        {
            float c = Mathf.Cos(angle);
            float s = Mathf.Sin(angle);
            col1.x = c; col2.x = -s;
            col1.y = s; col2.y = c;
        }

        public Mat22(Vector2 col1, Vector2 col2)
        {
            this.col1 = col1;
            this.col2 = col2;
        }

        public Mat22 Transpose()
        {
            return new Mat22(new Vector2(col1.x, col2.x), new Vector2(col1.y, col2.y));
        }

        public override string ToString()
        {
            return string.Format("{0} {1}\n{2} {3}", col1.x, col2.x, col1.y, col2.y);
        }

        public static Vector2 operator *(Mat22 A, Vector2 v)
        {
            return new Vector2(A.col1.x * v.x + A.col2.x * v.y, A.col1.y * v.x + A.col2.y * v.y);
        }

        public static Mat22 operator *(Mat22 A, Mat22 B)
        {
            return new Mat22(A * B.col1, A * B.col2);
        }

        public Vector2 col1;
        public Vector2 col2;
    }
}
