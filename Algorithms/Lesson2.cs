using System;
//using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Algorithms {
    /// <summary>
    /// support class for Lesson2
    /// </summary>
    class Lesson2_Utility { // -------------------------------------------------------------------- Some utility functions
        private static Random rnd = new Random();
        public static int InputNumber(string command) {
            do {
                Console.Write($"{command} :> ");
                try { return Int32.Parse(Console.ReadLine()); }
                catch { Console.WriteLine("Incorrect input. Please try again."); };
            } while (true);
        }
        public static int Rnd(int v) {
            return rnd.Next(v);
        }
    }
    /// <summary>
    /// Node - class
    /// </summary>
    public class Node {
        public int Value { get; set; }
        public Node NextNode { get; set; }
        public Node PrevNode { get; set; }
    }
    // -------------------------------------------------------------------------------------------- Начальную и конечную ноду нужно хранить в самой реализации интерфейса
    /// <summary>
    /// interface function to realize in class
    /// </summary>
    public interface ILinkedList {
        int GetCount();  // ----------------------------------------------------------------------- возвращает количество элементов в списке
        void AddNode(int value);  // -------------------------------------------------------------- добавляет новый элемент списка
        void AddNodeAfter(Node node, int value);  // ---------------------------------------------- добавляет новый элемент списка после определённого элемента
        void RemoveNode(int index);  // ----------------------------------------------------------- удаляет элемент по порядковому номеру
        void RemoveNode(Node node);  // ----------------------------------------------------------- удаляет указанный элемент
        Node FindNode(int searchValue);  // ------------------------------------------------------- ищет элемент по его значению
    }
    /// <summary>
    /// List-class
    /// * with linked interface
    /// </summary>
    class LinkedList : ILinkedList, IEnumerable {
        private Node first, last;
        private int count;
        public LinkedList() {  // ----------------------------------------------------------------- Constructor
            first = null;
            last = null;
            count = 0;
        }
        public bool IsEmpty() {
            return (count == 0);
        }
        public Node GetFirst() {  // -------------------------------------------------------------- Fet first element
            return first;
        }
        public Node GetLast() {  // --------------------------------------------------------------- Fet last element
            return last;
        }
        public int GetCount() {  // --------------------------------------------------------------- Interface function: Getcount
            //int ListLength = 0;
            // 1 // for (Node current = first; current != null; current = current.NextNode) ListLength++;
            // 2 // foreach (Node current in this) ListLength++;
            //return ListLength;
            return count;
        }
        public void AddNode(int value) {  // ------------------------------------------------------ Interface function: AddNode
            AddNodeAfter(last, value);
        }
        public void AddNodeAfter(Node node, int value) {  // -------------------------------------- Interface function: AddNodeAfter
            Node insert = new Node();
            insert.Value = value;
            insert.PrevNode = node;
            if (node != null) {
                insert.NextNode = node.NextNode;
                if (node.NextNode != null) node.NextNode.PrevNode = insert;
                node.NextNode = insert;
            } else {
                insert.NextNode = first;
                if (first != null) first.PrevNode = insert;
                first = insert;
            }
            if (node == last) last = insert;
            count++;
        }
        public Node GetElement(int index) {  // --------------------------------------------------- Support function GetElement by index
            if (index < 0) return null;
            //for (Node current = first; current != null; current = current.NextNode) if (index > 0) index--; else return current;
            foreach (Node current in this) if (index > 0) index--; else return current;
            return null;
        }
        public void RemoveNode(int index) {  // --------------------------------------------------- Interface function: RemoveNode(by index)
            RemoveNode(GetElement(index));
        }
        public void RemoveNode(Node node) {  // --------------------------------------------------- Interface function: RemoveNode(by Node)
            if (node == null) return;
            if (node.PrevNode != null) node.PrevNode.NextNode = node.NextNode; else first = node.NextNode;
            if (node.NextNode != null) node.NextNode.PrevNode = node.PrevNode; else last = node.PrevNode;
            //if (node == first) first = node.NextNode;
            //if (node == last) last = node.PrevNode;
            count--;
        }
        public Node FindNode(int searchValue) {  // ----------------------------------------------- Interface function: FindNode
            //for (Node current = first; current != null; current.NextNode = current) if (current.Value == searchValue) return current;
            foreach (Node current in this) if (current.Value == searchValue) return current;
            return null;
        }
        private Node SortListSegment(Node start, int size) {  // ---------------------------------- "Merge sort"
            if (size == 1) return start;
            // Find approximate center of the chain
            Node Half1, Half2;
            Half2 = start; for (int i = 0; i < size / 2; i++) Half2 = Half2.NextNode;
            // Disconnect list at center
            Half2.PrevNode.NextNode = null;
            Half2.PrevNode = null;
            // Sort halves
            Half1 = SortListSegment(start, size / 2);
            Half2 = SortListSegment(Half2, size - size / 2);
            // Merge halves
            first = null; last = null;
            while (true) {
                if (Half1 == null) {
                    if (Half2 == null) break;
                    else start = Half2;
                }
                else {
                    if (Half2 == null) start = Half1;
                    else start = (Half1.Value < Half2.Value) ? Half1 : Half2;
                }
                if (start == Half1) Half1 = Half1.NextNode; else Half2 = Half2.NextNode;
                if (first == null) first = start; else last.NextNode = start;
                start.PrevNode = last; last = start;
            }
            last.NextNode = null; return first;
        }
        public void SortList() { // --------------------------------------------------------------- Sort List
            if (GetCount() < 2) return;
            SortListSegment(first, GetCount());
        }
        public IEnumerator GetEnumerator() {  // -------------------------------------------------- Makes 'foreach' possible
            for (Node current = first; current != null; current = current.NextNode) yield return current;
        }
    }
    /// <summary>
    /// Test our List
    /// </summary>
    class Lesson2_Task1_LnkLst { // --------------------------------------------------------------- Dual-linked list
        private static LinkedList myList;
        private static ILinkedList myIList;  // --------------------------------------------------- Interface linked
        private static bool IsListEmpty() {
            return myList.IsEmpty();
        }
        private static void PrintList() {
            if (IsListEmpty()) Console.Write("List is empty.");
                //else for (Node print = myList.GetFirst(); print != null; print = print.NextNode) Console.Write($"{print.Value} ");
                else foreach (Node print in myList) Console.Write($"{print.Value} ");
            Console.WriteLine();
        }
        private static void FillListRandomly() {
            ClearList(false);
            int r = Lesson2_Utility.Rnd(99) + 1;
            for (int i = 0; i < r; i++) myIList.AddNode(Lesson2_Utility.Rnd(900) + 100);
            PrintList();
        }
        private static void CheckListLength() {
            Console.WriteLine($"List length: {myIList.GetCount()}");
        }
        private static void AddNewElement() {
            myIList.AddNode(Lesson2_Utility.InputNumber("Please enter element to add to the List"));
        }
        private static void AddNewElementAfter() {
            PrintList();
            if (IsListEmpty()) return;
            /*
            int indexToInsert = 0;
            do {
                indexToInsert = Lesson2_Utility.InputNumber("Please choose number of element to insert after this");
                if (indexToInsert < 0) Console.WriteLine($"Input error : {indexToInsert}. Number must be more than zero. Try again...");
                if (indexToInsert > myList.GetCount()) Console.WriteLine($"Input error: {indexToInsert}. Number must be less than List size. Try again...");
            } while ((indexToInsert < 0) & (indexToInsert > myList.GetCount()));
            Node insert = myList.GetElement(indexToInsert);
            */
            Node insert;
            do {
                insert = myList.GetElement(Lesson2_Utility.InputNumber("Please choose number of element to insert after"));
                if (insert != null) break;
                Console.WriteLine("Please enter the number of element that exists.");
            } while (true);
            myIList.AddNodeAfter(insert, Lesson2_Utility.InputNumber("Please enter element to add to the List"));
        }
        private static void DeleteElement() {
            PrintList();
            if (IsListEmpty()) return;
            myIList.RemoveNode(Lesson2_Utility.InputNumber("Please enter number element to delete"));
        }
        private static void FindElement() {
            PrintList();
            if (IsListEmpty()) return;
            Node element = myList.GetElement(Lesson2_Utility.InputNumber("Please enter number of element to find"));
            if (element == null) Console.WriteLine("There is not such element.");
                else Console.WriteLine(element.Value); 
        }
        private static void SortList() {
            Console.WriteLine("List before sort:");
            PrintList();
            myList.SortList();
            Console.WriteLine("List after sort:");
            PrintList();
        }
        private static void SortListArray() {  // ------------------------------------------------- Silly sort throught array ;)
            if (IsListEmpty()) { Console.WriteLine("There is nothing to sort. List is empty."); return; }
            Console.WriteLine("List before sort:");
            PrintList();
            int[] myArray = new int[myIList.GetCount()];
            int index = 0;
            foreach (Node sort in myList) { myArray[index] = sort.Value; index++; }
            Array.Sort(myArray, 0, myArray.Length);
            ClearList(false);
            for (int i = 0; i < myArray.Length; i++) myIList.AddNode(myArray[i]);
            Console.WriteLine("List after sort:");
            PrintList();
        }
        private static void ClearList(bool flag) {
            //for (Node print = myList.GetFirst(); print != null; print = print.NextNode) myIList.RemoveNode(print);
            foreach (Node print in myList) myIList.RemoveNode(print);
            if (flag) Console.WriteLine("List is cleared...");
        }
        public static void Main_TestList() {
            myList = new LinkedList();
            myIList = (ILinkedList)myList;
            Menu menu = new Menu(10,30);
            menu.Reset();
            menu.Append("1. Print the List");
            menu.Append("2. Fill the List randomly");
            menu.Append("3. Check list length");
            menu.Append("4. Add new element");
            menu.Append("5. Add new element after given");
            menu.Append("6. Delete element by index");
            menu.Append("7. Find element by index");
            menu.Append("8. Sort the List");
            menu.Append("9. Clear the List");
            menu.Append("Exit");
            do {
                Console.Clear(); 
                Console.WriteLine("       Test Dual-Linked List:      ");
                string str = menu.ShowMenu(1, 1);
                switch (str[0]) {
                    case '1': PrintList(); break;
                    case '2': FillListRandomly(); break;
                    case '3': CheckListLength(); break;
                    case '4': AddNewElement(); break;
                    case '5': AddNewElementAfter(); break;
                    case '6': DeleteElement(); break;
                    case '7': FindElement(); break;
                    case '8': SortList();  break;
                    case '9': ClearList(true); break;
                    case 'E': Environment.Exit(0); break; //return;
                }
                Console.Write("Press any key to return..."); Console.ReadKey(true);
            } while (true);
        }
    }
    class Lesson2_Task2_BnrSrc { // --------------------------------------------------------------- Binary search
        private static int[] FillArrayRandomly(int[] array) {
            for (int i=0; i < array.Length; i++) array[i] = Lesson2_Utility.Rnd(900)+100;
            return array;
        }
        private static int[] SortArray(int[] array) {
            Array.Sort(array);
            return array;
        }
        private static int BinarySearchRecursive(int[] array, int value, int first, int last) { //  Recursive binary search
            if (first > last) { return -1; }
            var middle = (first + last) / 2;
            var middleValue = array[middle];
            if (middleValue == value) { return middle; }
            else if (middleValue > value) return BinarySearchRecursive(array, value, first, middle - 1);
                else return BinarySearchRecursive(array, value, middle + 1, last);
        }
        private static int BinarySearchIterative(int[] array, int value, int left, int right) {  // Iterative binary search
            while (left <= right) {
                var middle = (left + right) / 2;
                if (value == array[middle]) return middle;
                    else if (value < array[middle]) right = middle - 1;
                            else left = middle + 1;
            }
            return -1;
        }
        private static int BinarySearchNormal(int[] array, int value) { // ------------------------ Normal binary search
            int a = 0;
            int b = array.Length;
            int c;
            while ((b - a) > 1) {
                c = (a + b) / 2;
                if (array[c] == value) return c;
                if (array[c] <= value) a = c; else b = c;
            }
            return (array[a] == value ? a : -1);
        }
        public static void Main_Recursive() {  // ------------------------------------------------- Recursive algorithm of sort
            int lng = Lesson2_Utility.Rnd(99) + 1;
            int[] myArray = new int[lng];
            myArray = FillArrayRandomly(myArray);
            myArray = SortArray(myArray);
            Console.WriteLine("Your array:");
            foreach (int i in myArray) Console.Write(i + " ");
            Console.WriteLine();
            int src = Lesson2_Utility.InputNumber("Please enter number to search");
            Console.WriteLine($"Number of element {src} is {BinarySearchRecursive(myArray, src, 0, myArray.Length-1)}");
        }
        public static void Main_Iterative() {  // ------------------------------------------------- Iterative algorithm of sort
            int lng = Lesson2_Utility.Rnd(99) + 1;
            int[] myArray = new int[lng];
            myArray = FillArrayRandomly(myArray);
            myArray = SortArray(myArray);
            Console.WriteLine("Your array:");
            foreach (int i in myArray) Console.Write(i + " ");
            Console.WriteLine();
            int src = Lesson2_Utility.InputNumber("Please enter number to search");
            Console.WriteLine($"Number of element {src} is {BinarySearchIterative(myArray, src, 0, myArray.Length-1)}");
        }
        public static void Main_Normal() {  // ---------------------------------------------------- My algorithm of binary sort
            int lng = Lesson2_Utility.Rnd(99)+1;
            int[] myArray = new int[lng];
            myArray = FillArrayRandomly(myArray);
            myArray = SortArray(myArray);
            Console.WriteLine("Your array:");
            foreach (int i in myArray) Console.Write(i + " ");
            Console.WriteLine();
            int src = Lesson2_Utility.InputNumber("Please enter number to search");
            Console.WriteLine($"Number of element {src} is {BinarySearchNormal(myArray, src)}");
        }
        public static void Main_Complexity() {  // ------------------------------------------------ Printing text file with unswer
            string str = File.ReadAllText("Lesson2_Task2.md");
            Console.WriteLine(str);
        }
    }
}
