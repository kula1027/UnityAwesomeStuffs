using UnityEngine;

public struct DoubleVec2 {
    public double x;
    public double y;

    public DoubleVec2(double x, double y) {
        this.x = x;
        this.y = y;
    }

    public static DoubleVec2 operator +(DoubleVec2 a) => a;

    public static DoubleVec2 operator -(DoubleVec2 a) => new DoubleVec2(-a.x, a.y);

    public static DoubleVec2 operator +(DoubleVec2 a, DoubleVec2 b)
    => new DoubleVec2(a.x + b.x, a.y + b.y);

    public static DoubleVec2 operator -(DoubleVec2 a, DoubleVec2 b)
    => a + (-b);

    public static DoubleVec2 operator *(DoubleVec2 a, double b)
    => new DoubleVec2(a.x * b, a.y * b);

    public static DoubleVec2 operator /(DoubleVec2 a, double b)
    => new DoubleVec2(a.x / b, a.y / b);

    public Vector2 ToUnityVec() => new Vector2((float)x, (float)y);

    public override string ToString() {
        return $"({x:F2}, {y:F2})";
    }
}