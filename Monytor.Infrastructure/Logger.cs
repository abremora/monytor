using System;

namespace Monytor.Infrastructure {
    public class Logger {
        public static void Info(string message) {
            Console.WriteLine($"{DateTime.Now}: {message}");
        }

        public static void Text(string message) {
            Console.WriteLine(message);
        }

        public static void Warning(string message) {
            ColorLog(message, ConsoleColor.Yellow);   
        }

        public static void Error(Exception ex, string message = null) {
            if (ex == null) return;

            if(message != null) ColorLog(message, ConsoleColor.Red);
            ColorLog(ex.ToString(), ConsoleColor.Red);
        }

        public static void ColorLog(string message, ConsoleColor color) {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Info(message);
            Console.ForegroundColor = temp;
        }

        public static void NewLine() {
            Console.WriteLine();
        }
    }
}