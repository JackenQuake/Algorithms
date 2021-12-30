using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmsMenuLibrary {
    public class Menu {
        private string[] items;
        private int num, max;
        private int width;
        public Menu(int _max, int _width) {
            max = _max; items = new string[max]; num = 0; width = _width;
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
            if (Selected) { Console.BackgroundColor = ConsoleColor.Cyan; Console.ForegroundColor = ConsoleColor.Black; } else { Console.BackgroundColor = ConsoleColor.Blue; Console.ForegroundColor = ConsoleColor.White; }
            Console.Write($"  {items[i]}  "); for (int j = items[i].Length; j < width; j++) Console.Write(" ");
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
}
