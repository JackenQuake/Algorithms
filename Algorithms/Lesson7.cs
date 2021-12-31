using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms {
    class Lesson7_Utility { // -------------------------------------------------------------------- Some utility functions

        // using some support functions from Lesson1_Utility class

    }
    class Lesson7_Task1_MinVar {
        static long[] Data;
        private static long RecursiveMinVar(int Number) {  // ------------------------------------- Recursive method to find minimal variable
            if (Data[Number] == 0) 
                if (Number == 1) Data[Number] = 1; else Data[Number] = RecursiveMinVar(Number - 1) + ((Number % 2 == 0) ? RecursiveMinVar(Number / 2) : 0);
            return Data[Number];
        }
        /// <summary>
        /// Function NonRecursiveMinVar is not needed! Instead of it - NonRecursive method is realised in Main-cycle.
        /// </summary>
        public static void Main_Recursive() {
            //Console.WriteLine($"{RecursiveMinVar(Lesson1_Utility.InputPositiveNumber())}");
            Data = new long[101];
            for (int i = 0; i <= 100; i++) Data[i] = 0;
            for (int i = 1; i <= 100; i++) Console.WriteLine($"i = {i, 3}: Result = {RecursiveMinVar(i), 4}");
        }
        public static void Main_NonRecursive() {
            //Console.WriteLine($"{NonRecursiveMinVar(Lesson1_Utility.InputPositiveNumber())}");
            Data = new long[101];
            for (int i = 0; i <= 100; i++) Data[i] = 0;
            Data[1] = 1;
            for (int i = 1; i <= 100; i++) {
                if (i > 1) Data[i] = Data[i - 1] + ((i % 2 == 0) ? Data[i / 2] : 0);
                Console.WriteLine($"i = {i, 3}: Result = {Data[i], 4}");
            }
        }
    }
    class Lesson7_Task2_NumPth {
        public static void Main_NumPath() { 
            Console.WriteLine("Number of rows (N-dimention):");
            int N = Lesson1_Utility.InputPositiveNumber();
            Console.WriteLine("Number of columns (M-dimention):");
            int M = Lesson1_Utility.InputPositiveNumber();
            Console.WriteLine();
            var Data = new int[N, M];
            int i, j;
            for (j = 0; j < M; j++) {
                Data[0, j] = 1;
                Console.Write($"{1, 4}");
            }
            Console.WriteLine();
            for (i = 1; i < N; i++) {
                Data[i, 0] = 1; Console.Write($"{1, 4}");
                for (j = 1; j < M; j++) {
                    Data[i, j] = Data[i, j - 1] + Data[i - 1, j];
                    Console.Write($"{Data[i, j], 4}");
                }
                Console.WriteLine();
            }
        }
    }
}
