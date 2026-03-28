using Microsoft.Xna.Framework;

namespace MagneticMaze.Core.Physics;

public struct WallData
{
    public Vector3 Min;
    public Vector3 Max;
    public float Restitution;
    public bool IsOpaque;
}

