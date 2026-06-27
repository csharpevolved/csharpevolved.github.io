public class Point
{
    public double X { get; set; }
    public double Y { get; set; }

    // Expression-bodied constructor (C# 7.0+)
    public Point(double x, double y) => (X, Y) = (x, y);

    // Expression-bodied method
    public double Distance() => Math.Sqrt(X * X + Y * Y);

    // Expression-bodied operator overload
    public static Point operator +(Point a, Point b)
        => new Point(a.X + b.X, a.Y + b.Y);

    public static Point operator -(Point a, Point b)
        => new Point(a.X - b.X, a.Y - b.Y);

    public static Point operator *(Point p, double scalar)
        => new Point(p.X * scalar, p.Y * scalar);

    // Expression-bodied method for validation
    public bool IsOrigin => X == 0 && Y == 0;

    // Expression-bodied method returning tuple
    public (double x, double y) AsAngle() => (
        X / Distance(),
        Y / Distance()
    );

    // Explicit conversion using expression body
    public static explicit operator Point(string coords)
    {
        var parts = coords.Split(',');
        return new Point(double.Parse(parts[0]), double.Parse(parts[1]));
    }

    // ToString as expression body
    public override string ToString() => $"({X}, {Y})";
}

// Record type (C# 9.0+) uses expression bodies by default
public record Rectangle(double Width, double Height)
{
    public double Area => Width * Height;
    public double Perimeter => 2 * (Width + Height);
    public bool IsSquare => Width == Height;
}
