using System.Collections.Generic;

namespace Recognizer;

internal static class MedianFilterTask
{
	/* 
	 * Для борьбы с пиксельным шумом, подобным тому, что на изображении,
	 * обычно применяют медианный фильтр, в котором цвет каждого пикселя, 
	 * заменяется на медиану всех цветов в некоторой окрестности пикселя.
	 * https://en.wikipedia.org/wiki/Median_filter
	 * 
	 * Используйте окно размером 3х3 для не граничных пикселей,
	 * Окно размером 2х2 для угловых и 3х2 или 2х3 для граничных.
	 */
	public static double[,] MedianFilter(double[,] original)
	{
		var width = original.GetLength(0);
		var height = original.GetLength(1);
		var filtered = new double[width, height];
		
		for (var x = 0; x < width; x++)
		{
			for (var y = 0; y < height; y++)
			{
				filtered[x, y] = GetFilteredPixelValue(original, x, y, width, height);
			}
		}
		
		return filtered;
	}
	
	private static double GetFilteredPixelValue(double[,] original, int x, int y, int width, int height)
	{
		var neighbors = GetNeighborValues(original, x, y, width, height);
		neighbors.Sort();
		return GetMedian(neighbors);
	}
	
	private static List<double> GetNeighborValues(double[,] original, int x, int y, int width, int height)
	{
		var neighbors = new List<double>();
		
		for (var dx = -1; dx <= 1; dx++)
		{
			for (var dy = -1; dy <= 1; dy++)
			{
				var nx = x + dx;
				var ny = y + dy;
				
				if (IsValidCoordinate(nx, ny, width, height))
				{
					neighbors.Add(original[nx, ny]);
				}
			}
		}
		
		return neighbors;
	}
	
	private static bool IsValidCoordinate(int x, int y, int width, int height)
	{
		return x >= 0 && x < width && y >= 0 && y < height;
	}
	
	private static double GetMedian(List<double> values)
	{
		var count = values.Count;
		if (count % 2 == 1)
		{
			return values[count / 2];
		}
		else
		{
			return (values[count / 2 - 1] + values[count / 2]) / 2.0;
		}
	}
}
