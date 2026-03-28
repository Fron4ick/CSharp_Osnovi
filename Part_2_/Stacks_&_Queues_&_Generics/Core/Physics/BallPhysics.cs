using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MagneticMaze.Core.Physics;

public class BallPhysics
{
    private readonly float _mass;
    private readonly float _magneticConstant;
    private readonly float _damping;
    private readonly float _maxSpeed;
    private readonly float _defaultRestitution;

    private const float BallRadius = 0.3f;
    private const float GoalTolerance = 0.2f;

    private readonly List<WallData> _walls = new();
    private Vector3 _position;
    private Vector3 _velocity;
    private Vector3 _goalPosition;
    private bool _hasLevel;

    public BallPhysics(float mass, float magneticConstant, float damping, float maxSpeed, float restitution)
    {
        _mass = mass;
        _magneticConstant = magneticConstant;
        _damping = damping;
        _maxSpeed = maxSpeed;
        _defaultRestitution = MathHelper.Clamp(restitution, 0f, 1f);
    }

    public void SetLevelData(List<WallData> walls, Vector3 startPosition, Vector3 goalPosition)
    {
        _walls.Clear();
        if (walls != null)
            _walls.AddRange(walls);

        _position = startPosition;
        _velocity = Vector3.Zero;
        _goalPosition = goalPosition;
        _hasLevel = true;
    }

    public void Update(float deltaTime, Vector3 magnetPosition, float magnetPolarity)
    {
        if (!_hasLevel || deltaTime <= 0f)
            return;

        var toMagnet = magnetPosition - _position;
        var distanceSq = toMagnet.LengthSquared();

        Vector3 force = Vector3.Zero;
        if (distanceSq > 1e-6f)
        {
            var direction = Vector3.Normalize(toMagnet);
            var strength = magnetPolarity * _magneticConstant / (distanceSq + 1e-6f);
            force = direction * strength;
        }

        var acceleration = force / _mass;

        _velocity += acceleration * deltaTime;

        if (_velocity.LengthSquared() > _maxSpeed * _maxSpeed)
        {
            _velocity.Normalize();
            _velocity *= _maxSpeed;
        }

        _velocity *= _damping;

        _position += _velocity * deltaTime;

        ResolveCollisions();
    }

    private void ResolveCollisions()
    {
        if (_walls.Count == 0)
            return;

        const int maxIterations = 3;

        for (var iteration = 0; iteration < maxIterations; iteration++)
        {
            var anyCollision = false;

            foreach (var wall in _walls)
            {
                if (!CheckSphereAabbIntersection(_position, BallRadius, wall.Min, wall.Max, out var penetrationNormal, out var penetrationDepth))
                    continue;

                anyCollision = true;

                _position += penetrationNormal * penetrationDepth;

                var vDotN = Vector3.Dot(_velocity, penetrationNormal);
                if (vDotN < 0f)
                {
                    var restitution = wall.Restitution > 0f ? wall.Restitution : _defaultRestitution;
                    var impulse = (1f + restitution) * vDotN;
                    _velocity -= impulse * penetrationNormal;
                }
            }

            if (!anyCollision)
                break;
        }
    }

    private static bool CheckSphereAabbIntersection(
        Vector3 center,
        float radius,
        Vector3 boxMin,
        Vector3 boxMax,
        out Vector3 normal,
        out float depth)
    {
        var closest = new Vector3(
            MathHelper.Clamp(center.X, boxMin.X, boxMax.X),
            MathHelper.Clamp(center.Y, boxMin.Y, boxMax.Y),
            MathHelper.Clamp(center.Z, boxMin.Z, boxMax.Z));

        var delta = center - closest;
        var distSq = delta.LengthSquared();
        var radiusSq = radius * radius;

        if (distSq > radiusSq)
        {
            normal = Vector3.Zero;
            depth = 0f;
            return false;
        }

        var dist = (float)Math.Sqrt(distSq);
        if (dist > 1e-6f)
        {
            normal = Vector3.Normalize(delta);
            depth = radius - dist;
        }
        else
        {
            var distances = new[]
            {
                center.X - boxMin.X,
                boxMax.X - center.X,
                center.Y - boxMin.Y,
                boxMax.Y - center.Y,
                center.Z - boxMin.Z,
                boxMax.Z - center.Z
            };

            var minIndex = 0;
            for (var i = 1; i < distances.Length; i++)
            {
                if (distances[i] < distances[minIndex])
                    minIndex = i;
            }

            depth = radius;
            normal = minIndex switch
            {
                0 => Vector3.Left,
                1 => Vector3.Right,
                2 => Vector3.Down,
                3 => Vector3.Up,
                4 => Vector3.Backward,
                5 => Vector3.Forward,
                _ => Vector3.Up
            };
        }

        return true;
    }

    public Vector3 GetBallPosition() => _position;

    public Vector3 GetBallVelocity() => _velocity;

    public bool HasReachedGoal()
    {
        if (!_hasLevel)
            return false;

        var distance = Vector3.Distance(_position, _goalPosition);
        return distance <= BallRadius + GoalTolerance;
    }
}

