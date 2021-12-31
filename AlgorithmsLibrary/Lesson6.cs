using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;

namespace Algorithms {
    class Lesson6_Utility { // -------------------------------------------------------------------- Some utility functions

        // using some support functions from Lesson2_Utility class

    }
    public struct ScriptCommand {
        public string Class;
        public string Task;
    }
    public class Lesson6_Task_Script {
        static ScriptCommand[] commands;
        public static void Main_DemoScript() {
            var commands = new ScriptCommand[3];
            commands[0].Class = "Lesson1_Task1_Prime";
            commands[0].Task = "Main_Automatic";
            commands[1].Class = "Lesson1_Task3_Fibo";
            commands[1].Task = "Main_NonRecursive";
            commands[2].Class = "Lesson5_Task_BnrSrc";
            commands[2].Task = "Main_DFS";
            StringWriter stringWriter = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(ScriptCommand[]));
            serializer.Serialize(stringWriter, commands);
            string xml = stringWriter.ToString();
            File.WriteAllText("script.xml", xml + "\n");
            Console.WriteLine("Demonstration script created.");
        }
        public static void Main_ScriptEditor() {
            ///
            ///  Under construction
            ///
            string str = File.ReadAllText("Lesson6_Task.md");
            Console.WriteLine(str);
        }
        public static void Main_ExecScript() {
            string xml = File.ReadAllText("script.xml");
            StringReader stringReader = new StringReader(xml);
            XmlSerializer serializer = new XmlSerializer(typeof(ScriptCommand[]));
            commands = (ScriptCommand[])serializer.Deserialize(stringReader);
            for (int i = 0; i < commands.Length; i++) {
                Console.WriteLine($"Executing {commands[i].Class} : {commands[i].Task}");
                Type t = typeof(Lesson6_Task_Script).Assembly.GetType("Algorithms."+commands[i].Class);
                if (t == null) Console.WriteLine($"Class {commands[i].Class} not found...");
                else {
                    MethodInfo m = t.GetMethod(commands[i].Task);
                    if (m == null) Console.WriteLine($"Method {commands[i].Task} not found...");
                    else m.Invoke(t, null);
                }
                Console.WriteLine(); Console.Write("Press any key to continue..."); Console.ReadKey(true); Console.WriteLine(); Console.WriteLine();
            }
        }
    }
    public interface IntCollectionWrapper {
        void Put(int t);
        int Get();
        int Count();
    }
    class IntStackWrapper : IntCollectionWrapper {
        Stack<int> myStack;
        public IntStackWrapper() { myStack = new Stack<int>(); }
        public void Put(int t) { myStack.Push(t); }
        public int Get() { return myStack.Pop(); }
        public int Count() { return myStack.Count(); }
    }
    class IntQueueWrapper : IntCollectionWrapper {
        Queue<int> myQueue;
        public IntQueueWrapper() { myQueue = new Queue<int>(); }
        public void Put(int t) { myQueue.Enqueue(t); }
        public int Get() { return myQueue.Dequeue(); }
        public int Count() { return myQueue.Count(); }
    }
    public class Lesson6_Task_Graph {
        static bool[,] Adjacency_matrix;  // ------------------------------------------------------ Array to store graph.
        static bool[] visited;  // ---------------------------------------------------------------- Vertices are visited
        static int N;
        public static void MakeRandomGraph() {
            N = 10 + Lesson2_Utility.Rnd(10);
            Adjacency_matrix = new bool[N, N];
            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++) Adjacency_matrix[i,j] = (Lesson2_Utility.Rnd(5) == 1);
        }
        public static void BinarySearch(IntCollectionWrapper Data) {  // ---------------------------- Main Search Library
            visited = new bool[N];
            for (int i = 0; i < N; i++) visited[i] = false;
            Data.Put(0); visited[0] = true;
            int NumAccessible = 0;
            while (Data.Count() > 0) {
                int Current = Data.Get();
                Console.WriteLine($"Got to vertex <{Current}>...");
                NumAccessible ++;
                for (int i = 0; i < N; i++)
                    if ((Adjacency_matrix[Current, i]) && (!visited[i])) { Data.Put(i); visited[i] = true; }
            }
            if (NumAccessible == N) Console.WriteLine("All vertices are accessible. Graph is CONNECTED!");
            else {
                Console.Write($"Accessible {NumAccessible} vertices of {N}, inaccesiible:");
                for (int i = 0; i < N; i++) if (!visited[i]) Console.Write(" " + i);
                Console.WriteLine();
            }
        }
        public static void Main_DFS() {
            MakeRandomGraph();
            var Data = new IntStackWrapper();
            BinarySearch(Data);
        }
        public static void Main_BFS() {
            MakeRandomGraph();
            var Data = new IntQueueWrapper();
            BinarySearch(Data);
        }
    }
}
