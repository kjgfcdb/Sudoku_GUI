using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sudoku
{
    public class Sudoku
    {

        private int require, total;
        private Random random = new Random(DateTime.Now.Second);
        private char[,,] output = new char[1000001, 20, 20];
        private bool[,] checkrow, checkcol, checksqr;
        public int[,] GeneratePuzzle()
        {
            int[,] map = new int[9, 9];
            List<int> temp = new List<int>();
            for (int i = 1; i <= 9; i++)
            {
                temp.Add(i);
            }
            for (int i = 0; i < 9; i++)
            {
                int tot = random.Next(0, 9);
                int foo = temp[i];
                temp[i] = temp[tot];
                temp[tot] = foo;
            }
            require = 1;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    map[i, j] = temp[i * 3 + j];
                }
            }
            PreProcess(map);
            Dfs(map, 0, 3);
            int[,] ret = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (random.Next(2) == 0)
                    {
                        ret[i, j] = 0;
                    }
                    else
                    {
                        ret[i, j] = (int)output[0, i, j * 2] - 48;
                    }
                }
            }
            return ret;
        }
        public void GenerateSudoku(int num)
        {
            int[,] map = new int[9, 9];
            map[0, 0] = 9; // Teacher's requirement
            require = num;
            PreProcess(map);
            Dfs(map, 0, 0);
            using (FileStream fs = new FileStream("Sudoku.txt", FileMode.Create, FileAccess.Write))
            {
                using (TextWriter tw = new StreamWriter(fs, Encoding.UTF8, 65536))
                {
                    for (int i = 0; i < num; i++)
                    {
                        for (int j = 0; j < 9; j++)
                            for (int k = 0; k < 19; k++)
                                tw.Write(output[i, j, k]);
                        tw.Write("------\n");
                    }
                    tw.Flush();
                }
            }
        }
        public void SolveSudoku(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("File doesn't exist!!!");
                return;
            }
            FileStream inFile = new FileStream(path, FileMode.Open, FileAccess.Read);
            FileStream outFile = new FileStream("SolvedSudoku.txt", FileMode.Create, FileAccess.Write);
            TextWriter tw = new StreamWriter(outFile, Encoding.UTF8, 65536);
            string line;
            int index = 0;
            int[,] map = new int[9, 9];
            using (StreamReader sr = new StreamReader(inFile, Encoding.Default))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    if (!line.Trim().Equals(""))
                        foreach (string c in line.Split(' '))
                        {
                            map[index / 9, index % 9] = int.Parse(c);
                            index++;
                        }
                    if (index == 81)
                    {
                        index = 0;
                        PreProcess(map);
                        require = 1;
                        Dfs(map, 0, 0);
                        for (int j = 0; j < 9; j++)
                            for (int k = 0; k < 19; k++)
                                tw.Write(output[0, j, k]);
                        tw.Write("------\n");
                    }
                }
            }
            tw.Flush();
            tw.Close();
            inFile.Close();
            outFile.Close();
        }
        public void PreProcess(int[,] map)
        {
            total = 0;
            checkrow = new bool[9, 10];
            checkcol = new bool[9, 10];
            checksqr = new bool[9, 10];
            int temp;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (map[i, j] != 0)
                    {
                        temp = map[i, j];
                        checkrow[i, temp] = checkcol[j, temp] = checksqr[i - i % 3 + j / 3, temp] = true;
                    }
                }
            }
        }
        public void Dfs(int[,] map, int i, int j)
        {
            if (total == require) return;
            if (i == 9)
            {
                CopyToOutput(map);
                total++;
                return;
            }
            if (map[i, j] != 0)
            {
                if (j == 8) Dfs(map, i + 1, 0);
                else Dfs(map, i, j + 1);
            }
            else
            {
                for (int num = 1; num <= 9; num++)
                {
                    int k = i - i % 3 + j / 3;
                    if (!checkrow[i, num] && !checkcol[j, num] && !checksqr[k, num])
                    {
                        map[i, j] = num;
                        checkrow[i, num] = checkcol[j, num] = checksqr[k, num] = true;
                        if (j == 8) Dfs(map, i + 1, 0);
                        else Dfs(map, i, j + 1);
                        map[i, j] = 0;
                        checkrow[i, num] = checkcol[j, num] = checksqr[k, num] = false;
                    }
                }
            }
        }
        public void CopyToOutput(int[,] map)
        {
            int rowIndex;
            for (int i = 0; i < 9; i++)
            {
                rowIndex = 0;
                for (int j = 0; j < 9; j++)
                {
                    output[total, i, rowIndex++] = (char)(map[i, j] + 48);
                    output[total, i, rowIndex++] = ' ';
                }
                output[total, i, rowIndex++] = '\n';
            }
        }
    }

}