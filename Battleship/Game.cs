using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Print message if target has already been shot(update shoot function in gameboard.cs that returns if target has already been shot)

namespace Battleship
{
    /// <summary>
    /// Creates a game of battleship! Use Start() to play.
    /// Uses default constructor.
    /// </summary>
    public class Game
    {
        //Constants
        private const int GRIDX = 10, GRIDY = 10;
        private const char BACKGROUND = ' ';

        //Variables
        private Random random = new Random();
        private List<Ship> ships;
        private Gameboard board;
        private List<List<int[]>> shipCoords;
        //Logic Variables
        private bool running;
        private bool hitting;
        private string input;

        public void Start()
        {
            input = "1";
            while(input[0] != '0')
            {
                Init();
                Run();
                do
                {
                    Console.Write("Do you want to play another round of battleship (1 = Yes, 0 = No)? ");
                    input = Console.ReadLine();
                }
                while (string.IsNullOrEmpty(input) || !(input[0] == '0' || input[0] == '1'));
            }
            Console.WriteLine("Thanks for playing!");
        }

        private void Init()
        {
            //Create Ships
            ships = MakeShips(2, 2, 1, 1);
            foreach (Ship ship in ships)
            {
                ship.Rotation = (random.Next(2) == 1);
            }
            RandomizeShips(ships, GRIDX, GRIDY);
            /*
            foreach (Ship ship in ships)
            {
                ship.printDetails();
                Console.WriteLine();
            }
            */

            //Create a game board with the ships
            board = new Gameboard(GRIDY, GRIDX, ships);

            //Create a list of ship coordinates
            shipCoords = new List<List<int[]>>();
            foreach (Ship ship in ships)
            {
                shipCoords.Add(ship.GetCoords());
            }
        }

        private void Run()
        {
            running = true;
            while (running)
            {
                //Display board
                do
                {
                    Console.Write("Display game grid(0) or ship grid(1)? ");
                    input = Console.ReadLine();
                }
                while (string.IsNullOrEmpty(input) || !(input[0] == '0' || input[0] == '1'));
                board.DisplayGrid(input[0] == '1');

                //Get target coordinates
                do
                {
                    Console.Write("Enter target coordinates (Example B2) (case sen): ");
                    input = Console.ReadLine();
                }
                while (string.IsNullOrEmpty(input) || (input[0] < 'A' || input[0] > 'A' + GRIDY - 1) || (input[1] < '0' || input[1] > '0'+GRIDX));
                if (board.Shoot(input[0] - 'A', input[1] - '0'))
                {
                    Console.WriteLine("That's already been shot!");
                }
                else
                {
                    foreach (List<int[]> list in shipCoords)
                    {
                        hitting = false;
                        foreach (int[] coord in list)
                        {
                            if (coord[0] == input[0] - 'A' && coord[1] == input[1] - '0')
                            {
                                hitting = true;
                                list.Remove(coord);
                                break;
                            }
                        }
                        if (!list.Any() && hitting)
                        {
                            Console.Write("You sunk a(n) ");
                            board.PrintShip(input[0] - 'A', input[1] - '0');
                        }
                    }
                }

                //If there are no more shootable coordinates, game is finished.
                running = false;
                foreach(List<int[]> list in shipCoords)
                {
                    if(list.Any())
                    {
                        running = true;
                    }
                }
            }
            Console.WriteLine("Game Over! You sunk all the ships!");
        }

        /// <summary>
        /// Makes a certain amount of ships with certain lengths.
        /// </summary>
        /// <param name="amounts">amount of length 2, amount of length 3...</param>
        private List<Ship> MakeShips(params int[] amounts)
        {
            List<Ship> ships = new List<Ship>();
            for (int i = 0; i < amounts.GetLength(0); i++)
            {
                for (int j = 0; j < amounts[i]; j++)
                {
                    Ship ship = new Ship(i + 2);
                    ships.Add(ship);
                }
            }
            return ships;
        }

        /// <summary>
        /// Give each ship a random position and rotation.
        /// Possible infinite loop if not enough space for ships.
        /// (Can be fixed by redoing entire function to place ships from biggest the smallest then checking empty spaces)
        /// </summary>
        private void RandomizeShips(List<Ship> ships, int xMax, int yMax)
        {
            int xPos = xMax, yPos = yMax;
            bool placed = false;
            Random random = new Random();
            foreach (Ship ship in ships)
            {
                if (ship.Length() > xMax && ship.Length() > yMax)
                {
                    Console.WriteLine("Unable to place ship due to length.");
                }
                else
                {
                    placed = false;
                    while (!placed)
                    {
                        placed = true;
                        if (ship.Rotation)
                        {
                            xPos = random.Next(xMax);
                            yPos = random.Next(yMax - ship.Length());
                        }
                        else
                        {
                            xPos = random.Next(xMax - ship.Length());
                            yPos = random.Next(yMax);
                        }
                        ship.setPosition(xPos, yPos);
                        foreach (Ship otherShip in ships)
                        {
                            if (ship != otherShip)
                            {
                                //Placing Vertical Ship
                                if (ship.Rotation)
                                {
                                    //Other ship is vertical and X position matches
                                    if (otherShip.Rotation)
                                    {
                                        if (xPos == otherShip.GetX())
                                        {
                                            //Current ship is smaller
                                            if (ship.Length() < otherShip.Length())
                                            {
                                                for (int i = yPos; i < yPos + ship.Length(); i++)
                                                {
                                                    if (i >= otherShip.GetY() && i <= otherShip.GetY() + otherShip.Length())
                                                    {
                                                        placed = false;
                                                    }
                                                }
                                            }
                                            //Current ship is not smaller
                                            else
                                            {
                                                for (int i = otherShip.GetY(); i < otherShip.GetY()+otherShip.Length(); i++)
                                                {
                                                    if(i >= yPos && i <= yPos + ship.Length())
                                                    {
                                                        placed = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //Other ship is horizontal
                                    else
                                    {
                                        //Check if any part of the ship matches y pos of other ship
                                        for(int i = yPos; i < yPos + ship.Length(); i++)
                                        {
                                            if(i == otherShip.GetY() && ship.GetX() >= otherShip.GetX() && ship.GetX() <= otherShip.GetX() + otherShip.Length())
                                            {
                                                placed = false;
                                            }
                                        }
                                    }
                                }
                                //Placing Horizontal Ship
                                else
                                {
                                    //Other ship is horizontal and Y position matches
                                    if (!otherShip.Rotation)
                                    {
                                        if (yPos == otherShip.GetY())
                                        {
                                            //Current ship is smaller
                                            if (ship.Length() < otherShip.Length())
                                            {
                                                for (int i = xPos; i < xPos + ship.Length(); i++)
                                                {
                                                    if (i >= otherShip.GetX() && i <= otherShip.GetX() + otherShip.Length())
                                                    {
                                                        placed = false;
                                                    }
                                                }
                                            }
                                            //Current ship is not smaller
                                            else
                                            {
                                                for (int i = otherShip.GetX(); i < otherShip.GetX() + otherShip.Length(); i++)
                                                {
                                                    if (i >= xPos && i <= xPos + ship.Length())
                                                    {
                                                        placed = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //Other ship is vertical
                                    else
                                    {
                                        //Check if any part of the ship matches x pos of other ship
                                        for (int i = xPos; i < xPos + ship.Length(); i++)
                                        {
                                            if (i == otherShip.GetX() && ship.GetY() >= otherShip.GetY() && ship.GetY() <= otherShip.GetY() + otherShip.Length())
                                            {
                                                
                                                placed = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Prints all the ships and their details.
        /// </summary>
        private void PrintShips(List<Ship> ships)
        {
            foreach (Ship ship in ships)
            {
                Console.WriteLine("Rotation = " + ship.Rotation);
                Console.WriteLine("Position = " + ship.GetX() + " " + ship.GetY());
                Console.WriteLine("Length = "+ship.Length());
                Console.WriteLine();
            }
        }
    }
}