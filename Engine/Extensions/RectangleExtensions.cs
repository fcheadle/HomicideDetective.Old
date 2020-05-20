﻿using GoRogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Extensions
{
    public static class RectangleExtensions
    {
        public static IEnumerable<Rectangle> RecursiveBisect(this Rectangle parent, int minimumDimension)
        {
            List<Rectangle> ogChildren = Bisect(parent).ToList();
            List<Rectangle> children = new List<Rectangle>(); //so that we can modify children during the loop
            foreach (Rectangle child in ogChildren)
            {
                if (child.Width > minimumDimension && child.Height > minimumDimension)
                {
                    children.AddRange(RecursiveBisect(child, minimumDimension));
                }
                else
                {
                    children.Add(child);
                }
            }

            return children;
        }

        public static IEnumerable<Rectangle> Bisect(this Rectangle parent)
        {
            if (parent.Width > parent.Height)
                return parent.BisectVertically();

            else if (parent.Width < parent.Height)
                return parent.BisectHorizontally();

            else
                return Calculate.Percent() % 2 == 1 ? BisectHorizontally(parent) : BisectVertically(parent);
        }

        //puts a horizontal line through the rectangle
        public static IEnumerable<Rectangle> BisectHorizontally(this Rectangle rectangle)
        {
            int startX = rectangle.MinExtentX;
            int stopY = rectangle.MaxExtentY;
            int stopX = rectangle.MaxExtentX;
            int startY = rectangle.MinExtentY;
            int bisection = 0;
            while (bisection < rectangle.MinExtentY + rectangle.Height / 5 || bisection > rectangle.MaxExtentY - rectangle.Height / 5)
                bisection = rectangle.RandomPosition().Y;

            
            yield return new Rectangle(new Coord(startX, startY), new Coord(stopX, bisection));
            yield return new Rectangle(new Coord(startX, bisection), new Coord(stopX, stopY));
        }
        public static IEnumerable<Rectangle> BisectVertically(this Rectangle rectangle)
        {
            int startY = rectangle.MinExtentY;
            int stopY = rectangle.MaxExtentY;
            int startX = rectangle.MinExtentX;
            int stopX = rectangle.MaxExtentX;
            int bisection = 0;
            while (bisection < rectangle.MinExtentX + rectangle.Width / 5 || bisection > rectangle.MaxExtentX - rectangle.Width / 5)
                bisection = rectangle.RandomPosition().X;

            yield return new Rectangle(new Coord(startX, startY), new Coord(bisection, stopY));
            yield return new Rectangle(new Coord(bisection, startY), new Coord(stopX, stopY));
        }
    }
}
