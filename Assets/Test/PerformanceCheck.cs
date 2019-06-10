using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleIntersectDetect;
using System.Diagnostics;

public class PerformanceCheck : MonoBehaviour {
    const int OBJ_COUNT = 1000;
    SquareCollider[] squares = new SquareCollider[OBJ_COUNT];
    CircleCollider[] circles = new CircleCollider[OBJ_COUNT];
    RayCollider[] rays = new RayCollider[OBJ_COUNT];

	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < OBJ_COUNT; i++)
        {
            squares[i] = new SquareCollider() { position = new Vector2(Random.Range(0, 10), Random.Range(0, 10)), rotation = Random.Range(0, 359), width = new Vector2(Random.Range(0.1f, 2), Random.Range(0.1f, 2)) };
            circles[i] = new CircleCollider() { position = new Vector2(Random.Range(0, 10), Random.Range(0, 10)), radius = Random.Range(0.1f, 2)};
            rays[i] = new RayCollider() { p1 = new Vector2(Random.Range(0, 10), Random.Range(0, 10)), p2 = new Vector2(Random.Range(0, 10), Random.Range(0, 10)) };
        }

        TestSqureCircle();
        TestSqureSqure();
        TestCircleCircle();
        TestRaySquare();
        TestRayCircle();
    }

    void TestSqureCircle() {
        Stopwatch watch = new Stopwatch();
        watch.Start();

        for (int i = 0; i < OBJ_COUNT; i++)
        {
            for (int j = 0; j < OBJ_COUNT; j++)
            {
                Intersect.DetectSquareCircle(squares[i], circles[j]);
            }
        }
        UnityEngine.Debug.LogFormat("TestSqureCircle {0}", watch.ElapsedMilliseconds);
    }

    void TestSqureSqure() {
        Stopwatch watch = new Stopwatch();
        watch.Start();

        for (int i = 0; i < OBJ_COUNT; i++)
        {
            for (int j = 0; j < OBJ_COUNT; j++)
            {
                Intersect.DetectTwoSquare(squares[i], squares[j]);
            }
        }
        UnityEngine.Debug.LogFormat("TestSqureSqure {0}", watch.ElapsedMilliseconds);
    }

    void TestCircleCircle() {
        Stopwatch watch = new Stopwatch();
        watch.Start();

        for (int i = 0; i < OBJ_COUNT; i++)
        {
            for (int j = 0; j < OBJ_COUNT; j++)
            {
                Intersect.DetectTwoCircle(circles[i], circles[j]);
            }
        }
        UnityEngine.Debug.LogFormat("TestCircleCircle {0}", watch.ElapsedMilliseconds);
    }

    void TestRaySquare()
    {
        Vector2 normal;
        float fraction;

        Stopwatch watch = new Stopwatch();
        watch.Start();

        for (int i = 0; i < OBJ_COUNT; i++)
        {
            for (int j = 0; j < OBJ_COUNT; j++)
            {
                Intersect.RaycastSquare(squares[j], rays[i], out normal, out fraction);
            }
        }
        UnityEngine.Debug.LogFormat("TestRaySquare {0}", watch.ElapsedMilliseconds);
    }

    void TestRayCircle() {
        Vector2 normal;
        float fraction;

        Stopwatch watch = new Stopwatch();
        watch.Start();

        for (int i = 0; i < OBJ_COUNT; i++)
        {
            for (int j = 0; j < OBJ_COUNT; j++)
            {
                Intersect.RaycastCircle(circles[j], rays[i], out normal, out fraction);
            }
        }
        UnityEngine.Debug.LogFormat("TestRayCircle {0}", watch.ElapsedMilliseconds);
    }
}
