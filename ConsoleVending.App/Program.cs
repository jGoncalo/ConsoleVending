using System;

namespace ConsoleVending.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var app = new AppUi();
            app.Init();
        }
    }
}