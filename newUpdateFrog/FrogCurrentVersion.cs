﻿﻿﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using System.Threading;
using System.IO;

class FrogCurr
{
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
                Console.ForegroundColor = ConsoleColor.Green;
                if (i == 1 && j == 1) Console.ForegroundColor = ConsoleColor.Red;
                if (i == 1 && j == 7) Console.ForegroundColor = ConsoleColor.Blue;
                if (i == 1 && j == 15) Console.ForegroundColor = ConsoleColor.Yellow;
                if (i == 1 && j == 22) Console.ForegroundColor = ConsoleColor.DarkMagenta;
                if (i == 1 && j == 28) Console.ForegroundColor = ConsoleColor.Gray;
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

        Console.SetCursorPosition(0, Console.WindowHeight / 2 - scoreWindowBuffer + 4);
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(new string('=', Console.WindowWidth));
    }// draw safety zone 

    public static void DrawWater()
    {
        Console.ResetColor();
        Console.SetCursorPosition(0, 2);
        Console.ForegroundColor = ConsoleColor.Blue;
        for (int i = 0; i < 9; i++)
        {
            Console.Write(new string('=', Console.WindowWidth));
        }
    }
    public static int[] Moves(Object frog, int[] data, int scoreWindowBuffer)
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

    public static void FillListOfCars(List<Object> cars, int x)
    {
        int rowCounter = Console.WindowHeight - x - 1;
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

    static void Lived()
    {
        Console.Clear();
        score += 50;
        PrintOnPosition(10, 5, "Hooray!!!!", ConsoleColor.Red);
        Thread.Sleep(500);
    }
    static void Finish()
    {
        Console.Clear();
        PrintOnPosition(8, 5, "You win!", ConsoleColor.Red);
        PrintOnPosition(6, 6, "Your score is " + score, ConsoleColor.Red);
        PrintOnPosition(4, 7, "Press [enter] for exit", ConsoleColor.Red);
        Console.ReadLine();
        Environment.Exit(0);

    }



    static void Main()
    {

        Console.BufferHeight = Console.WindowHeight = 22;
        Console.BufferWidth = Console.WindowWidth = 30;
        int n = Console.WindowWidth;

        char[,] grass = new char[2, n];// grass array 
        FillGrass(grass);// 
        bool[] holesToFill = new bool[5];

        int scoreWindowBuffer = 4;
        int rockField = 9;

        Object frog = new Object();
        frog.x = Console.WindowWidth / 2;
        frog.y = Console.WindowHeight - scoreWindowBuffer;
        frog.c = "X";
        frog.color = ConsoleColor.Yellow;
        int[] data = new int[3] { 0, frog.x, frog.y };

        List<Object> rocks = new List<Object>();

        List<Object> cars = new List<Object>(18);
        FillListOfCars(cars, scoreWindowBuffer);

        Random randomGenerator = new Random();

        int x;
        int y;
        string c;
        ConsoleColor color;

        while (true)
        {



            // Move frog
            Moves(frog, data, scoreWindowBuffer);
            if (data[0] == 1)//chek for exit 0=exit
            {
                return;
            }


            // Rocks
            Object newRock = new Object();
            newRock.color = ConsoleColor.DarkGray;
            newRock.x = randomGenerator.Next(0, Console.WindowWidth - 1);
            newRock.y = 1;
            newRock.c = "###";
            rocks.Add(newRock);


            List<Object> newList = new List<Object>();
            for (int i = 0; i < rocks.Count; i++)
            {
                Object oldRock = rocks[i];
                Object movingRocks = new Object();
                movingRocks.x = oldRock.x;
                if (oldRock.y > 9) continue;
                else movingRocks.y = oldRock.y + 1;

                movingRocks.c = oldRock.c;
                movingRocks.color = oldRock.color;
                if (movingRocks.c == "#" && oldRock.y == frog.y && oldRock.x == frog.x)
                {
                    if (lives == 0) GameOver();
                    Died();

                }
                if (movingRocks.y < Console.WindowHeight)
                {
                    newList.Add(movingRocks);
                }
            }
            rocks = newList;



            Console.Clear();
            DrawTheGrass(grass);// print grass
            DrawSafetyZone(scoreWindowBuffer);// print save zone 
            DrawWater();//draw water



            // Print the rocks
            foreach (Object rock in rocks)
            {
                PrintOnPosition(rock.x, rock.y, rock.c, rock.color);
            }

            // Move the cars
            for (int i = 0; i < cars.Count; i++)
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

            // print the  frog
            PrintOnPosition(frog.x = data[1], frog.y = data[2], frog.c, frog.color);

            //collusion detection//////////////////////////////////
            foreach (var rock in rocks)
            {
                int rockCord = rock.x;

                if ((frog.x == rockCord && frog.y == rock.y) || (frog.x == rockCord + 1 && frog.y == rock.y) || (frog.x == rockCord + 2 && frog.y == rock.y))
                {
                    if (lives == 0) GameOver();
                    Died();
                    frog.x = Console.WindowWidth / 2;
                    frog.y = Console.WindowHeight - scoreWindowBuffer;

                }
            }

            foreach (var car in cars)
            {
                int carCord = car.x;

                if (car.c.Length == 3)
                {
                    if ((frog.x == (carCord + 1) && frog.y == car.y) || (frog.x == (carCord - 1) && frog.y == car.y) || (frog.x == (carCord - 2) && frog.y == car.y) || (frog.x == car.x && frog.y == car.y))
                    {
                        if (lives == 0) GameOver();
                        Died();
                        frog.x = Console.WindowWidth / 2;
                        frog.y = Console.WindowHeight - scoreWindowBuffer;

                    }
                }

                if (car.c.Length == 2)
                {
                    if ((frog.x == carCord && frog.y == car.y) || (frog.x == (carCord - 1) && frog.y == car.y))
                    {
                        if (lives == 0) GameOver();
                        Died();
                        frog.x = Console.WindowWidth / 2;
                        frog.y = Console.WindowHeight - scoreWindowBuffer;

                    }
                }
            }
            CheckIsInHole(frog.x, frog.y, grass);
            if (CheckIsInHole(frog.x, frog.y, grass) == true)
            {
                if (grass[1, 1] == 'X' && !holesToFill[0])
                {
                    Lived();
                    frog.x = Console.WindowWidth / 2;
                    frog.y = Console.WindowHeight - scoreWindowBuffer;
                    holesToFill[0] = true;
                }
                if (grass[1, 7] == 'X' && !holesToFill[1])
                {
                    Lived();
                    frog.x = Console.WindowWidth / 2;
                    frog.y = Console.WindowHeight - scoreWindowBuffer;
                    holesToFill[1] = true;
                }
                if (grass[1, 15] == 'X' && !holesToFill[2])
                {
                    Lived();
                    frog.x = Console.WindowWidth / 2;
                    frog.y = Console.WindowHeight - scoreWindowBuffer;
                    holesToFill[2] = true;
                }

                if (grass[1, 22] == 'X' && !holesToFill[3])
                {
                    Lived();
                    frog.x = Console.WindowWidth / 2;
                    frog.y = Console.WindowHeight - scoreWindowBuffer;
                    holesToFill[3] = true;
                }
                if (grass[1, 28] == 'X' && !holesToFill[4])
                {
                    Lived();
                    frog.x = Console.WindowWidth / 2;
                    frog.y = Console.WindowHeight - scoreWindowBuffer;
                    holesToFill[4] = true;
                }

            }
            int count = 0;
            foreach (var hole in holesToFill)
            {
                if (hole == true)
                {
                    count++;
                }
            }
            if (count == 5)
            {
                Finish();
            }
            // print score window
            PrintOnPosition(0, Console.WindowHeight - 3, "Lives: " + lives, ConsoleColor.Cyan);
            PrintOnPosition(0, Console.WindowHeight - 2, "Score: " + score, ConsoleColor.Cyan);

            Thread.Sleep(300);
        }
    }

    private static bool CheckIsInHole(int x, int y, char[,] holes)
    {
        bool flag = false;
        if (x == 1 && y == 1)
        {
            holes[1, 1] = 'X';
            PrintOnPosition(1, 1, "X");
            flag = true;
        }
        if (x == 7 && y == 1)
        {
            holes[1, 7] = 'X';
            PrintOnPosition(1, 7, "X");
            flag = true;
        }
        if (x == 15 && y == 1)
        {
            holes[1, 15] = 'X';
            PrintOnPosition(1, 15, "X");
            flag = true;
        }
        if (x == 22 && y == 1)
        {
            holes[1, 22] = 'X';
            PrintOnPosition(22, 1, "X");
            flag = true;
        }
        if (x == 28 && y == 1)
        {
            holes[1, 28] = 'X';
            PrintOnPosition(28, 1, "X");
            flag = true;
        }
        return flag;
    }
}
