using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Algorithms {
    /// <summary>
    /// support class for Lesson4
    /// </summary>
    class Lesson4_Utility { // -------------------------------------------------------------------- Some utility functions

        // using some support functions from Lesson2_Utility class

    }
    /// <summary>
    /// ------------------------------------------------------------------------------------------- TASK 1
    /// </summary>
    public class TreeNode {
        public int Value { get; set; }
        public TreeNode LeftChild { get; set; }
        public TreeNode RightChild { get; set; }
        public override bool Equals(object obj) {
            var node = obj as TreeNode;
            if (node == null)
                return false;
            return node.Value == Value;
        }
    }
    public interface ITree {
        TreeNode GetRoot();
        void AddItem(int value); // добавить узел
        void RemoveItem(int value); // удалить узел по значению
        TreeNode GetNodeByValue(int value); //получить узел дерева по значению
        void PrintTree(); //вывести дерево в консоль
    }
    public static class TreeHelper {
        public static NodeInfo[] GetTreeInLine(ITree tree) {
            var bufer = new Queue<NodeInfo>();
            var returnArray = new List<NodeInfo>();
            var root = new NodeInfo() { Node = tree.GetRoot() };
            bufer.Enqueue(root);
            while (bufer.Count != 0) {
                var element = bufer.Dequeue();
                returnArray.Add(element);
                var depth = element.Depth + 1;
                if (element.Node.LeftChild != null) {
                    var left = new NodeInfo() {
                        Node = element.Node.LeftChild,
                        Depth = depth,
                    };
                    bufer.Enqueue(left);
                }
                if (element.Node.RightChild != null) {
                    var right = new NodeInfo() {
                        Node = element.Node.RightChild,
                        Depth = depth,
                    };
                    bufer.Enqueue(right);
                }
            }
            return returnArray.ToArray();
        }
    }
    public class NodeInfo {
        public int Depth { get; set; }
        public TreeNode Node { get; set; }
    }

    class BinaryTree : ITree {
        private TreeNode Root;
        public BinaryTree() {  // ----------------------------------------------------------------- Constructor
            Root = null;
        }
        public TreeNode GetRoot() {
            return Root;
        }
        public void AddItem(int value) {  
            TreeNode NewElement = new TreeNode();
            NewElement.Value = value;
            NewElement.LeftChild = null;
            NewElement.RightChild = null;
            if (Root == null) { Root = NewElement; return; }
            for (TreeNode Current = Root; true;) {
                if (Current.Equals(NewElement)) return;
                if (value < Current.Value) {
                    if (Current.LeftChild == null) { Current.LeftChild = NewElement; return; }
                    Current = Current.LeftChild;
                } else {
                    if (Current.RightChild == null) { Current.RightChild = NewElement; return; }
                    Current = Current.RightChild;
                }
            }
        }
        public void RemoveItem(int value) {
            TreeNode Current, Parent = null, Replacement, ReplacementParent;
            // First we search tree for the requested item, also storing its parent to properly update links
            for (Current = Root; Current != null;) {
                if (Current.Value == value) break;
                Parent = Current;
                if (value < Current.Value) Current = Current.LeftChild;
                else Current = Current.RightChild;
            }
            // If the element was not found - leave
            if (Current == null) return;
            // Otherwise we have to find another element to put in place of removed one
            // If the element did not have one of the child subtrees, we just replace it with existing subtree
            // (this is also valid, if element had no children - then it will be replaced with (null)
            if (Current.LeftChild == null) Replacement = Current.RightChild;
            else if (Current.RightChild == null) Replacement = Current.LeftChild;
            // If the element had both subtrees, we replace it with largest (rightmost) element of its left subtree
            else {
                Replacement = Current.LeftChild;
                ReplacementParent = null;
                // Search the left subtree for its rightmost element (that is, element having no right child)
                // also remembering its parent
                while (Replacement.RightChild != null) {
                    ReplacementParent = Replacement;
                    Replacement = Replacement.RightChild;
                }
                // Unlink replacement element from its old position
                // (as it had no right child itselt - by defition - we just put its left child in its place
                if (ReplacementParent != null) {
                    ReplacementParent.RightChild = Replacement.LeftChild;
                    Replacement.LeftChild = Current.LeftChild;
                }
                // Replacement becomes new parent of the current child
                Replacement.RightChild = Current.RightChild;
            }
            // And finally we put replacement into the tree instead of removed element
            if (Parent == null) Root = Replacement;
            else if (Parent.LeftChild == Current) Parent.LeftChild = Replacement;
            else Parent.RightChild = Replacement;
        }
        public TreeNode GetNodeByValue(int value) {
            for (TreeNode Current = Root; Current != null;) {
                if (Current.Value == value) return Current;
                if (value < Current.Value) Current = Current.LeftChild;
                else Current = Current.RightChild;
            }
            return null;
        }
        // ----------------------------------------------------------------------------------- Printing Tree
        // To print the tree, we first make a list of list of nodes per-layer
        private class TreeLayerNode {
            public TreeNode Content;
            public int Width, TotalWidth, Offset, ParentOffset, LeftLinkWidth, RightLinkWidth;
            public TreeLayerNode Next, Parent;
            public static int CurrPrintPos;
            public bool Right;
            public TreeLayerNode(TreeNode _Content) {
                Content = _Content; Next = null; Parent = null;
                Width = 3; for (int x = Content.Value; x > 9; x /= 10) Width++; TotalWidth = Width;
                Offset = 0; ParentOffset = 0; LeftLinkWidth = 0; RightLinkWidth = 0;
            }
            public void Print(bool Highlight) {
                if (LeftLinkWidth > 0) {
                    for (; CurrPrintPos < Offset - LeftLinkWidth - 1; CurrPrintPos++) Console.Write(" ");
                    Console.Write("\u250C"); CurrPrintPos++;
                    for (; CurrPrintPos < Offset; CurrPrintPos++) Console.Write("\u2500");
                }
                else for (; CurrPrintPos < Offset; CurrPrintPos++) Console.Write(" ");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = Highlight ? ConsoleColor.Red : ConsoleColor.Gray;
                Console.Write($" {Content.Value} "); CurrPrintPos += Width;
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                if (RightLinkWidth > 0) {
                    for (; CurrPrintPos < Offset + Width + RightLinkWidth; CurrPrintPos++) Console.Write("\u2500");
                    Console.Write("\u2510"); CurrPrintPos++;
                }
            }
            public void PrintUplink() {
                ParentOffset += Parent.ParentOffset; Offset += ParentOffset;
                for (; CurrPrintPos < Offset + (Right ? 0 : Width - 1); CurrPrintPos++) Console.Write(" ");
                Console.Write("\u2502"); CurrPrintPos++;
            }
        }
        private class TreeLayer {
            public TreeLayerNode First, Last;
            public TreeLayer Next;
            public TreeLayer() { First = null; Last = null; Next = null; }
            public void AddNode(TreeNode Content) {
                if (Last == null) { First = new TreeLayerNode(Content); Last = First; }
                else { Last.Next = new TreeLayerNode(Content); Last = Last.Next; }
            }
        }
        private void CreateLayers(TreeLayer Layer, TreeNode Node) {
            Layer.AddNode(Node);
            TreeLayerNode LeftChild = null, RightChild = null, Current = Layer.Last;
            if (Node.LeftChild != null) {
                if (Layer.Next == null) Layer.Next = new TreeLayer();
                CreateLayers(Layer.Next, Node.LeftChild);
                LeftChild = Layer.Next.Last; LeftChild.Parent = Current;
                Current.LeftLinkWidth = 1; LeftChild.Right = false;
                Current.Offset = LeftChild.Offset + LeftChild.Width + 1;
                Current.TotalWidth = Math.Max(Current.Offset + Current.Width, LeftChild.TotalWidth);
            }
            if (Node.RightChild == null) return;
            if (Layer.Next == null) Layer.Next = new TreeLayer();
            CreateLayers(Layer.Next, Node.RightChild);
            RightChild = Layer.Next.Last; RightChild.Parent = Current;
            Current.RightLinkWidth = 1; RightChild.Right = true; 
            if (LeftChild != null) RightChild.ParentOffset = LeftChild.TotalWidth + 2;
            int d = (RightChild.ParentOffset + RightChild.Offset) - (Current.Offset + Current.Width + 1);
            if (d > 0) {
                if (LeftChild != null) { Current.Offset += d / 2; Current.LeftLinkWidth += d / 2; d -= d / 2; }
                Current.RightLinkWidth += d; 
            } else RightChild.ParentOffset -= d;
            Current.TotalWidth = RightChild.ParentOffset + RightChild.TotalWidth;
        }
        public void PrintTreeHighlight(TreeNode Highlight) {
            if (Root == null) return;
            TreeLayer FirstLayer = new TreeLayer();
            CreateLayers(FirstLayer, Root);
            while (true) {
                TreeLayerNode.CurrPrintPos = -1;
                for (TreeLayerNode Current = FirstLayer.First; Current != null; Current = Current.Next) Current.Print(Highlight == Current.Content);
                Console.WriteLine();
                FirstLayer = FirstLayer.Next; if (FirstLayer == null) break;
                TreeLayerNode.CurrPrintPos = -1;
                for (TreeLayerNode Current = FirstLayer.First; Current != null; Current = Current.Next) Current.PrintUplink();
                Console.WriteLine();
            }
        }
        public void PrintTree() {
            PrintTreeHighlight(null);
        }
        public void ClearTree() {
            Root = null;
        }
    }
    /// <summary>
    /// ------------------------------------------------------------------------------------------- TASK 2
    /// </summary>
    class Lesson4_Task1_Tree {
        private static BinaryTree myTree;
        private static ITree myITree;  // --------------------------------------------------------- Interface linked
        private static void PrintTree() {
            Console.Clear();
            myITree.PrintTree();
        }
        private static void FillTreeRandomly() {
            ClearTree(false);
            //int r = Lesson2_Utility.Rnd(99) + 1;
            int r = Lesson2_Utility.Rnd(20) + 1;
            for (int i = 0; i < r; i++) myITree.AddItem(Lesson2_Utility.Rnd(900) + 100);
            PrintTree();
        }
        private static void AddNewElement() {
            myITree.AddItem(Lesson2_Utility.InputNumber("Please enter element to add to the Tree"));
        }
        private static void DeleteElement() {
            PrintTree();
            myITree.RemoveItem(Lesson2_Utility.InputNumber("Please enter number element to delete"));
            PrintTree();
        }
        private static void FindElement() {
            PrintTree();
            TreeNode element = myITree.GetNodeByValue(Lesson2_Utility.InputNumber("Please enter number to find in the tree"));
            if (element == null) Console.WriteLine("There is not such element.");
            else { Console.Clear(); myTree.PrintTreeHighlight(element); }
        }
        private static void ClearTree(bool flag) {
            myTree.ClearTree();
            if (flag) Console.WriteLine("Tree is cleared...");
        }
        public static void Main_TestTree() {
            myTree = new BinaryTree();
            myITree = (ITree)myTree;
            Menu menu = new Menu(10, 30);
            menu.Reset();
            menu.Append("1. Print the Tree");
            menu.Append("2. Fill the Tree randomly");
            menu.Append("3. Add new element");
            menu.Append("4. Delete element by value");
            menu.Append("5. Find element by value");
            menu.Append("6. Clear the Tree");
            menu.Append("Exit");
            do {
                Console.Clear();
                Console.WriteLine("           Binary  Tree:           ");
                string str = menu.ShowMenu(1, 1);
                switch (str[0]) {
                    case '1': PrintTree(); break;
                    case '2': FillTreeRandomly(); break;
                    case '3': AddNewElement(); break;
                    case '4': DeleteElement(); break;
                    case '5': FindElement(); break;
                    case '6': ClearTree(true); break;
                    case 'E': Environment.Exit(0); break; //return;
                }
                Console.WriteLine();
                Console.Write("Press any key to return..."); Console.ReadKey(true);
            } while (true);
        }
    }
    public class Lesson4_CoreBench {
        public int Amount;
        public string[] StringArray;
        public HashSet<string> Hash;
        public string str;
        public Lesson4_CoreBench() {
            Amount = 10000;  // ------------------------------------------------------------------- Ammount of strings in array
            StringArray = new string[Amount];
            Hash = new HashSet<string>();
            for (int i = 0; i < Amount; i++) {
                StringArray[i] = RandomString(Lesson2_Utility.Rnd(40) + 10);
                Hash.Add(StringArray[i]);
            }
            str = StringArray[Lesson2_Utility.Rnd(Amount)];  // ----------------------------------- Choose one random string in array to compare in Bench's methods
        }
        private string RandomString(int length) {
            char ch;
            string str = "";
            for (int i = 0; i < length; i++) {
                ch = (char)(Lesson2_Utility.Rnd(26) + 97);
                str += ch;
            }
            return str;
        }
        [Benchmark]
        public bool SearchInStringArray() {
            foreach (string s in StringArray) if (s == str) return true;
            return false;
        }
        [Benchmark]
        public bool SearchInHash() {
            return Hash.Contains(str);
        }
    }
    public class Lesson4_Task2_Hash {
        public static void Main_Benchmark() {
            Lesson4_CoreBench Core = new Lesson4_CoreBench();
            //Core.Initialize();
            string[] args = new string[2];
            args[0] = "-f";
            args[1] = "*Lesson4_CoreBench*";
            BenchmarkSwitcher.FromAssembly(typeof(Lesson4_CoreBench).Assembly).Run(args);
        }
    }
}
