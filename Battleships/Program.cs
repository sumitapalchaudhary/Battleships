using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Battleships
{
    class Battleships
    {
        /// <summary>
        /// Create list of list of battleship and destroyers and store the row and  list of columns as key value pair.
        /// </summary>
        protected Dictionary<int, List<int>> rowColumnPairs = new Dictionary<int, List<int>>();

        /// <summary>
        /// Create a 10 by 10 grid matrix.
        /// </summary>
        protected int[,] gridArray = new int[10, 10];

        /// <summary>
        /// Size of battleship grid - no. of squares occupied by battleship
        /// </summary>
        private int bSize = 5;

        /// <summary>
        /// Size of destroyers grid - no. of squares occupied by destroyers
        /// </summary>
        private int dSize = 4;

        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Initialize new instance of the class Battleships
            Battleships bts = new Battleships();

            //Call method to create the battleship and destroyers in the grid matrix
            bts.CreateBattleshipAndDestroyers();

            //Call method to start the game - get user input for row and column to target
            bts.PlayGame();
        }

        /// <summary>
        /// Method to create 1 battleship and 2 destroyers at random grid points.
        /// This method uses random variable (Random class) to get random values for row and column.
        /// </summary>
        public void CreateBattleshipAndDestroyers()
        {
            var random = new Random();

            //Generate battleship
            GenerateShip(random, bSize);

            //Generate destroyer 1
            GenerateShip(random, dSize);

            //Generate destroyer 2
            GenerateShip(random, dSize);

        }

        /// <summary>
        /// Method to generate random grid points.
        /// This method generates random values for row and column between 0 to 9 range.
        /// The row and column values are then validated using ValidateGrid method 
        /// to check if proper grid squares can be formed in the grid matrix.
        /// </summary>
        /// <param name="random">random variable of Random class</param>
        /// <param name="size">size of battleship or destroyer</param>
        public void GenerateShip(Random random, int size)
        {
            bool isShipCreatedSuccessfully = false;
            while (!isShipCreatedSuccessfully)
            {
                var row = random.Next(0, 9);
                var column = random.Next(0, 9);                

                isShipCreatedSuccessfully = ValidateAndCreateShip(row, column, size);
            }
        }

        /// <summary>
        /// Method to validate the grid points using row, column and size.
        /// This method will check whether the row and column points can create ships of required size or not,
        /// first check will be done horizontally (forward and backward)
        /// and if proper ships couldn't be formed then check vertically (downward and upward).
        /// If required ships are formed then the bool value isGoodShipFound is returned true, else false.
        /// listOfShipIndices - This list of int is created to store the variable grid points from the loop,
        /// once all the points in the gridArray has been checked and if valid ship squares could be formed 
        /// then the gridArray points will be assigned as 1, and the row and column values will be added in the rowColumnPairs dictionary.
        /// </summary>
        /// <param name="row">Value of row generated using random variable</param>
        /// <param name="column">Value of column generated using random variable</param>
        /// <param name="size">Size of the squares of battleship or destroyers</param>
        /// <returns>bool isGoodShipFound - if valid ship created then true, else false.</returns>
        public bool ValidateAndCreateShip(int row, int column, int size)
        {
            bool isGoodShipFound = false;
            List<int> listOfShipIndices = new List<int>();
            try
            {
                //Check for available ship squares in the grid in horizontal direction (forward).
                if (column + (size - 1) <= 9)
                {
                    for (int col = column; col <= column + (size - 1); col++)
                    {
                        //Check if this cell in gridArray is already occupied
                        if (gridArray[row, col] == 1)
                        {
                            break;
                        }
                        listOfShipIndices.Add(col);
                    }
                    //Check if listGrids has all the grid points, same as the required size
                    if (listOfShipIndices.Count == size)
                    {
                        foreach (int col in listOfShipIndices)
                        {
                            gridArray[row, col] = 1;
                            UpdateRowColumnDictionary(row, col);
                            isGoodShipFound = true;
                        }
                    }
                }
                //Check for available ship squares in the grid in horizontal direction (backward).
                else if ((column - size) + 1 >= 0)
                {
                    for (int col = column; col >= (column - size) + 1; col--)
                    {
                        //Check if this cell in gridArray is already occupied
                        if (gridArray[row, col] == 1)
                        {
                            break;
                        }
                        listOfShipIndices.Add(col);
                    }
                    //Check if listGrids has all the grid points, same as the required size
                    if (listOfShipIndices.Count == size)
                    {
                        foreach (int col in listOfShipIndices)
                        {
                            gridArray[row, col] = 1;
                            UpdateRowColumnDictionary(row, col);
                            isGoodShipFound = true;
                        }
                    }
                }
                //Check for available ship squares in the grid in vertical direction (downward).
                else if (row + (size - 1) <= 9)
                {
                    for (int rw = row; rw <= row + (size - 1); rw++)
                    {
                        //Check if this cell in gridArray is already occupied
                        if (gridArray[rw, column] == 1)
                        {
                            break;
                        }
                        listOfShipIndices.Add(rw);
                    }
                    //Check if listGrids has all the grid points, same as the required size
                    if (listOfShipIndices.Count == size)
                    {
                        foreach (int rw in listOfShipIndices)
                        {
                            gridArray[rw, column] = 1;
                            UpdateRowColumnDictionary(rw, column);
                            isGoodShipFound = true;
                        }
                    }
                }
                //Check for available ship squares in the grid in vertical direction (upward).
                else if ((row - size) + 1 >= 0)
                {
                    for (int rw = row; rw >= (row - size) + 1; rw--)
                    {
                        //Check if this cell in gridArray is already occupied
                        if (gridArray[rw, column] == 1)
                        {
                            break;
                        }
                        listOfShipIndices.Add(rw);
                    }
                    //Check if listGrids has all the grid points, same as the required size
                    if (listOfShipIndices.Count == size)
                    {
                        foreach (int rw in listOfShipIndices)
                        {
                            gridArray[rw, column] = 1;
                            UpdateRowColumnDictionary(rw, column);
                            isGoodShipFound = true;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception occured : " + ex);
            }
            return isGoodShipFound;
        }

        /// <summary>
        /// Method to add the row and column points (ship square points) in the dictionary - rowColumnPairs.
        /// Key - row, Value - list of columns.
        /// While playing the game, row and column points obtained as user input is checked in this dictionary,
        /// if match found, then delete the entry.
        /// If all the entries are deleted then game ends.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
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

        /// <summary>
        /// Method to let user play the game. User will input the row and column values.
        /// This input will be checked in the gridArray, if value is 1 then it's a Hit, else Miss.
        /// Also, the row and column pair will be checked in the rowColumnPairs dictionary,
        /// if match found, then delete the entry.
        /// If all the entries are deleted then all ships have sunk and game ends.
        /// Once the game ends, summary and the grid will be displayed to the user.
        /// </summary>
        public void PlayGame()
        {
            int totalValidAttempts = 0;
            int totalHits = 0;
            int totalMisses = 0;
            bool isAllShipsDestroyed = false;

            while (!isAllShipsDestroyed)
            {
                Console.WriteLine("Enter row and column coordinates to shoot (For e.g. A5 where A is column (A - J) and 5 is row (0 - 9)); OR Q to quit : ");
                string input = Console.ReadLine();
                int row;
                int column;

                if (input.ToLower() == "q")
                {
                    break;
                }

                try
                {
                    CheckUserInput(input, out row, out column);
                }
                catch
                {
                    Console.WriteLine("Invalid input! Please try again!");
                    continue;
                }

                totalValidAttempts += 1;
                
                //Check whether hit, miss or sink
                int inputValue = gridArray[row, column];

                if (inputValue == 1)
                {
                    totalHits += 1;
                    Console.WriteLine("It's a Hit!");

                    //Check from rowColumnPairs, if row column match found, delete the entry,
                    //if all entries are deleted then all ships sunk.
                    isAllShipsDestroyed = CheckIfShipSunk(row, column);
                }
                else
                {
                    totalMisses += 1;
                    Console.WriteLine("It's a Miss!");
                }
            }

            //Display summary after game ends.
            if (isAllShipsDestroyed)
            {
                Console.WriteLine("All ships destroyed!");
            }
            Console.WriteLine("Total hits : " + totalHits);
            Console.WriteLine("Total misses : " + totalMisses);
            Console.WriteLine("Total attempts : " + totalValidAttempts);

            //Display array grid with battleships and destroyers after game ends.
            DisplayGrid(gridArray);
        }

        /// <summary>
        /// Method to check the user input and parse into row and column.
        /// User will input column in alphabetical format and row in numeric format,
        /// this method will use regex and parse the row and column (from Columns enum).
        /// </summary>
        /// <param name="input">User input</param>
        /// <param name="row">Output row value</param>
        /// <param name="column">Output column value</param>
        public void CheckUserInput(string input, out int row, out int column)
        {
            Regex regex = new Regex(@"([a-zA-Z]+)(\d+)");
            Match match = regex.Match(input);

            string columnText = match.Groups[1].Value;
            row = Convert.ToInt32(match.Groups[2].Value);

            Columns col = (Columns)Enum.Parse(typeof(Columns), columnText);
            column = (int)col;
        }

        /// <summary>
        /// Method to pass the row and column values as input and check the rowColumnPairs dictionary,
        /// if match exists, then delete the entry; and if all entries are deleted then all ships sunk.
        /// </summary>
        /// <param name="row">Input row</param>
        /// <param name="column">Input column</param>
        /// <returns>bool isAllShipsSunk - if rowColumnPairs is empty, then all ships have sunk, return true, else false.</returns>
        public bool CheckIfShipSunk(int row, int column)
        {
            bool isAllShipsSunk = false;
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
                isAllShipsSunk = true;
                rowColumnPairs = null;
            }
            return isAllShipsSunk;
        }

        /// <summary>
        /// Display the array grid with battleships and destroyers on the console.
        /// </summary>
        /// <param name="arrayGrid">Input array grid</param>
        public void DisplayGrid(int[,] arrayGrid)
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
                
        /// <summary>
        /// Enum Columns to identify columns alphabetically, i.e. column 0 = A, 1 = B...
        /// </summary>
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
