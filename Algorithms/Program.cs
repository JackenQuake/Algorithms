using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Algorithms {
    class Menu {
        private string[] items;
        private int num, max;
        public Menu(int _max) {
            max = _max; items = new string[max]; num = 0;
        }
        public void Reset() {
            num = 0;
        }
        public void Append(string str) {
            if (num == max) return;
            for (int i = 0; i < num; i++) if (str == items[i]) return;
            items[num++] = str;
        }
        private void ShowItem(int x, int y, int i, bool Selected) {
            Console.SetCursorPosition(x, y + i);
            if (Selected) { Console.BackgroundColor = ConsoleColor.Cyan; Console.ForegroundColor = ConsoleColor.Black; }
            else { Console.BackgroundColor = ConsoleColor.Blue; Console.ForegroundColor = ConsoleColor.White; }
            Console.Write($"  {items[i],-12}  ");
        }
        public string ShowMenu(int x, int y) {
            int i;
            Console.CursorVisible = false;
            Array.Sort(items, 0, num - 1);
            for (i = 0; i < num; i++) ShowItem(x, y, i, (i == 0));
            for (i = 0; true;) {
                switch (Console.ReadKey(true).Key) {
                    case ConsoleKey.UpArrow:
                        if (i == 0) break;
                        ShowItem(x, y, i, false); i--; ShowItem(x, y, i, true); break;
                    case ConsoleKey.DownArrow:
                        if (i == num - 1) break;
                        ShowItem(x, y, i, false); i++; ShowItem(x, y, i, true); break;
                    case ConsoleKey.Enter:
                        Console.CursorVisible = true; Console.SetCursorPosition(0, y + num + 2);
                        Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.White;
                        return items[i];
                }
            }

        }
    }
    class Program {
        static void Main(string[] args) {
            var menu = new Menu(20);
            string str, lesson, task, method;
            int a, b;
            menu.Reset();
            foreach (Type t in typeof(Program).Assembly.GetTypes()) {
                str = t.FullName;
                a = str.IndexOf("Lesson"); b = str.IndexOf("_Task", a + 1);
                if ((a >= 0) && (b > a)) menu.Append(str.Substring(a, b - a));
            }
            menu.Append("Exit");
            lesson = menu.ShowMenu(1, 1); if (lesson == "Exit") return;
            lesson = lesson + "_Task";
            menu.Reset();
            foreach (Type t in typeof(Program).Assembly.GetTypes()) {
                str = t.FullName; a = str.IndexOf(lesson);
                if (a >= 0) menu.Append(str.Substring(a + lesson.Length - 4));
            }
            menu.Append("Exit");
            task = menu.ShowMenu(20, 1); if (task == "Exit") return;
            task = task.Substring(4);
            Type taskClass = null;
            foreach (Type t in typeof(Program).Assembly.GetTypes()) {
                str = t.FullName; a = str.IndexOf(lesson);
                if (a >= 0) if (str.Substring(a) == lesson + task) taskClass = t;
            }
            if (taskClass == null) Console.WriteLine("Strange: class not found...");
            else {
                menu.Reset();
                foreach (MethodInfo m in taskClass.GetMethods())
                    if (m.Name.Substring(0, 5) == "Main_") menu.Append(m.Name.Substring(5));
                menu.Append("Exit");
                method = menu.ShowMenu(39, 1); if (method == "Exit") return;
                Console.Clear();
                taskClass.GetMethod("Main_" + method).Invoke(taskClass, null);
            }
            Console.WriteLine(); Console.Write("Press any key to exit..."); Console.ReadKey(true);
        }
    }
}
