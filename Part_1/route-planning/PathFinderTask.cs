using System.Drawing;

namespace RoutePlanning;

public static class PathFinderTask
{
	public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
	{
		var bestOrder = new int[checkpoints.Length];
		var currentOrder = new int[checkpoints.Length];
		currentOrder[0] = 0;
		var used = new bool[checkpoints.Length];
		used[0] = true;
		var bestLength = double.MaxValue;
		
		FindBestOrderRecursive(checkpoints, currentOrder, used, 1, 0.0, bestOrder, ref bestLength);
		return bestOrder;
	}

	private static void FindBestOrderRecursive(
		Point[] checkpoints, 
		int[] currentOrder, 
		bool[] used, 
		int position,
		double currentLength,
		int[] bestOrder,
		ref double bestLength)
	{
		if (position == checkpoints.Length)
		{
			UpdateBestResult(currentOrder, currentLength, bestOrder, ref bestLength);
			return;
		}

		if (currentLength >= bestLength)
			return;

		var lastPoint = checkpoints[currentOrder[position - 1]];
		for (var i = 0; i < checkpoints.Length; i++)
		{
			if (used[i]) continue;
			
			var newLength = currentLength + lastPoint.DistanceTo(checkpoints[i]);
			if (newLength >= bestLength)
				continue;
			
			used[i] = true;
			currentOrder[position] = i;
			FindBestOrderRecursive(checkpoints, currentOrder, used, position + 1, newLength, bestOrder, ref bestLength);
			used[i] = false;
		}
	}

	private static void UpdateBestResult(int[] currentOrder, double currentLength, int[] bestOrder, ref double bestLength)
	{
		if (currentLength < bestLength)
		{
			bestLength = currentLength;
			currentOrder.CopyTo(bestOrder, 0);
		}
	}
}
