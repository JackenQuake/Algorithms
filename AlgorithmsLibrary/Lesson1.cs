using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Algorithms {
    class Lesson1_Utility { // -------------------------------------------------------------------- Some utility functions
        public static int InputPositiveNumber() {
            int Number;
            do {
                Console.Write("Please enter positive number :> ");
                try {
                    Number = Int32.Parse(Console.ReadLine());
                    if (Number <= 0) Console.WriteLine("Positive number means number above zero. Please try again.");
                }
                catch { Console.WriteLine("Incorrect number. Please try again."); Number = 0; };
            } while (Number <= 0);
            return Number;
        }
    }
    public class Lesson1_Task1_Prime {
        public static bool PrimeNumber(int number) {  // ------------------------------------------ The Function requested
            int d = 0, i;
            for (i = 2; i < number; i++) if (number % i == 0) d++;
            if (d == 0) return true; else return false;
        }
        public static void Main_Automatic() {  // ------------------------------------------------- Automatic test
            Console.WriteLine($"Positive result for number <727> : {PrimeNumber(727)}");
            Console.WriteLine($"Negative result for number <371> : {PrimeNumber(371)}");
            //for (int i = 0; i < 1000; i++) Console.WriteLine($"Result for number <{i}> : {PrimeNumber(i)}");
        }
        public static void Main_UserInput() { // -------------------------------------------------- Manual test
            int myNumber = Lesson1_Utility.InputPositiveNumber();
            Console.WriteLine($"Result for number <{myNumber}> : {PrimeNumber(myNumber)}");
        }
    }
    public class Lesson1_Task2_Compl {
        public static void Main_Complexity() {  // ------------------------------------------------ Printing text file with unswer
            string str = File.ReadAllText("Lesson1_Task2.md");
            Console.WriteLine(str);
        }
    }
    public class Lesson1_Task3_Fibo {
        private static long RecursiveFibonacci(int Number) {  // ---------------------------------- Recursive Fibonacci
            if (Number < 2) return Number;
            return RecursiveFibonacci(Number - 1) + RecursiveFibonacci(Number - 2);
        }
        private static long NonRecursiveFibonacci(int Number) {  // ------------------------------- Non-recursive (Cycle) Fibonacci
            long a = 0, b = 1, c = 1;
            for (int i = 2; i <= Number; i++) {
                c = a + b; a = b; b = c;
            }
            return c;
        }
        public static void Main_Recursive() {  // ------------------------------------------------- Test recursive method
            Console.WriteLine("Recursive Fibonacci algorithm:");
            int myNumber = Lesson1_Utility.InputPositiveNumber();
            Console.WriteLine($"{myNumber}-th Fibonacci number is: {RecursiveFibonacci(myNumber)}");
        }
        public static void Main_NonRecursive() { // ----------------------------------------------- Test non-recursive method
            Console.WriteLine("Non-Recursive Fibonacci algorithm:");
            int myNumber = Lesson1_Utility.InputPositiveNumber();
            Console.WriteLine($"{myNumber}-th Fibonacci number is: {NonRecursiveFibonacci(myNumber)}");
        }
    }
}
