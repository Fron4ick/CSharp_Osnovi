using System;

namespace Recognizer;
internal static class SobelFilterTask
{
    public static double[,] SobelFilter(double[,] g, double[,] sx)
    {
        var width = g.GetLength(0);
        var height = g.GetLength(1);
        var result = new double[width, height];
        
        // Получаем транспонированную матрицу sy из sx
        var sy = TransposeMatrix(sx);
        
        // Определяем размер ядра свертки
        var kernelWidth = sx.GetLength(0);
        var kernelHeight = sx.GetLength(1);
        var offsetX = kernelWidth / 2;
        var offsetY = kernelHeight / 2;
        
        // Применяем свертку только к внутренним пикселям
        for (var x = offsetX; x < width - offsetX; x++)
        {
            for (var y = offsetY; y < height - offsetY; y++)
            {
                // Вычисляем градиенты по x и y с помощью свертки
                var gx = ApplyConvolution(g, sx, x, y, offsetX, offsetY);
                var gy = ApplyConvolution(g, sy, x, y, offsetX, offsetY);
                
                // Вычисляем магнитуду градиента
                result[x, y] = Math.Sqrt(gx * gx + gy * gy);
            }
        }
        
        return result;
    }
    
    private static double[,] TransposeMatrix(double[,] matrix)
    {
        var width = matrix.GetLength(0);
        var height = matrix.GetLength(1);
        var transposed = new double[height, width];
        
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                transposed[y, x] = matrix[x, y];
            }
        }
        
        return transposed;
    }
    
    private static double ApplyConvolution(double[,] image, double[,] kernel, 
        int centerX, int centerY, int offsetX, int offsetY)
    {
        var result = 0.0;
        var kernelWidth = kernel.GetLength(0);
        var kernelHeight = kernel.GetLength(1);
        
        for (var kx = 0; kx < kernelWidth; kx++)
        {
            for (var ky = 0; ky < kernelHeight; ky++)
            {
                var imageX = centerX + kx - offsetX;
                var imageY = centerY + ky - offsetY;
                result += image[imageX, imageY] * kernel[kx, ky];
            }
        }
        
        return result;
    }
}