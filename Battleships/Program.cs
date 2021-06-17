using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Battleships
{
    class Program
    {
        //Create list of list of battleship and destroyers and store the row and column index as key value pair.
        protected Dictionary<int, List<int>> rowColumnPairs = new Dictionary<int, List<int>>();
        protected int[,] gridArray = new int[10, 10];
        private int bSize = 5;
        private int dSize = 4;

        static void Main(string[] args)
        {
            Program prg = new Program();
            prg.CreateRandomBD();          
            prg.ShootShip();
        }

        public void CreateRandomBD()
        {
            var random = new Random();
            
            //Generate battleship grid
            GenerateRandomGrid(random, bSize);

            //Generate destroyer 1
            GenerateRandomGrid(random, dSize);

            //Generate destroyer 2
            GenerateRandomGrid(random, dSize);

            //UpdateGrid(gridArray);
        }

        public void GenerateRandomGrid(Random random, int size)
        {
            bool isGridCreatedSuccessfully = false;
            while (!isGridCreatedSuccessfully)
            {
                var row = random.Next(0, 9);
                var column = random.Next(0, 9);                

                isGridCreatedSuccessfully = ValidateGrid(row, column, size);
            }
        }

        public bool ValidateGrid(int row, int column, int size)
        {
            bool isGoodGridFound = false;
            List<int> listGrids = new List<int>();
            try
            {
                if (column + (size - 1) <= 9)
                {
                    for (int i = column; i <= column + (size - 1); i++)
                    {
                        if (gridArray[row, i] == 1)
                        {
                            break;
                        }
                        listGrids.Add(i);
                    }
                    if (listGrids.Count == size)
                    {
                        foreach (int col in listGrids)
                        {
                            gridArray[row, col] = 1;
                            UpdateRowColumnDictionary(row, col);
                            isGoodGridFound = true;
                        }
                    }
                }
                else if ((column - size) + 1 >= 0)
                {
                    for (int i = column; i >= (column - size) + 1; i--)
                    {
                        if (gridArray[row, i] == 1)
                        {
                            break;
                        }
                        listGrids.Add(i);
                    }
                    if (listGrids.Count == size)
                    {
                        foreach (int col in listGrids)
                        {
                            gridArray[row, col] = 1;
                            UpdateRowColumnDictionary(row, col);
                            isGoodGridFound = true;
                        }
                    }
                }
                else if (row + (size - 1) <= 9)
                {
                    for (int i = row; i <= row + (size - 1); i++)
                    {
                        if (gridArray[i, column] == 1)
                        {
                            break;
                        }
                        listGrids.Add(i);
                    }
                    if (listGrids.Count == size)
                    {
                        foreach (int r in listGrids)
                        {
                            gridArray[r, column] = 1;
                            UpdateRowColumnDictionary(r, column);
                            isGoodGridFound = true;
                        }
                    }
                }
                else if ((row - size) + 1 >= 0)
                {
                    for (int i = row; i >= (row - size) + 1; i--)
                    {
                        if (gridArray[i, column] == 1)
                        {
                            break;
                        }
                        listGrids.Add(i);
                    }
                    if (listGrids.Count == size)
                    {
                        foreach (int r in listGrids)
                        {
                            gridArray[r, column] = 1;
                            UpdateRowColumnDictionary(r, column);
                            isGoodGridFound = true;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception occured : " + ex);
            }
            return isGoodGridFound;
        }

        public void UpdateGrid(int[,] arrayGrid)
        {
            Console.WriteLine();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(arrayGrid[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.ReadKey();
            Environment.Exit(0);
        }

        public void ShootShip()
        {
            int totalValidAttempts = 0;
            int totalHits = 0;
            int totalMisses = 0;
            bool isAllShipsDestroyed = false;

            while (!isAllShipsDestroyed)
            {
                Console.WriteLine("Enter row and column coordinates (For e.g. A5 where A is column (A - J) and 5 is row (0 - 9)) : ");
                string input = Console.ReadLine();

                int row = 0;
                int column = 0;
                try
                {
                    CheckInput(input, out row, out column);
                }
                catch
                {
                    input = string.Empty;
                    Console.WriteLine("Invalid input! Please try again : ");
                    input = Console.ReadLine();
                    CheckInput(input, out row, out column);
                }

                totalValidAttempts += 1;
                
                //Check whether hit, miss or sink
                int inputValue = gridArray[row, column];

                if (inputValue == 1)
                {
                    totalHits += 1;
                    Console.WriteLine("Hit");

                    //Check from rowColumnPairs
                    isAllShipsDestroyed = CheckIfShipSunk(row, column);
                }
                else
                {
                    totalMisses += 1;
                    Console.WriteLine("Miss");
                }
            }

            Console.WriteLine("All ships destroyed");
            Console.WriteLine("Total hits : " + totalHits);
            Console.WriteLine("Total misses : " + totalMisses);
            Console.WriteLine("Total attempts : " + totalValidAttempts);

            UpdateGrid(gridArray);
        }

        public void CheckInput(string input, out int row, out int column)
        {
            Regex regex = new Regex(@"([a-zA-Z]+)(\d+)");
            Match match = regex.Match(input);

            string columnText = match.Groups[1].Value;
            row = Convert.ToInt32(match.Groups[2].Value);

            Columns col = (Columns)Enum.Parse(typeof(Columns), columnText);
            column = (int)col;
        }

        public bool CheckIfShipSunk(int row, int column)
        {
            bool isShipSunk = false;
            if (rowColumnPairs.ContainsKey(row) && rowColumnPairs[row].Contains(column))
            {
                rowColumnPairs[row].Remove(column);
                if (rowColumnPairs[row].Count <= 0)
                {
                    rowColumnPairs.Remove(row);
                }
            }
            if (rowColumnPairs.Count() <= 0)
            {
                isShipSunk = true;
                rowColumnPairs = null;
            }
            return isShipSunk;
        }

        public void UpdateRowColumnDictionary(int row, int column)
        {
            if (rowColumnPairs.ContainsKey(row))
            {
                rowColumnPairs[row].Add(column);
            }
            else
            {
                rowColumnPairs.Add(row, new List<int> { column });
            }
        }
        
        enum Columns
        {
            A,
            B,
            C,
            D,
            E,
            F,
            G,
            H,
            I,
            J
        }
    }
}
