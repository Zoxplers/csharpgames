using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battleship
{
    public class Ship
    {
        private int length;
        private bool rotation; //false - Horizontal, true - Vertical
        private int X;
        private int Y;

        public bool Rotation { get => rotation; set => rotation = value; }

        public Ship(int length)
        {
            this.length = length;
            rotation = false;
            X = 0;
            Y = 0;
        }

        public int Length()
        {
            return length;
        }

        public int GetX()
        {
            return X;
        }

        public int GetY()
        {
            return Y;
        }

        public List<int[]> GetCoords()
        {
            List<int[]> coords = new List<int[]>();
            for (int i = 0; i < length; i++)
            {
                if (rotation)
                {
                    coords.Add(new int[] { X, Y + i });
                }
                else
                {
                    coords.Add(new int[] { X + i, Y });
                }
            }
            return coords;
        }

        public void setPosition(int X, int Y)
        { 
            this.X = X;
            this.Y = Y;
        }
        public void printDetails()
        {
            Console.WriteLine(rotation);
            for(int i = 0; i < length; i++)
            {
                if(rotation)
                {
                    Console.WriteLine(X + " " + (Y + i));
                }
                else
                {
                    Console.WriteLine((X + i) + " " + Y);
                }
            }
        }
    }
}