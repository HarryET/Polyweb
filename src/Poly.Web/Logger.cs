using System;
using Poly.Web.Interfaces;

namespace Poly.Web
{
    public class Logger : ILogger
    {
        public void Info(string message)
        {
            Console.WriteLine($"[Polyweb][{DateTime.Now:u}][INFO] {message}");
        }

        public void Error(string message)
        {
            Console.WriteLine($"[Polyweb][{DateTime.Now:u}][ERROR] {message}");
        }
    }
}