using System;
using Raylib_cs;

namespace Orzea
{
    public class Auton
    {

        public bool[,] History;
        bool[] auxiliar;
        bool[] rule;
        public int Width;
        public int Height;
        public Random Random;
        int iteration = 0;
        int ruleNumber;
        public Auton(int width, int height)
        {
            Random = new Random();
            Width = width;
            Height = height;
            History = new bool[width, height];
            auxiliar = new bool[width];
            rule = new bool[8];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (Random.Next() % 2 == 0)
                    {
                        History[i, j] = false;
                    }
                    else
                    {
                        History[i, j] = true;
                    }
                }
            }
        }

        public void Iterate()
        {
            if (iteration == 0)
            {
                for (int i = 0; i < Width; i++)
                {
                    auxiliar[i] = History[i, 0] ^ History[i, 1];
                    for (int j = 2; j < Height; j++)
                    {
                        auxiliar[i] = auxiliar[i] ^ History[i, j];
                    }
                }
            }
            else
            {
                for (int i = 0; i < Width; i++)
                {
                    auxiliar[i] = History[i, 0];
                }
            }
            iteration++;
            iteration %= 16;
            for (int j = Height - 2; j >= 0; j--)
            {
                for (int i = 0; i < Width; i++)
                {
                    History[i, j + 1] = History[i, j];
                }
            }
            int previous = 0;
            int positional = 1;
            ruleNumber = 0;
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (History[i, j])
                    {
                        ruleNumber += positional;
                    }
                    positional += previous;
                }
            }
            ruleNumber %= 256;
            for (int i = 0; i < 8; i++)
            {
                rule[i] = false;
            }
            int index = 0;
            while (ruleNumber > 0)
            {
                if (ruleNumber % 2 == 0)
                {
                    rule[index] = false;
                }
                else
                {
                    rule[index] = true;
                }
                ruleNumber /= 2;
                index++;
            }
            int leftIndex;
            int centralIndex;
            int rightIndex;
            for (int i = 0; i < Width; i++)
            {
                centralIndex = i;
                int ruleIndex = 0;
                if (i == 0)
                {
                    leftIndex = Width - 1;
                    rightIndex = i + 1;
                }
                else if (i == Width - 1)
                {
                    leftIndex = i - 1;
                    rightIndex = 0;
                }
                else
                {
                    leftIndex = i - 1;
                    rightIndex = i + 1;
                }

                if (auxiliar[leftIndex])
                {
                    ruleIndex += 4;
                }
                if (auxiliar[centralIndex])
                {
                    ruleIndex += 2;
                }
                if (auxiliar[rightIndex])
                {
                    ruleIndex += 1;
                }
                History[i, 0] = rule[ruleIndex];
            }
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            int width = 128;
            int height = 64;
            int cellSize = 12;
            Raylib.InitWindow(width * cellSize, height * cellSize, "Orzea");
            Auton auton = new Auton(width, height);
            Color color;
            float elapsedTime = 0;
            //Raylib.SetTargetFPS(60);
            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (auton.History[i, j])
                        {
                            color = Color.WHITE;
                        }
                        else
                        {
                            color = Color.BLACK;
                        }
                        Raylib.DrawRectangle(i * cellSize, j * cellSize, cellSize, cellSize, color);
                    }
                }
                elapsedTime += Raylib.GetFrameTime();
                if (elapsedTime >= 0.2)
                {
                    auton.Iterate();
                    elapsedTime = 0;
                }
                Raylib.EndDrawing();
                
            }

            Raylib.CloseWindow();
        }
    }
}
