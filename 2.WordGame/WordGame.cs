namespace _2.WordGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal class Program
    {
        private static void Main(string[] args)
        {
            // string word = "ivan";
            // char[,] table =
            // {
            // { 'i', 'v', 'a', 'n' }, { 'e', 'v', 'n', 'h' }, { 'i', 'n', 'a', 'v' }, 
            // { 'm', 'v', 'v', 'n' }, { 'q', 'r', 'i', 't' }
            // };
            string word = Console.ReadLine();
            string input = Console.ReadLine();
            List<string> inputData = new List<string>();
            while (string.IsNullOrEmpty(input))
            {
                input = Console.ReadLine();
                inputData.Add(input);
            }

            char[,] table = new char[inputData.Count, inputData[0].Length];
            for (int row = 0; row < table.GetLength(0); row++)
            {
                for (int col = 0; col < table.GetLength(1); col++)
                {
                    table[row, col] = inputData[row][col];
                }
            }

            string reversedWord = ReverseWord(word);

            Console.WriteLine(reversedWord);
            char[,] mirrorTable = (char[,])table.Clone();
            Reverse2DimArray(mirrorTable);

            List<string> diagonalsOfTable = GetDiagonals(table);
            var filteredDiagonals = FilterDiagonals(diagonalsOfTable, word);

            List<string> diagonalsOfMirrorTable = GetDiagonals(mirrorTable);
            var filteredDiagonalsTable = FilterDiagonals(GetDiagonals(table), word);
            var filteredDiagonalsMirrorTable = FilterDiagonals(GetDiagonals(mirrorTable), word);
            int result = 0;
            result += CountOcc(filteredDiagonalsTable, word);
            result += CountOcc(filteredDiagonalsMirrorTable, word);
            result += ChexkMatrixVertically(table, word);
            result += ChexkMatrixVertically(table, ReverseWord(word));
            result += CheckMatrixHorizontally(table, word);
            result += CheckMatrixHorizontally(table, ReverseWord(word));
            Console.WriteLine(result);
        }

        public static string ReverseWord(string word)
        {
            string reversedWord = string.Empty;
            for (int i = word.Length - 1; i >= 0; i--)
            {
                reversedWord += word[i];
            }

            return reversedWord;
        }

        public static int CountOcc(List<string> strings, string pattern)
        {
            string reversedStr = ReverseWord(pattern);
            int countOcc = 0;
            foreach (string str in strings)
            {
                countOcc += Regex.Matches(str, pattern).Count;
                countOcc += Regex.Matches(str, reversedStr).Count;
            }

            return countOcc;
        }

        public static List<string> FilterDiagonals(List<string> diagonals, string word)
        {
            var filteredDiagonals = diagonals.Where(d => d.Length >= word.Length).ToList();
            return filteredDiagonals;
        }

        public static void Reverse2DimArray(char[,] array)
        {
            for (int rowIndex = 0; rowIndex <= array.GetUpperBound(0); rowIndex++)
            {
                for (int colIndex = 0; colIndex <= (array.GetUpperBound(1) / 2); colIndex++)
                {
                    char tempHolder = array[rowIndex, colIndex];
                    array[rowIndex, colIndex] = array[rowIndex, array.GetUpperBound(1) - colIndex];
                    array[rowIndex, array.GetUpperBound(1) - colIndex] = tempHolder;
                }
            }
        }

        public static List<string> GetDiagonals(char[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            List<string> diagonals = new List<string>();

            for (int slice = 0; slice < rows + cols - 1; ++slice)
            {
                List<char> diagonal = new List<char>();
                int z1 = slice < cols ? 0 : slice - cols + 1;
                int z2 = slice < rows ? 0 : slice - rows + 1;
                for (int j = slice - z2; j >= z1; --j)
                {
                    diagonal.Add(matrix[j, slice - j]);
                }

                string diagonalAsString = String.Format(String.Join(string.Empty, diagonal));
                diagonals.Add(diagonalAsString);
                diagonal.Clear();
            }

            return diagonals;
        }

        public static int ChexkMatrixVertically(char[,] matrix, string word)
        {
            string reversedWord = word.Reverse().ToString();
            int countOfLetterOccurarances = 0;
            int positionInWord = 0;
            int wordOccurances = 0;
            for (int col = 0; col < matrix.GetLength(1); col++)
            {
                for (int row = 0; row < matrix.GetLength(0); row++)
                {
                    if (matrix[row, col] == word[positionInWord])
                    {
                        countOfLetterOccurarances++;
                        positionInWord++;
                    }
                    else
                    {
                        countOfLetterOccurarances = 0;
                        positionInWord = 0;
                    }

                    if (countOfLetterOccurarances == word.Length)
                    {
                        wordOccurances++;
                        positionInWord = 0;
                        countOfLetterOccurarances = 0;
                    }
                }

                positionInWord = 0;
                countOfLetterOccurarances = 0;
            }

            return wordOccurances;
        }

        public static int CheckMatrixHorizontally(char[,] matrix, string word)
        {
            int countOfLetterOccurrances = 0;
            int positionInWord = 0;
            int wordOccurances = 0;
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    if (matrix[row, col] == word[positionInWord])
                    {
                        countOfLetterOccurrances++;
                        positionInWord++;
                    }
                    else
                    {
                        countOfLetterOccurrances = 0;
                        positionInWord = 0;
                    }

                    if (countOfLetterOccurrances == word.Length)
                    {
                        wordOccurances++;
                        positionInWord = 0;
                        countOfLetterOccurrances = 0;
                    }
                }

                positionInWord = 0;
                countOfLetterOccurrances = 0;
            }

            return wordOccurances;
        }
    }
}