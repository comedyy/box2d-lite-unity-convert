using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public enum ColliderType {
        Circle,
        Square
    }

    public struct SquareCollider
    {
        public Vector2 position;
        public float rotation;
        public Vector2 width;

        public static Vector2[] normals = {
                new Vector2(0, -1),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(-1, 0),
            };

        public static Vector2[] vecties = {
            new Vector2(-0.5f, -0.5f),
            new Vector2(0.5f, -0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(-0.5f, 0.5f),
        };
    }

    public struct CircleCollider {
        public Vector2 position;
        public int radius;
    }

    public struct LineCollider
    {
        public Vector2 p1;
        public Vector2 p2; 
    }
}
