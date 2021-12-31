using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using AlgorithmsMenuLibrary;
using System.IO;

namespace Algorithms {
    class Program {
        static void Main(string[] args) {
            var DLL = Assembly.LoadFile(Directory.GetCurrentDirectory()+@"\AlgorithmsLibrary.dll");
            var menu = new Menu(20, 12);
            string str, lesson, task, method;
            int a, b;
            menu.Reset();
            foreach (Type t in DLL.GetExportedTypes()) {
                str = t.FullName;
                a = str.IndexOf("Lesson"); b = str.IndexOf("_Task", a + 1);
                if ((a >= 0) && (b > a)) menu.Append(str.Substring(a, b - a));
            }
            menu.Append("Exit");
            lesson = menu.ShowMenu(1, 1); if (lesson == "Exit") return;
            lesson = lesson + "_Task";
            menu.Reset();
            foreach (Type t in DLL.GetExportedTypes()) {
                str = t.FullName; a = str.IndexOf(lesson);
                if (a >= 0) menu.Append(str.Substring(a + lesson.Length - 4));
            }
            menu.Append("Exit");
            task = menu.ShowMenu(20, 1); if (task == "Exit") return;
            task = task.Substring(4);
            Type taskClass = null;
            foreach (Type t in DLL.GetExportedTypes()) {
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

