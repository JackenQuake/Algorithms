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
    public class PointClass {
        public float x;
        public float y;
        public PointClass(float _X, float _Y) {  /// ---------------------------------------------- Constructor
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
    public class Lesson3_Core {
        public int TestSize;
        public PointClass originClass;
        public PointStructFloat originStructFloat;
        public PointStructDouble originStructDouble;
        public PointClass[] ClassFloatArray;
        public PointStructFloat[] StructFloatArray;
        public PointStructDouble[] StructDoubleArray;
        public float PointDistance(PointClass pointOne, PointClass pointTwo) {
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
            originClass = new PointClass(0, 0);
            originStructFloat.set(0, 0);
            originStructDouble.set(0, 0);
            ClassFloatArray = new PointClass[TestSize];
            StructFloatArray = new PointStructFloat[TestSize];
            StructDoubleArray = new PointStructDouble[TestSize];
            for (int i = 0; i < TestSize; i++) {
                ClassFloatArray[i] = new PointClass((float)(0.01 * rnd.Next(10000)), (float)(0.01 * rnd.Next(10000)));
                StructFloatArray[i].set(ClassFloatArray[i].x, ClassFloatArray[i].y);
                StructDoubleArray[i].set(ClassFloatArray[i].x, ClassFloatArray[i].y);
            }
        }
        [Benchmark]
        public void Test_ClassFloat() {
            for (int i = 0; i < TestSize; i++) PointDistance(originClass, ClassFloatArray[i]);
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
        public void Test4_StructFloatSHort() {
            for (int i = 0; i < TestSize; i++) PointDistanceShort(originStructFloat, StructFloatArray[i]);
        }
    }
    /// <summary>
    /// Main class, executes benchmark
    /// </summary>
    class Lesson3_Task {
        public static void Main_Benchmark() {
            Lesson3_Core Core = new Lesson3_Core();
            Core.InitArrays();
            BenchmarkSwitcher.FromAssembly(typeof(Lesson3_Core).Assembly).Run();
        }
    }
}
