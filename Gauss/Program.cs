using System;
using System.IO;

namespace GausProject
{
    class Program
    {
        static int n = 3; // количество неизвестных
        static double[,] a = new double[n, n];
        static double[,] copya = new double[n, n];
        static double[] b = new double[n];
        static double[] copyb = new double[n];
        static double[] ans = new double[n];
        static int[] x = new int[n];
        static double buf;
        static double startNum = 0;
        static int value = 0;
        static int rowcell = -1, columncell = -1;


        static void MatrixRead(string FileName)
        {
            StreamReader f = new StreamReader(FileName);
            for (int i = 0; i < n; i++)
            {
                string s = f.ReadLine();
                string[] split = s.Split(new Char[] { ' ' });
                for (int j = 0; j < n; j++)
                    copya[i, j] = a[i, j] = Convert.ToDouble(split[j]);
                copyb[i] = b[i] = Convert.ToDouble(split[n]);
            }
        }

        static void MatrixPrint()
        {
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < n; j++)
                    Console.Write("{0, 7}", Math.Round(a[i, j], 3));
                Console.Write("|" + "{0, 7}", Math.Round(b[i], 3));
            }
            Console.WriteLine();
        }
        //Поиск максимального элемента
        static double SearchMax()
        {
            double cell = 0;
            for (int i = value; i < n; i++)
            {
                for (int j = value; j < n; j++)
                    if (Math.Abs(a[i, j]) > cell)
                    {
                        cell = Math.Abs(a[i, j]);
                        rowcell = i;
                        columncell = j;
                    }
            }
            return cell;
        }
        
        //Перестановка строк
        static void RowCell()
        {
            for (int j = value; j < n; j++)
            {
                buf = a[value, j];
                a[value, j] = a[rowcell, j];
                a[rowcell, j] = buf;
            }
            buf = b[value];
            b[value] = b[rowcell];
            b[rowcell] = buf;
        }
        //Перестановка столбцов
        static void ColumnCell()
        {
            for (int i = 0; i < n; i++)
            {
                buf = a[i, value];
                a[i, value] = a[i, columncell];
                a[i, columncell] = buf;
            }
            int bufX;
            bufX = x[value];
            x[value] = x[columncell];
            x[columncell] = bufX;
        }
        //делаем первую единицу
        static void StartOne()
        {
            startNum = a[value, value];
            for (int j = value; j < n; j++)
                a[value, j] /= startNum;
            b[value] /= startNum;
        }
        //делаем нули под еденицей
        static void StartZero()
        {
            for (int i = value + 1; i < n; i++)
            {
                startNum = a[i, value];
                for (int j = 0; j < n; j++)
                {
                    a[i, j] /= startNum;
                    a[i, j] -= a[value, j];
                }
                b[i] /= startNum;
                b[i] -= b[value];
            }
        }
        //Обратный ход
        static void Reverse()
        {
            for (int i = n - 1; i >= 0; i--)
            {
                double sum = b[i];
                for (int j = i + 1; j < n; j++)
                    sum -= a[i, j] * ans[j];
                ans[i] = sum / a[i, i];
            }
        }

        static void CheckAnsver()
        {
            Console.WriteLine("Проверка:");
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += copya[i, j] * ans[j];
                }
                Console.WriteLine("{0} = {1}", sum, copyb[i]);
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            double max = 0;
            MatrixRead("C:\\Users\\yfvfy\\source\\repos\\Gauss\\Gauss\\input.txt");
            Console.WriteLine("Исходная система:");
            MatrixPrint();
            for (int i = 0; i < n; i++)
                x[i] = i + 1;
            for (; value < n; value++)
            {
                Console.WriteLine("Итерация №" + (value + 1));
                max = SearchMax();
                RowCell();
                ColumnCell();
                StartOne();
                StartZero();
                Reverse();
                MatrixPrint();
            }
            for (int i = 0; i < n; i++)
            {
                int j = i;
                while ((j < n - 1) && (x[j] != i + 1))
                    j++;
                if (j != i)
                {
                    int tempX = x[i]; double tempA = ans[i];
                    x[i] = x[j]; ans[i] = ans[j];
                    x[j] = tempX; ans[j] = tempA;
                }
                Console.Write("X" + x[i] + " = ");
                Console.WriteLine("{0, 7}", ans[i]);
            }
            CheckAnsver();
        }
    }
}

