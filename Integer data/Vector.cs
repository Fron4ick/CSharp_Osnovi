namespace Geometry
{
    public class Vector
    {
        public double X;
        public double Y;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public Vector Add(Vector other)
        {
            return Geometry.Add(this, other);
        }

        public bool Belongs(Segment segment)
        {
            return Geometry.IsVectorInSegment(this, segment);
        }
    }

    public class Segment
    {
        public Vector Begin;
        public Vector End;

        public double GetLength()
        {
            return Geometry.GetLength(this);
        }

        public bool Contains(Vector point)
        {
            return Geometry.IsVectorInSegment(point, this);
        }
    }

    public class Geometry
    {
        public static double GetLength(Vector vector)
        {
            return System.Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static double GetLength(Segment segment)
        {
            Vector diff = new Vector { X = segment.End.X - segment.Begin.X, Y = segment.End.Y - segment.Begin.Y };
            return GetLength(diff);
        }

        public static Vector Add(Vector vector1, Vector vector2)
        {
            return new Vector { X = vector1.X + vector2.X, Y = vector1.Y + vector2.Y };
        }

        public static bool IsVectorInSegment(Vector point, Segment segment)
        {
            double minX = System.Math.Min(segment.Begin.X, segment.End.X);
            double maxX = System.Math.Max(segment.Begin.X, segment.End.X);
            double minY = System.Math.Min(segment.Begin.Y, segment.End.Y);
            double maxY = System.Math.Max(segment.Begin.Y, segment.End.Y);

            if (point.X < minX || point.X > maxX || point.Y < minY || point.Y > maxY)
            {
                return false;
            }

            Vector segmentVec = new Vector { X = segment.End.X - segment.Begin.X, Y = segment.End.Y - segment.Begin.Y };
            Vector pointVec = new Vector { X = point.X - segment.Begin.X, Y = point.Y - segment.Begin.Y };

            double segmentLengthSq = segmentVec.X * segmentVec.X + segmentVec.Y * segmentVec.Y;
            double dotProduct = pointVec.X * segmentVec.X + pointVec.Y * segmentVec.Y;

            if (dotProduct < 0 || dotProduct > segmentLengthSq)
            {
                return false;
            }

            double crossProduct = pointVec.X * segmentVec.Y - pointVec.Y * segmentVec.X;
            return System.Math.Abs(crossProduct) < 1e-10;
        }
    }
}
