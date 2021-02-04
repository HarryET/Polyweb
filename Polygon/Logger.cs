using System;
using Polygon.Interfaces;

namespace Polygon
{
    public class Logger : ILogger
    {
        public void Info(string message)
        {
            Console.WriteLine($"[Polygon][{DateTime.Now:u}][INFO] {message}");
        }

        public void Error(string message)
        {
            Console.WriteLine($"[Polygon][{DateTime.Now:u}][ERROR] {message}");
        }
    }
}