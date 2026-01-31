using System;
using NUnit.Framework;

namespace Manipulation;

public class TriangleTask
{
    public static double GetABAngle(double sideOne, double sideTwo, double opposite)
    {
        if (sideOne <= 0 || sideTwo <= 0 || opposite < 0)
            return double.NaN;
        
        return (opposite != 0) 
            ? Math.Acos((sideOne * sideOne + sideTwo * sideTwo - opposite * opposite) / (2 * sideOne * sideTwo))
            : 0;
    }
}

[TestFixture]
public class TriangleTask_Tests
{
    [TestCase(3, 4, 5, Math.PI / 2)]
    [TestCase(1, 1, 1, Math.PI / 3)]
    [TestCase(5, 5, 5, Math.PI / 3)]
    [TestCase(1, 1, 0, 0)]
    public void TestGetABAngle(double sideOne, double sideTwo, double opposite, double expectedAngle)
    {
        var actualAngle = TriangleTask.GetABAngle(sideOne, sideTwo, opposite);
        Assert.That(actualAngle, Is.EqualTo(expectedAngle).Within(1e-10));
    }
}