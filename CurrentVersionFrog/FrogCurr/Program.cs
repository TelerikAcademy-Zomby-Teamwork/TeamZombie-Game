﻿﻿﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using System.Threading;
using System.IO;

class FrogCurr
{
    // ???? ?? ? ????????? ????? ?? ?????? ? ?????????????
    static int lives = 5;
    static int score = 0;

    public struct Object
    {

        public int x;
        public int y;
        public string c;
        public ConsoleColor color;
        public Object(int x, int y, string c, ConsoleColor color)// constructor
        {
            this.x = x;
            this.y = y;
            this.c = c;
            this.color = color;
        }
    }// the objects

    public static void FillGrass(char[,] grass)
    {
        for (int i = 0; i < grass.GetLength(0); i++)
        {
            for (int j = 0; j < grass.GetLength(1); j++)
            {
                grass[i, j] = '#';
                grass[1, 1] = ' ';
                grass[1, 7] = ' ';
                grass[1, 15] = ' ';
                grass[1, 22] = ' ';
                grass[1, 28] = ' ';
            }
        }
    }
    //  fill the grass array 
    public static void DrawTheGrass(char[,] grass)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        for (int i = 0; i < grass.GetLength(0); i++)
        {
            for (int j = 0; j < grass.GetLength(1); j++)
            {
                Console.Write(grass[i, j]);
            }
        }
    } //draw grass

    public static void DrawSafetyZone(int scoreWindowBuffer)
    {
        Console.ResetColor();
        Console.SetCursorPosition(0, Console.WindowHeight - scoreWindowBuffer);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(new string('=', Console.WindowWidth));

        Console.SetCursorPosition(0, Console.WindowHeight / 2 - scoreWindowBuffer + 2);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(new string('=', Console.WindowWidth));
    }// draw safety zone 
    public static void DrawWater()
    {
        Console.ResetColor();
        Console.SetCursorPosition(0, 2);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(new string('=', Console.WindowWidth));
        Console.Write(new string('=', Console.WindowWidth));
        Console.Write(new string('=', Console.WindowWidth));
        Console.Write(new string('=', Console.WindowWidth));
        Console.Write(new string('=', Console.WindowWidth));
    }
    public static int[] Moves1(Object frog, int[] data, int scoreWindowBuffer)
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo pressedKey = Console.ReadKey(true);
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
            if (pressedKey.Key == ConsoleKey.Escape)
            {
                PrintOnPosition(10, 7, @"
    Paused
    Any key ---> Continue.
    Q ---> Quit.", ConsoleColor.Red);
                pressedKey = Console.ReadKey();
                if (pressedKey.Key == ConsoleKey.C)
                {
                    data[0] = 0;
                }
                if (pressedKey.Key == ConsoleKey.Q)
                {
                    Console.WriteLine();
                    data[0] = 1;
                }
            }
            if (pressedKey.Key == ConsoleKey.LeftArrow)
            {
                if (frog.x >= 1)
                {
                    frog.x--;
                }
            }
            else if (pressedKey.Key == ConsoleKey.RightArrow)
            {
                if (frog.x < Console.WindowWidth - 1)
                {
                    frog.x++;
                }
            }
            else if (pressedKey.Key == ConsoleKey.Spacebar)// specila jump forward
            {
                if (frog.y > 1)
                {
                    frog.y -= 2;
                }
            }
            else if (pressedKey.Key == ConsoleKey.B)// specila jump backward
            {
                if (frog.y < Console.WindowHeight - scoreWindowBuffer - 1)
                {
                    frog.y += 2;
                }
            }

            else if (pressedKey.Key == ConsoleKey.DownArrow)
            {
                if (frog.y < Console.WindowHeight - scoreWindowBuffer)
                {
                    frog.y++;
                }
            }
            else if (pressedKey.Key == ConsoleKey.UpArrow)
            {
                if (frog.y >= 1)
                {
                    frog.y--;
                }
            }
        }
        data[1] = frog.x;
        data[2] = frog.y;
        return data;
    }//move frog and chek for over 

    public static void PrintOnPosition(int x, int y, string c = "", ConsoleColor color = ConsoleColor.Gray)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(c);
    }

    public static void FillListOfCars(List<Object> cars, int scoreWindowBuffer)
    {
        int rowCounter = Console.WindowHeight - scoreWindowBuffer - 1;
        byte carCount = 0;

        string smallCars = ">>";
        string middleCars = ">>>";

        byte counter = 0;
        byte col = 0;

        for (int i = 0; i < 18; i++)
        {
            if (counter == 0)
            {
                cars.Add(new Object(col, rowCounter, smallCars, ConsoleColor.Red));
            }
            if (counter == 1)
            {
                cars.Add(new Object(col, rowCounter, middleCars, ConsoleColor.DarkCyan));
            }
            if (counter == 2)
            {
                cars.Add(new Object(col, rowCounter, smallCars, ConsoleColor.White));
            }
            col += 8;

            carCount++;
            if (carCount % 3 == 0)
            {
                rowCounter--;
                col = 0;
                switch (carCount / 3)
                {
                    case 0: col = 5; break;
                    case 1: col = 5; break;
                    case 2: col = 4; break;
                    case 3: col = 0; break;
                    case 4: col = 6; break;
                    case 5: col = 0; break;
                    case 6: col = 7; break;
                    default: col = 0; break;
                }
                counter++;
                if (counter > 2)
                {
                    counter = 0;
                }
            }
        }
    }

    public static void FillListOfWoods(List<Object> woods, int scoreWindowBuffer)
    {
        int rowCounter = 6;
        byte woodCount = 0;


        string bigWoods = "00000";

        byte counter = 0;
        byte col = 0;

        for (int i = 0; i < 15; i++)
        {
            if (counter == 0)
            {
                woods.Add(new Object(col, rowCounter, bigWoods, ConsoleColor.DarkRed));
            }
            if (counter == 1)
            {
                woods.Add(new Object(col, rowCounter, bigWoods, ConsoleColor.DarkMagenta));
            }
            if (counter == 2)
            {
                woods.Add(new Object(col, rowCounter, bigWoods, ConsoleColor.White));
            }
            col += 8;

            woodCount++;
            if (woodCount % 3 == 0)
            {
                rowCounter--;
                col = 0;
                switch (woodCount / 3)
                {
                    case 0: col = 4; break;
                    case 1: col = 2; break;
                    case 2: col = 3; break;
                    case 3: col = 2; break;
                    case 4: col = 4; break;
                    case 5: col = 1; break;
                    case 6: col = 4; break;
                    default: col = 3; break;
                }
                counter++;
                if (counter > 2)
                {
                    counter = 0;
                }
            }
        }
    }
    static void GameOver()
    {
        Console.Clear();
        PrintOnPosition(8, 5, "Game Over!", ConsoleColor.Red);
        PrintOnPosition(6, 6, "Your score is " + score, ConsoleColor.Red);
        PrintOnPosition(4, 7, "Press [enter] for exit", ConsoleColor.Red);
        Console.ReadLine();
        Environment.Exit(0);
    }

    static void Died()
    {
        Console.Clear();
        PrintOnPosition(10, 5, "You are dead", ConsoleColor.Red);
        Thread.Sleep(1000);
        lives--;
    }

    static void Main()
    {

        Console.BufferHeight = Console.WindowHeight = 18;
        Console.BufferWidth = Console.WindowWidth = 30;
        int n = Console.WindowWidth;

        char[,] grass = new char[2, n];// grass array 
        FillGrass(grass);// 

        int scoreWindowBuffer = 4;

        // ???? ??????? ? ????? ?? ?? ???????? ???????? ?? ??????
        Object frog = new Object();
        frog.x = Console.WindowWidth / 2;
        frog.y = Console.WindowHeight - scoreWindowBuffer;
        frog.c = "X";
        frog.color = ConsoleColor.Yellow;
        int[] data = new int[3] { 0, frog.x, frog.y };
        List<Object> cars = new List<Object>(18);
        List<Object> woods = new List<Object>(12);
        FillListOfCars(cars, scoreWindowBuffer);
        FillListOfWoods(woods, scoreWindowBuffer);
        int x;
        int y;
        string c;
        int frogCord;
        int carCord;
        int woodCord;
        ConsoleColor color;

        DrawWater();

        while (true)
        {
            // TODO:
            // check if frog is in the grass matrix
            //if (frog.x == !!! && frog.y == !!!)
            //{
            //    score += 50;
            //}

            Moves1(frog, data, scoreWindowBuffer);
            if (data[0] == 1)//chek for exit 0=exit
            {
                return;
            }

            // Move our frog
            // Move obstacles
            Console.Clear();

            DrawTheGrass(grass);// print grass
            DrawSafetyZone(scoreWindowBuffer);// print save zone 
            DrawWater();//draw water


            for (int i = 0; i < cars.Count; i++)// move cars
            {
                PrintOnPosition(cars[i].x, cars[i].y, cars[i].c, cars[i].color);
                x = cars[i].x;
                x++;
                y = cars[i].y;
                c = cars[i].c;
                color = cars[i].color;

                cars.Remove(cars[i]);
                cars.Insert(i, new Object(x, y, c, color));
            }

            for (int i = 0; i < woods.Count; i++)// move woods
            {
                if (woods[i].x < Console.WindowWidth - woods[i].c.Length - 3 && woods[i].y % 2 == 0)
                {
                    PrintOnPosition(woods[i].x, woods[i].y, woods[i].c, woods[i].color);
                    x = woods[i].x;
                    x += 2;
                    y = woods[i].y;
                    c = woods[i].c;
                    color = woods[i].color;

                    woods.Remove(woods[i]);
                    woods.Insert(i, new Object(x, y, c, color));

                }
                else
                {
                    PrintOnPosition(woods[i].x, woods[i].y, woods[i].c, woods[i].color);
                    x = woods[i].x;
                    x++;
                    y = woods[i].y;
                    c = woods[i].c;
                    color = woods[i].color;

                    woods.Remove(woods[i]);
                    woods.Insert(i, new Object(x, y, c, color));
                }
            }

            // check if the car is in the end of the console window and print it in the begging of the console. remove car and add other with new cordinates//
            for (int i = 0; i < cars.Count; i++)
            {
                if (cars[i].x == Console.WindowWidth + 1 - cars[i].c.Length)
                {
                    x = 0;
                    y = cars[i].y;
                    c = cars[i].c;
                    color = cars[i].color;
                    cars.Remove(cars[i]);
                    cars.Insert(i, new Object(x, y, c, color));
                }
            }

            for (int i = 0; i < woods.Count; i++)// check if the wood is in the end of the console window and print it in the begging of the console. remove car and add other with new cordinates//
            {
                if (woods[i].x == Console.WindowWidth + 1 - woods[i].c.Length)
                {
                    x = 0;
                    y = woods[i].y;
                    c = woods[i].c;
                    color = woods[i].color;
                    woods.Remove(woods[i]);
                    woods.Insert(i, new Object(x, y, c, color));
                }
            }

            // collusion detection //////////////////////////////////
            PrintOnPosition(frog.x = data[1], frog.y = data[2], frog.c, frog.color);// print the  frog
            foreach (var car in cars)
            {
                carCord = car.x;
                frogCord = frog.x;

                if (car.c.Length == 3)
                {
                    if ((frogCord == (carCord + 1) && frog.y == car.y) || (frogCord == (carCord - 1) && frog.y == car.y) || (frogCord == (carCord - 2) && frog.y == car.y))
                    {
                        if (lives == 0) GameOver();
                        Died();
                        frog.x = Console.WindowWidth / 2;
                        frog.y = Console.WindowHeight - scoreWindowBuffer;
                    }
                }

                if (car.c.Length == 2)
                {
                    if ((frogCord == carCord && frog.y == car.y) || (frogCord == (carCord - 1) && frog.y == car.y))
                    {
                        if (lives == 0) GameOver();
                        Died();
                        frog.x = Console.WindowWidth / 2;
                        frog.y = Console.WindowHeight - scoreWindowBuffer;
                    }
                }
            }


            //////////////////// colusion with woods and water  ///////////

            foreach (var wood in woods)
            {
                woodCord = wood.x;
                frogCord = frog.x;


                if (frog.y < 7 && frog.y > 1)
                {

                    if ((frogCord == (woodCord + 1) && frog.y == wood.y) || (frogCord == (woodCord) && frog.y == wood.y) || (frogCord == (woodCord + 2) && frog.y == wood.y) ||
                        (frogCord == (woodCord + 3) && frog.y == wood.y) || (frogCord == (woodCord + 4) && frog.y == wood.y) || (frogCord == (woodCord + 5) && frog.y == wood.y))
                    {

                        Console.WriteLine("I am here");
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        if (lives == 0) GameOver();
                        Died();
                        frog.x = Console.WindowWidth / 2;
                        frog.y = Console.WindowHeight - scoreWindowBuffer;

                    }
                }
            }


            // print score window
            PrintOnPosition(0, Console.WindowHeight - 3, "Lives: " + lives, ConsoleColor.Cyan);
            PrintOnPosition(0, Console.WindowHeight - 2, "Score: " + score, ConsoleColor.Cyan);

            Thread.Sleep(300);
        }
    }
}
