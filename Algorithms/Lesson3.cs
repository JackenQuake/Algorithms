using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Algorithms {
    /*
    class Lesson3_Utility {  // ------------------------------------------------------------------- Some utility functions
    it's empty for this lesson
    }
    */
    /// <summary>
    /// Class and structures describing points
    /// </summary>
    public class PointClassFloat {
        public float x;
        public float y;
        public PointClassFloat(float _X, float _Y) {  /// ---------------------------------------------- Constructor
            x = _X;
            y = _Y;
        }
    }
    public class PointClassDouble {
        public double x;
        public double y;
        public PointClassDouble(double _X, double _Y) {  /// ---------------------------------------------- Constructor
            x = _X;
            y = _Y;
        }
    }
    public struct PointStructFloat {
        public float x;
        public float y;
        public void set(float _X, float _Y) {  /// ------------------------------------------------ Function to onveniently set variables
            x = _X;
            y = _Y;
        }
    }
    public struct PointStructDouble {
        public double x;
        public double y;
        public void set(double _X, double _Y) {  /// ---------------------------------------------- Function to onveniently set variables
            x = _X;
            y = _Y;
        }
    }
    /// <summary>
    /// Core for bench-functions
    /// </summary>
    public class Lesson3_CoreAlt {
        public int TestSize;
        public PointClassFloat originClassFloat;
        public PointStructFloat originStructFloat;
        public PointStructDouble originStructDouble;
        public PointClassFloat[] ClassFloatArray;
        public PointStructFloat[] StructFloatArray;
        public PointStructDouble[] StructDoubleArray;
        public float PointDistance(PointClassFloat pointOne, PointClassFloat pointTwo) {
            float x = pointOne.x - pointTwo.x;
            float y = pointOne.y - pointTwo.y;
            return (float)Math.Sqrt((x * x) + (y * y));
        }
        public float PointDistance(PointStructFloat pointOne, PointStructFloat pointTwo) {
            float x = pointOne.x - pointTwo.x;
            float y = pointOne.y - pointTwo.y;
            return (float)Math.Sqrt((x * x) + (y * y));
        }
        public  float PointDistanceShort(PointStructFloat pointOne, PointStructFloat pointTwo) {
            float x = pointOne.x - pointTwo.x;
            float y = pointOne.y - pointTwo.y;
            return (x * x) + (y * y);
        }
        public double PointDistance(PointStructDouble pointOne, PointStructDouble pointTwo) {
            double x = pointOne.x - pointTwo.x;
            double y = pointOne.y - pointTwo.y;
            return Math.Sqrt((x * x) + (y * y));
        }
        public void InitArrays() {
            Random rnd = new Random();
            TestSize = 10000;
            originClassFloat = new PointClassFloat(0, 0);
            originStructFloat.set(0, 0);
            originStructDouble.set(0, 0);
            ClassFloatArray = new PointClassFloat[TestSize];
            StructFloatArray = new PointStructFloat[TestSize];
            StructDoubleArray = new PointStructDouble[TestSize];
            for (int i = 0; i < TestSize; i++) {
                ClassFloatArray[i] = new PointClassFloat((float)(0.01 * rnd.Next(10000)), (float)(0.01 * rnd.Next(10000)));
                StructFloatArray[i].set(ClassFloatArray[i].x, ClassFloatArray[i].y);
                StructDoubleArray[i].set(ClassFloatArray[i].x, ClassFloatArray[i].y);
            }
        }
        [Benchmark]
        public void Test_ClassFloat() {
            for (int i = 0; i < TestSize; i++) PointDistance(originClassFloat, ClassFloatArray[i]);
        }
        [Benchmark]
        public void Test_StructFloat() {
            for (int i = 0; i < TestSize; i++) PointDistance(originStructFloat, StructFloatArray[i]);
        }
        [Benchmark]
        public void Test_StructDouble() {
            for (int i = 0; i < TestSize; i++) PointDistance(originStructDouble, StructDoubleArray[i]);
        }
        [Benchmark]
        public void Test4_StructFloatShort() {
            for (int i = 0; i < TestSize; i++) PointDistanceShort(originStructFloat, StructFloatArray[i]);
        }
    }
    /// <summary>
    /// Main class, executes benchmark
    /// </summary> 
    class Lesson3_Task1_Alt {  /// ---------------------------------------------------------------- Alternate Bench 
        public static void Main_Benchmark() {
            Lesson3_CoreAlt Core = new Lesson3_CoreAlt();
            Core.InitArrays();
            string[] args = new string[2];
            args[0] = "-f";
            args[1] = "*Lesson3_CoreAlt*";
            BenchmarkSwitcher.FromAssembly(typeof(Lesson3_CoreAlt).Assembly).Run(args);
        }
    }
    public class Lesson3_CoreMain {
        public int TestSize;
        public PointClassDouble originClassDouble;
        public PointStructDouble originStructDouble;
        public PointClassDouble[] ClassDoubleArray;
        public PointStructDouble[] StructDoubleArray;
        public double PointDistance(PointClassDouble pointOne, PointClassDouble pointTwo) {
            double x = pointOne.x - pointTwo.x;
            double y = pointOne.y - pointTwo.y;
            return Math.Sqrt((x * x) + (y * y));
        }
        public double PointDistance(PointStructDouble pointOne, PointStructDouble pointTwo) {
            double x = pointOne.x - pointTwo.x;
            double y = pointOne.y - pointTwo.y;
            return Math.Sqrt((x * x) + (y * y));
        }
        public void InitArrays() {
            Random rnd = new Random();
            int InputNumber = 0;
            do {
                Console.WriteLine("Please choose length of arrays: ");
                Console.WriteLine("1. Length equals 100000");
                Console.WriteLine("2. Length equals 200000");
                Console.Write(":> ");
                try { InputNumber = Int32.Parse(Console.ReadLine()); } catch { Console.WriteLine("Input error. Please try again."); };
            } while ((InputNumber !=1) && (InputNumber!=2));
            TestSize = 100000 * InputNumber;
            originClassDouble = new PointClassDouble(0, 0);
            originStructDouble.set(0, 0);
            ClassDoubleArray = new PointClassDouble[TestSize];
            StructDoubleArray = new PointStructDouble[TestSize];
            for (int i = 0; i < TestSize; i++) {
                ClassDoubleArray[i] = new PointClassDouble(0.01 * rnd.Next(10000), 0.01 * rnd.Next(10000));
                StructDoubleArray[i].set(ClassDoubleArray[i].x, ClassDoubleArray[i].y);
            }
        }
        [Benchmark]
        public void Test_ClassDouble() {
            for (int i = 0; i < TestSize; i++) PointDistance(originClassDouble, ClassDoubleArray[i]);
        }
        [Benchmark]
        public void Test_StructDouble() {
            for (int i = 0; i < TestSize; i++) PointDistance(originStructDouble, StructDoubleArray[i]);
        }
    }
    class Lesson3_Task1_Main { /// ---------------------------------------------------------------- Main Bench
        public static void Main_Benchmark() {
            Lesson3_CoreMain Core = new Lesson3_CoreMain();
            Core.InitArrays();
            string[] args = new string[2];
            args[0] = "-f";
            args[1] = "*Lesson3_CoreMain*";
            BenchmarkSwitcher.FromAssembly(typeof(Lesson3_CoreMain).Assembly).Run(args);
        }
    }
}
