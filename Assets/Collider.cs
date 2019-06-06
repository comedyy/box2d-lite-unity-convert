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
    }

    public struct CircleCollider {
        public Vector2 position;
        public int radius;
    }
}
