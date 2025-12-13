using System;
using NUnit.Framework;

namespace Manipulation;

public static class ManipulatorTask
{
	public static double[] MoveManipulatorTo(double x, double y, double angle)
	{
		double palmEndY = y + Math.Sin(Math.PI - angle) * Manipulator.Palm;
		double palmEndX = x + Math.Cos(Math.PI - angle) * Manipulator.Palm;
		double distanceToWrist = Math.Sqrt(palmEndY * palmEndY + palmEndX * palmEndX);
		
		double elbowAngle = TriangleTask.GetABAngle(Manipulator.UpperArm,
			Manipulator.Forearm, distanceToWrist);
		double shoulderAngle = TriangleTask.GetABAngle(Manipulator.UpperArm,
			distanceToWrist, Manipulator.Forearm) + Math.Atan2(palmEndY, palmEndX);
		double wristAngle = 3 * Math.PI - shoulderAngle - elbowAngle - angle - Math.PI;
		
		if (double.IsNaN(shoulderAngle) || double.IsNaN(elbowAngle) || double.IsNaN(wristAngle))
			return new[] { double.NaN, double.NaN, double.NaN };
		
		return new[] { shoulderAngle, elbowAngle, wristAngle };
	}
}

[TestFixture]
public class ManipulatorTask_Tests
{
	[Test]
	public void TestMoveManipulatorTo()
	{
		var angles = ManipulatorTask.MoveManipulatorTo(100, 100, 0);
		Assert.That(angles, Is.Not.Null);
		Assert.That(angles.Length, Is.EqualTo(3));
		Assert.That(double.IsNaN(angles[0]), Is.False);
		Assert.That(double.IsNaN(angles[1]), Is.False);
		Assert.That(double.IsNaN(angles[2]), Is.False);
	}

	[Test]
	public void TestMoveManipulatorToWithDifferentAngles()
	{
		var angles1 = ManipulatorTask.MoveManipulatorTo(150, 150, Math.PI / 4);
		var angles2 = ManipulatorTask.MoveManipulatorTo(150, 150, Math.PI / 2);
		
		Assert.That(angles1, Is.Not.Null);
		Assert.That(angles2, Is.Not.Null);
		Assert.That(angles1[2], Is.Not.EqualTo(angles2[2]).Within(1e-10));
	}

	[Test]
	public void TestMoveManipulatorToInvalidPosition()
	{
		var angles = ManipulatorTask.MoveManipulatorTo(1000, 1000, 0);
		Assert.That(double.IsNaN(angles[0]) || double.IsNaN(angles[1]) || double.IsNaN(angles[2]), Is.True);
	}
}