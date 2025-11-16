using System.Collections.Generic;
using System.Linq;

namespace Recognizer;

public static class ThresholdFilterTask
{
	public static double[,] ThresholdFilter(double[,] original, double whitePixelsFraction)
	{
		var width = original.GetLength(0);
		var height = original.GetLength(1);
		var result = new double[width, height];
		
		// Находим пороговое значение
		var threshold = FindThreshold(original, whitePixelsFraction);
		
		// Применяем пороговое преобразование
		for (var x = 0; x < width; x++)
		{
			for (var y = 0; y < height; y++)
			{
				result[x, y] = original[x, y] >= threshold ? 1.0 : 0.0;
			}
		}
		
		return result;
	}
	
	private static double FindThreshold(double[,] original, double whitePixelsFraction)
	{
		var width = original.GetLength(0);
		var height = original.GetLength(1);
		var totalPixels = width * height;
		var minWhitePixels = (int)(whitePixelsFraction * totalPixels);
		
		var thresholdValue = HandleBoundaryCase(minWhitePixels, totalPixels);
		if (thresholdValue.HasValue)
		{
			return thresholdValue.Value;
		}
		
		var sortedPixels = GetSortedPixelValues(original, width, height);
		return sortedPixels[minWhitePixels - 1];
	}
	
	private static double? HandleBoundaryCase(int minWhitePixels, int totalPixels)
	{
		if (minWhitePixels == 0)
		{
			return double.MaxValue;
		}
		if (minWhitePixels >= totalPixels)
		{
			return double.MinValue;
		}
		return null;
	}
	
	private static List<double> GetSortedPixelValues(double[,] original, int width, int height)
	{
		var pixelValues = CollectPixelValues(original, width, height);
		pixelValues.Sort((a, b) => b.CompareTo(a));
		return pixelValues;
	}
	
	private static List<double> CollectPixelValues(double[,] original, int width, int height)
	{
		var pixelValues = new List<double>();
		for (var x = 0; x < width; x++)
		{
			for (var y = 0; y < height; y++)
			{
				pixelValues.Add(original[x, y]);
			}
		}
		return pixelValues;
	}
}
