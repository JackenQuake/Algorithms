using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms {
    /// <summary>
    /// At first i wrote this! But then I understood there is en elegant way to do it without
    /// duplicating code, if we make wrappers for Stack and Queue with one interface.
    /// 
    ///                      VERSION 1. (below)
    ///
    /// </summary>
    /*
    public class Lesson5_Task_BnrSrc {
        private static BinaryTree myTree = null;
        private static ITree myITree;  // --------------------------------------------------------- Interface linked
        public static void InitTree() {
            if (myTree == null) { myTree = new BinaryTree(); myITree = (ITree)myTree; }
            int r = Lesson2_Utility.Rnd(10) + 10;
            for (int i = 0; i < r; i++) myITree.AddItem(Lesson2_Utility.Rnd(900) + 100);
            Console.Clear();
            myITree.PrintTree();
            Console.WriteLine();
        }
        public static void Main_DFS() {
            InitTree();
            int value = Lesson2_Utility.InputNumber("Please enter element to search");
            Console.WriteLine();
            TreeNode Gotcha = null;
            Stack<TreeNode> myStack = new Stack<TreeNode>();
            myStack.Push(myITree.GetRoot());
            while (myStack.Count > 0) {
                TreeNode Current = myStack.Pop();
                Console.Write($"Scanning <{Current.Value}>...");
                if (Current.Value == value) {
                    Gotcha = Current;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($" ...Gotcha <{Gotcha.Value}>");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
                if (Current.LeftChild != null) myStack.Push(Current.LeftChild);
                if (Current.RightChild != null) myStack.Push(Current.RightChild);
            }
            if (Gotcha == null) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"<{value}> not found.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            } else {
                Console.WriteLine();
                Console.Write("Press any key to view tree with found element..."); Console.ReadKey(true);
                Console.Clear();
                myTree.PrintTreeHighlight(Gotcha);
            }
        }
        public static void Main_BFS() {
            InitTree();
            int value = Lesson2_Utility.InputNumber("Please enter element to search");
            Console.WriteLine();
            TreeNode Gotcha = null;
            Queue <TreeNode> myQueue = new Queue<TreeNode>();
            myQueue.Enqueue(myITree.GetRoot());
            while (myQueue.Count > 0) {
                TreeNode Current = myQueue.Dequeue();
                Console.Write($"Scanning <{Current.Value}>...");
                if (Current.Value == value) {
                    Gotcha = Current;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($" ...Gotcha <{Gotcha.Value}>");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
                if (Current.LeftChild != null) myQueue.Enqueue(Current.LeftChild);
                if (Current.RightChild != null) myQueue.Enqueue(Current.RightChild);
            }
            if (Gotcha == null) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"<{value}> not found.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            } else {
                Console.WriteLine();
                Console.Write("Press any key to view tree with found element..."); Console.ReadKey(true);
                Console.Clear();
                myTree.PrintTreeHighlight(Gotcha);
            }
        }
    }
    */
    /// <summary>
    /// 
    ///                      VERSION 2. (below)
    ///
    /// </summary>
    public interface ICollectionWrapper {
        void Put(TreeNode t);
        TreeNode Get();
        int Count();
    }
    class StackWrapper : ICollectionWrapper {
        Stack<TreeNode> myStack ;
        public StackWrapper() { myStack = new Stack<TreeNode>(); }
        public void Put(TreeNode t) { myStack.Push(t); }
        public TreeNode Get() { return myStack.Pop(); }
        public int Count() { return myStack.Count(); }
    }
    class QueueWrapper : ICollectionWrapper {
        Queue<TreeNode> myQueue;
        public QueueWrapper() { myQueue = new Queue<TreeNode>(); }
        public void Put(TreeNode t) { myQueue.Enqueue(t); }
        public TreeNode Get() { return myQueue.Dequeue(); }
        public int Count() { return myQueue.Count(); }
    }
    public class Lesson5_Task_BnrSrc {
        private static BinaryTree myTree = null;
        private static ITree myITree;  // --------------------------------------------------------- Interface linked
        public static void InitTree() {
            if (myTree == null) { myTree = new BinaryTree(); myITree = (ITree)myTree; }
            int r = Lesson2_Utility.Rnd(10) + 10;
            for (int i = 0; i < r; i++) myITree.AddItem(Lesson2_Utility.Rnd(900) + 100);
            Console.Clear();
            myITree.PrintTree();
            Console.WriteLine();
        }
        public static void BinarySearch(ICollectionWrapper Data) {  // --------------------------- Main Search Library
            InitTree();
            int value = Lesson2_Utility.InputNumber("Please enter element to search");
            Console.WriteLine();
            TreeNode Gotcha = null;
            Data.Put(myITree.GetRoot());
            while (Data.Count() > 0) {
                TreeNode Current = Data.Get();
                Console.Write($"Scanning <{Current.Value}>...");
                if (Current.Value == value) {
                    Gotcha = Current;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($" ...Gotcha <{Gotcha.Value}>");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
                if (Current.LeftChild != null) Data.Put(Current.LeftChild);
                if (Current.RightChild != null) Data.Put(Current.RightChild);
            }
            if (Gotcha == null) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"<{value}> not found.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
            } else {
                Console.WriteLine();
                Console.Write("Press any key to view tree with found element..."); Console.ReadKey(true);
                Console.Clear();
                myTree.PrintTreeHighlight(Gotcha);
            }
        }
        public static void Main_DFS() {
            var Data = new StackWrapper();
            BinarySearch(Data);
        }
        public static void Main_BFS() {
            var Data = new QueueWrapper();
            BinarySearch(Data);
        }
    }
}

