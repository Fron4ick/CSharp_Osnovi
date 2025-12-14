using System;
using System.Drawing;
using Avalonia;
using NUnit.Framework;
using static Manipulation.Manipulator;

namespace Manipulation;

public static class AnglesToCoordinatesTask
{
	public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
	{
		float currentX = UpperArm * (float)Math.Cos(shoulder);
		float currentY = UpperArm * (float)Math.Sin(shoulder);
		var firstJoint = new PointF(currentX, currentY);
		
		currentX += Forearm * (float)Math.Cos(elbow + shoulder - Math.PI);
		currentY += Forearm * (float)Math.Sin(elbow + shoulder - Math.PI);
		var secondJoint = new PointF(currentX, currentY);
		
		currentX += Palm * (float)Math.Cos(wrist + elbow + shoulder - 2 * Math.PI);
		currentY += Palm * (float)Math.Sin(wrist + elbow + shoulder - 2 * Math.PI);
		var thirdJoint = new PointF(currentX, currentY);
		
		return new PointF[]
		{
			firstJoint,
			secondJoint,
			thirdJoint
		};
	}
}

[TestFixture]
public class AnglesToCoordinatesTask_Tests
{
	// Доработайте эти тесты!
	// С помощью строчки TestCase можно добавлять новые тестовые данные.
	// Аргументы TestCase превратятся в аргументы метода.
	[TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Forearm + Palm, UpperArm)]
	public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
	{
		var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
		Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
		Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
	}
}