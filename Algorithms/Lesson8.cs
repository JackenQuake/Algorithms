using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Algorithms {
    class Lesson8_Utility {  // ------------------------------------------------------------------- Some utility functions

        // using some support functions from Lesson1_Utility & Lesson2.Utility class

    }
    class Lesson8_Task1_Bcksrt {  // -------------------------------------------------------------- Bucketsort
        const int NumBuckets = 5;
        private static void PrintArray(int[] array) {
            for (int i = 0; i < array.Length; i++) Console.Write($"{array[i], 4}");
            Console.WriteLine();
        }
        public static void BucketSort(int[] array) {
            int[] bucketCount = new int[NumBuckets];
            int min = array[0], max = array[0], bucketSize, i, k, j;
            // find MIN and MAX of the array
            for (i = 1; i < array.Length; i++) {
                if (array[i] < min) min = array[i];
                if (array[i] > max) max = array[i];
            }
            bucketSize = ((max - min) / NumBuckets) + 1;
            for (i = 0; i < NumBuckets; i++) bucketCount[i] = 0;
            for (i = 0; i < array.Length; i++) {
                k = (array[i] - min) / bucketSize;
                bucketCount[k]++;
            }
            int[][] Buckets = new int[NumBuckets][];
            for (i = 0; i < NumBuckets; i++) {
                Buckets[i] = new int[bucketCount[i]];
                bucketCount[i] = 0;
            }
            for (i = 0; i < array.Length; i++) {
                k = (array[i] - min) / bucketSize;
                Buckets[k][bucketCount[k]] = array[i];
                bucketCount[k]++;
            }
            for (i = 0; i < NumBuckets; i++) Array.Sort(Buckets[i]);
            k = 0;
            for (i = 0; i < NumBuckets; i++)
                for (j = 0; j < bucketCount[i]; j++) array[k++] = Buckets[i][j];
        }
        public static void Main_Start() {
            int N = Lesson2_Utility.Rnd(990) + 10;
            int[] array = new int[N];
            for (int i = 0; i < N; i++) array[i] = Lesson2_Utility.Rnd(100);
            Console.WriteLine("Non-sorted array:");
            PrintArray(array);
            BucketSort(array);
            Console.WriteLine("Sorted array:");
            PrintArray(array);
        }
    }
    class BlockInfo {  // ------------------------------------------------------------------------- Block descriptor for external sort
        public long offset;
        public int size;
        public int value;
        public void GetValue(BinaryReader File) {
            File.BaseStream.Seek(offset * 4, SeekOrigin.Begin);
            value = File.ReadInt32();
            offset++;
        }
    }
    class Lesson8_Task2_ExtSrt {  // ------------------------------------------------------------- External sort
        public static void Main_DemoFile() {
            Console.WriteLine("Enter file size:");
            long FileSize = Lesson1_Utility.InputPositiveNumber();
            BinaryWriter writer = new BinaryWriter(File.Open("demo.bin", FileMode.Create));
            for (int i = 0; i < FileSize; i++) writer.Write(Lesson2_Utility.Rnd(Int32.MaxValue));
            writer.Dispose();
        }
        public static void Main_Sort() {
            int BlockSize, BlockAmount;
            string FileName;
            Console.Write("Enter file name (default is 'Demo.bin'):> "); FileName = Console.ReadLine();
            if (FileName == "") FileName = "Demo.bin";
            Console.Write("Enter block size. ");
            BlockSize = Lesson1_Utility.InputPositiveNumber();
            long FileSize = new System.IO.FileInfo(FileName).Length / 4;
            BlockAmount = (int)(FileSize / BlockSize);
            if ((FileSize % BlockSize) != 0) BlockAmount++ ;
            var Blocks = new BlockInfo[BlockAmount];
            for (int i = 0; i < BlockAmount; i++) {
                Blocks[i] = new BlockInfo();
                Blocks[i].offset = i*(long)BlockSize;
                Blocks[i].size = BlockSize;
            }
            if ((FileSize % BlockSize) != 0) Blocks[BlockAmount-1].size = (int)(FileSize % BlockSize);
            Console.WriteLine($"There are {BlockAmount} blocks.");
            // Sorting blocks
            BinaryReader Reader = new BinaryReader(File.Open(FileName, FileMode.Open));
            BinaryWriter Writer = new BinaryWriter(File.Open("Temp.bin", FileMode.Create));
            int[] buffer = null;
            int CurrentBufferSize = 0;
            for (int i = 0; i < BlockAmount; i++) {
                if (Blocks[i].size != CurrentBufferSize) {
                    CurrentBufferSize = Blocks[i].size;
                    buffer = new int[CurrentBufferSize];
                }
                for (int j = 0; j < CurrentBufferSize; j++) buffer[j] = Reader.ReadInt32();
                //Array.Sort(buffer);
                Lesson8_Task1_Bcksrt.BucketSort(buffer);
                for (int j = 0; j < CurrentBufferSize; j++) Writer.Write(buffer[j]);
            }
            Reader.Dispose(); Writer.Dispose();
            // Merging blocks
            Reader = new BinaryReader(File.Open("Temp.bin", FileMode.Open));
            Writer = new BinaryWriter(File.Open(FileName + ".sorted", FileMode.Create));
            for (int i = 0; i < BlockAmount; i++) Blocks[i].GetValue(Reader);
            BlockInfo min;
            do {
                min = null;
                for (int i = 0; i < BlockAmount; i++) {
                    if (Blocks[i].size == 0) continue;
                    if ((min == null) || (Blocks[i].value < min.value)) min = Blocks[i];
                }
                if (min == null) break;
                Writer.Write(min.value); 
                min.size--;
                if (min.size > 0) min.GetValue(Reader);
            } while (true);
            Reader.Dispose(); Writer.Dispose();
            File.Delete("Temp.bin");
            // Verifing 
            Reader = new BinaryReader(File.Open(FileName + ".sorted", FileMode.Open));
            int currentvalue = 0, oldvalue;
            bool good = true;
            for (long i = 0; i < FileSize; i++) {
                oldvalue = currentvalue;
                currentvalue = Reader.ReadInt32();
                if ((i > 0) && (currentvalue < oldvalue)) good = false;
            }
            if (good) Console.WriteLine("Verified, file is sorted.");
            else Console.WriteLine("Oooh no! Something went wrong.");
            Reader.Dispose();
        }
    }
    class Lesson8_Task3_8queen {
        static int N;
        static int[] position;
        static bool[] busy;
        static long AmountVariants, DesiredVariant;
        private static void PrintVariant() {
            for (int i = 0; i < N; i++) {
                for (int j = 0; j < N; j++) Console.Write((j == position[i]) ? " Q" : " \u00B7");
                Console.WriteLine();
            }
        }
        private static void TryPutQueen(int K) {
            bool HitsDiagonal = false;
            for (int i = 0; i < N; i++) {
                if (busy[i]) continue;  // there is a queen in this column already; skipped
                HitsDiagonal = false;
                for (int j = 0; j < K; j++) HitsDiagonal |= ((K - j) == Math.Abs(i - position[j]));
                if (HitsDiagonal) continue;  // this cell is under diagonal attack*
                // this cell is suitable for a queen
                position[K] = i; busy[i] = true;
                if (K < (N - 1)) TryPutQueen(K + 1);  // recursively place more queens
                else {  // success: queen successfully placed in last row! gotcha
                    AmountVariants++;
                    if (AmountVariants == DesiredVariant) PrintVariant();
                }
                busy[i] = false;  // free the column - getting ready for a new variant
            }
        }
        public static void Main_Start() {
            Console.WriteLine("Enter dimension of your field N*N:");
            N = Lesson1_Utility.InputPositiveNumber();
            position = new int[N];
            busy = new bool[N];
            for (int i = 0; i < N; i++) busy[i] = false;
            DesiredVariant = 0;
            do {
                AmountVariants = 0;
                TryPutQueen(0);
                Console.WriteLine($"Amount of variants (for field {N}x{N}) is: {AmountVariants}");
                do {
                    DesiredVariant = Lesson2_Utility.InputNumber("Choose your variant or 0 to exit");
                    if ((DesiredVariant >= 0) && (DesiredVariant <= AmountVariants)) break;
                    else Console.WriteLine($"Please enter number from 0 to {AmountVariants}.");
                } while (true);
                if (DesiredVariant == 0) break;
            } while (true);
        }
    }
}
