using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship
{
    public class Gameboard
    {
        private const char SPACE = ' ', HIT = 'O', MISS = 'X', SHIP = 'S';
        private char[,] gameGrid;
        private char[,] shipGrid;

        /// <summary>
        /// Creates a ship grid and a game grid.
        /// </summary>
        /// <param name="">size x</param>
        /// <param name="">size y</param>
        public Gameboard(int x, int y, List<Ship> ships)
        {
            //Game Grid
            gameGrid = new char[x + 1, y + 1];
            FillGrid(SPACE, gameGrid);

            for (int i = 1; i < gameGrid.GetLength(1); i++)
            {
                //47 is '0' - 1
                gameGrid[0, i] = (char)(47 + i);
            }
            for(int i = 1; i < gameGrid.GetLength(0); i++)
            {
                //64 is 'A' - 1
                gameGrid[i, 0] = (char)(64 + i);
            }

            //Ships Grid
            shipGrid = new char[x, y];
            FillGrid(SPACE, shipGrid);
            foreach (Ship ship in ships)
            {
                if(ship.Rotation)
                {
                    for (int i = ship.GetY(); i < ship.GetY() + ship.Length(); i++)
                    {
                        //shipGrid[ship.GetX(), i] = SHIP;
                        shipGrid[ship.GetX(), i] = (char)(SHIP - ship.Length());
                    }
                }
                else
                {
                    for (int i = ship.GetX(); i < ship.GetX() + ship.Length(); i++)
                    {
                        //shipGrid[i, ship.GetY()] = SHIP;
                        shipGrid[i, ship.GetY()] = (char)(SHIP - ship.Length());
                    }
                }
            }
        }

        /// <summary>
        /// Shoots a target on the board.
        /// </summary>
        /// <returns>Returns true if target has already been shot, otherwise false.</returns>
        public bool Shoot(int x, int y)
        {
            bool returnVal = false;
            if (gameGrid[x + 1, y + 1] == MISS || gameGrid[x + 1, y + 1] == HIT)
            {
                returnVal = true;
            }
            gameGrid[x+1, y+1] = MISS;
            if(shipGrid[x,y] != SPACE)
            {
                gameGrid[x + 1, y + 1] = HIT;
            }
            return returnVal;
        }

        /// <summary>
        /// Prints the ship type on the target coordinates.
        /// </summary>
        public void PrintShip(int x, int y)
        {
            switch(SHIP-shipGrid[x,y])
            {
                case 2:
                    Console.Write("Destroyer");
                    break;
                case 3:
                    Console.Write("Submarine");
                    break;
                case 4:
                    Console.Write("Battleship");
                    break;
                case 5:
                    Console.Write("Carrier");
                    break;
                default:
                    Console.Write("Unknown");
                    break;
            }
            Console.WriteLine(".");
        }

        private void FillGrid(char fillChar, char[,] grid)
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    grid[i, j] = fillChar;
                }
            }
        }

        //It displays rotated, but this type of iteration is better memory management
        public void DisplayGrid(bool showShips)
        {
            if (showShips)
            {
                
                for (int i = 0; i < shipGrid.GetLength(0); i++)
                {
                    for (int j = 0; j < shipGrid.GetLength(1); j++)
                    { 
                        if (shipGrid[i, j] == SPACE)
                        {
                            Console.Write("| |");
                        }
                        else
                        {
                            Console.Write($"|S|");
                        }
                        
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                for (int i = 0; i < gameGrid.GetLength(0); i++)
                {
                    for (int j = 0; j < gameGrid.GetLength(1); j++)
                    {
                        Console.Write($"|{gameGrid[i, j]}|");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}