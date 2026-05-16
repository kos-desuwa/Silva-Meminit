using System;
using System.Collections.Generic;
using System.Text;

namespace Silva_Meminit
{
    internal abstract class Grove
    {
        public int Width { get; private init; }
        public int Height { get; private init; }
        public Grove(int height, int width)
        {
            Width = width;
            Height = height;
        }
        public bool IsInBounds(int r, int c) =>
            c >= 0 && c < Width && r >= 0 && r < Height;
        public void Draw()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(ToString());
            Console.SetCursorPosition(0, Height + 2);
        }
    }
}
